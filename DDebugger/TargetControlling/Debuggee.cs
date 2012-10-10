using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CodeViewExaminer;
using DDebugger.Breakpoints;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// The central class needed for a debug session.
	/// </summary>
	public class Debuggee : IDisposable
	{
		#region Properties
		readonly List<DebugProcess> processes=new List<DebugProcess>();
		public DebugProcess[] Processes { get { return processes.ToArray(); } }
		public DebugProcess MainProcess { get { return processes.Count == 0 ? null : processes[0]; } }

		public readonly BreakpointManagement Breakpoints;
		public readonly MemoryManagement Memory;
		public readonly Stepping CodeStepping;
		public DebugException LastException
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns true if the main process hasn't returned yet.
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return MainProcess != null && MainProcess.IsAlive;
			}
		}

		public DebugThread CurrentThread
		{
			get;
			private set;
		}
		#endregion

		#region Constructor/Init
		internal Debuggee(string executable,
			IntPtr procHandle,uint procId,
			IntPtr mainThreadHandle,uint mainThreadId,
			ExecutableMetaInfo emi = null)
		{
			// Note: The CodeView information extraction will be done per module, i.e. when the module/process is loaded into the memory.
			Memory = new MemoryManagement(this);
			Breakpoints = new BreakpointManagement(this);
			CodeStepping = new Stepping(this);

			var mProc = new DebugProcess(this,executable, procHandle, procId, mainThreadHandle, mainThreadId, emi);

			CurrentThread = mProc.MainThread;
			processes.Add(mProc);
		}

		public void Dispose()
		{
			foreach (var p in processes)
				p.Dispose();
		}
		#endregion

		#region Process controls
		/// <summary>
		/// Forces the debugged process to pause all threads.
		/// </summary>
		public void InterruptExecution()
		{

		}

		/// <summary>
		/// Resumes all thread activities.
		/// </summary>
		public void ContinueExecution(uint timeOut = Constants.INFINITE)
		{
			CodeStepping.ContinueUntilBreakpoint(timeOut);
		}


		/// <summary>
		/// Terminates the process.
		/// Afterwards, the debuggee object cannot be used anymore and will be disposed.
		/// </summary>
		public void Terminate(uint exitCode = 0)
		{
			if (MainProcess != null)
				MainProcess.Terminate(exitCode);
			Dispose();
		}

		/// <summary>
		/// Blocking.
		/// Waits for the next debug event to occur.
		/// </summary>
		public void WaitForDebugEvent(uint timeOut = Constants.INFINITE)
		{
			HandleDebugEvent(CodeStepping.WaitForDebugEventInternal(timeOut));
		}

		public DebugProcess ProcessById(uint Id)
		{
			foreach (var proc in processes)
				if (proc.Id == Id)
					return proc;
			return null;
		}
		#endregion

		#region Debug events
		void HandleDebugEvent(DebugEventData de)
		{
			var p = ProcessById(de.dwProcessId);
			var th = CurrentThread = p.ThreadById(de.dwThreadId);

			switch (de.dwDebugEventCode)
			{
				case DebugEventCode.EXCEPTION_DEBUG_EVENT:
					HandleException(th, de.Exception);
					break;


				case DebugEventCode.CREATE_PROCESS_DEBUG_EVENT:
					var cpi = de.CreateProcessInfo;

					if (MainProcess != null && de.dwProcessId == MainProcess.Id)
					{
						API.CloseHandle(cpi.hProcess);
						API.CloseHandle(cpi.hThread);
						API.CloseHandle(cpi.hFile);

						foreach(var l in DDebugger.EventListeners)
							l.OnCreateProcess(MainProcess);
						break;
					}

					// After a new process was created (also occurs after initial WaitForDebugEvent()!!),
					p = new DebugProcess(this,cpi, de.dwProcessId, de.dwThreadId);

					API.CloseHandle(cpi.hFile);
					
					// enlist it
					processes.Add(p);

					// and call the listeners
					foreach (var l in DDebugger.EventListeners)
						l.OnCreateProcess(p);
					break;


				case DebugEventCode.CREATE_THREAD_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);
					
					// Create new thread wrapper
					th = CurrentThread = new DebugThread(p, 
						de.CreateThread.hThread, 
						de.dwThreadId,
						de.CreateThread.lpStartAddress, 
						de.CreateThread.lpThreadLocalBase);
					// Register it to main process
					p.RegThread(th);

					// Call listeners
					foreach (var l in DDebugger.EventListeners)
						l.OnCreateThread(th);
					break;


				case DebugEventCode.EXIT_PROCESS_DEBUG_EVENT:
					
					foreach (var l in DDebugger.EventListeners)
						l.OnProcessExit(p, de.ExitProcess.dwExitCode);

					processes.Remove(p);
					p.Dispose();
					break;


				case DebugEventCode.EXIT_THREAD_DEBUG_EVENT:
					
					foreach (var l in DDebugger.EventListeners)
						l.OnThreadExit(th, de.ExitThread.dwExitCode);

					p.RemThread(th);
					th.Dispose();
					break;


				case DebugEventCode.LOAD_DLL_DEBUG_EVENT:
					var loadParam = de.LoadDll;

					var modName = APIIntermediate.GetModulePath(p.Handle, loadParam.lpBaseOfDll, loadParam.hFile);
					API.CloseHandle(loadParam.hFile);

					var mod = new DebugProcessModule(loadParam.lpBaseOfDll, modName, ExecutableMetaInfo.ExtractFrom(modName));
					p.RegModule(mod);

					foreach (var l in DDebugger.EventListeners)
						l.OnModuleLoaded(p, mod);
					break;


				case DebugEventCode.UNLOAD_DLL_DEBUG_EVENT:
					mod = p.ModuleByBase(de.UnloadDll.lpBaseOfDll);

					foreach (var l in DDebugger.EventListeners)
							l.OnModuleUnloaded(p, mod);

					p.RemModule(mod);
					break;


				case DebugEventCode.OUTPUT_DEBUG_STRING_EVENT:
					var message = APIIntermediate.ReadString(p.Handle,
						de.DebugString.lpDebugStringData,
						de.DebugString.fUnicode == 0 ? Encoding.ASCII : Encoding.Unicode,
						(int)de.DebugString.nDebugStringLength);

					foreach (var l in DDebugger.EventListeners)
						l.OnDebugOutput(th, message);
					break;
			}
		}

		void HandleException(DebugThread th, EXCEPTION_DEBUG_INFO e)
		{
			th.Context.Update();

			var code = e.ExceptionRecord.Code;
			// The instruction
			var targetSiteAddress = e.ExceptionRecord.ExceptionAddress;
			if (code == ExceptionCode.Breakpoint)
			{
				if (CodeStepping.lastUnhandledBreakpoint != null)
					CodeStepping.postBreakpointResetStepCompleted = true;

				var bp = Breakpoints.ByAddress(targetSiteAddress);
				if (bp == null)
					return;
				//APIIntermediate.GetCallStack_x86(th.OwnerProcess.Handle, (uint)th.StartAddress.ToInt32(), th.Context["ebp"], th.Context["eip"]);
				/*
				var sf = new STACKFRAME64();
				sf.AddrPC.Offset = th.Context["eip"];
				sf.AddrFrame.Offset = th.Context["ebp"];
				sf.AddrStack.Offset = th.Context["esp"];
				sf.AddrReturn.Mode = sf.AddrStack.Mode = sf.AddrPC.Mode = sf.AddrFrame.Mode = ADDRESS_MODE.AddrModeFlat;

				if(!API.StackWalk64(MachineType.i386, th.OwnerProcess.Handle, th.Handle, ref sf, ref th.Context.lastReadCtxt))
					throw new Win32Exception(Marshal.GetLastWin32Error());
				*/
				th.CurrentInstruction = targetSiteAddress;
				bp.Disable();
				bp.temporarilyDisabled = true;
				CodeStepping.lastUnhandledBreakpoint = bp;
				CodeStepping.lastUnhandledBreakpointThread = th;

				bp.WasHit();

				foreach (var l in DDebugger.EventListeners)
					l.OnBreakpoint(th, bp);
			}
			else if (code == ExceptionCode.SingleStep)
			{
				if (CodeStepping.lastUnhandledBreakpoint != null)
					CodeStepping.postBreakpointResetStepCompleted = true;
				else
					foreach (var l in DDebugger.EventListeners)
						l.OnStepComplete(th);
			}
			else
			{
				var ex = new DebugException(e.ExceptionRecord, e.dwFirstChance != 0);

				foreach (var l in DDebugger.EventListeners)
					l.OnException(th, ex);

				LastException = ex;
			}
		}

		public void RegisterListener(DebugEventListener listener)
		{
			DDebugger.EventListeners.Add(listener);
		}

		public bool UnregisterListener(DebugEventListener listener)
		{
			return DDebugger.EventListeners.Remove(listener);
		}
		#endregion
	}
}
