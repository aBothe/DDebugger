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

		readonly List<DebugEventListener> EventListeners = new List<DebugEventListener>();

		public DebugThread CurrentThread
		{
			get
			{
				return null;
			}
			set
			{

			}
		}
		#endregion

		#region Constructor/Init
		public Debuggee()
		{
			// Note: The CodeView information extraction will be done per module, i.e. when the module/process is loaded into the memory.

			Memory = new MemoryManagement(this);
			Breakpoints = new BreakpointManagement(this);
			CodeStepping = new Stepping(this);
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
		public void ContinueExecution()
		{

		}

		/// <summary>
		/// Blocking.
		/// Waits for the next debug event to occur.
		/// </summary>
		public void WaitForDebugEvent(uint timeOut = Constants.INFINITE)
		{
			HandleDebugEvent(APIIntermediate.WaitForDebugEvent(timeOut));
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
		void HandleDebugEvent(DEBUG_EVENT de)
		{
			switch (de.dwDebugEventCode)
			{
				case DebugEventCode.EXCEPTION_DEBUG_EVENT:
					var p = ProcessById(de.dwProcessId);
					var th = p.ThreadById(de.dwThreadId);

					HandleException(th, de.Exception);
					break;


				case DebugEventCode.CREATE_PROCESS_DEBUG_EVENT:
					// After a new process was created (also occurs after initial WaitForDebugEvent()!!),
					p = new DebugProcess(de.CreateProcessInfo, de.dwProcessId, de.dwThreadId);

					API.CloseHandle(de.CreateProcessInfo.hFile);
					
					// enlist it
					processes.Add(p);

					// and call the listeners
					foreach (var l in EventListeners)
						l.OnCreateProcess(p);
					break;


				case DebugEventCode.CREATE_THREAD_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);
					
					// Create new thread wrapper
					th = new DebugThread(p, 
						de.CreateThread.hThread, 
						de.dwThreadId,
						de.CreateThread.lpStartAddress, 
						de.CreateThread.lpThreadLocalBase);
					// Register it to main process
					p.RegThread(th);

					// Call listeners
					foreach (var l in EventListeners)
						l.OnCreateThread(th);
					break;


				case DebugEventCode.EXIT_PROCESS_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);

					foreach (var l in EventListeners)
						l.OnProcessExit(p, de.ExitProcess.dwExitCode);

					processes.Remove(p);
					p.Dispose();
					break;


				case DebugEventCode.EXIT_THREAD_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);
					th = p.ThreadById(de.dwThreadId);

					foreach (var l in EventListeners)
						l.OnThreadExit(th, de.ExitThread.dwExitCode);

					p.RemThread(th);
					th.Dispose();
					break;


				case DebugEventCode.LOAD_DLL_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);

					var mod = new DebugProcessModule(
						de.LoadDll.lpBaseOfDll, IntPtr.Zero,
						DebugProcessModule.GetModuleFileName(
							de.LoadDll.lpImageName, 
							de.LoadDll.fUnicode!=0, 
							0),
						CodeViewExaminer.CodeView.CodeViewReader.Read(
							de.LoadDll.hFile, 
							de.LoadDll.dwDebugInfoFileOffset, 
							de.LoadDll.nDebugInfoSize));
					p.RegModule(mod);

					API.CloseHandle(de.LoadDll.hFile);

					foreach (var l in EventListeners)
						l.OnModuleLoaded(p, mod);
					break;


				case DebugEventCode.UNLOAD_DLL_DEBUG_EVENT:
					p = ProcessById(de.dwProcessId);

					mod = p.ModuleByBase(de.UnloadDll.lpBaseOfDll);

					foreach (var l in EventListeners)
						l.OnModuleUnloaded(p, mod);

					p.RemModule(mod);
					break;


				case DebugEventCode.OUTPUT_DEBUG_STRING_EVENT:
					p = ProcessById(de.dwProcessId);
					th = p.ThreadById(de.dwThreadId);

					var message = APIIntermediate.ReadString(p.Handle,
						de.DebugString.lpDebugStringData,
						de.DebugString.fUnicode == 0 ? Encoding.ASCII : Encoding.Unicode,
						(int)de.DebugString.nDebugStringLength);

					foreach (var l in EventListeners)
						l.OnDebugOutput(th, message);
					break;
			}
		}

		void HandleException(DebugThread th, EXCEPTION_DEBUG_INFO e)
		{
			var code = e.ExceptionRecord.Code;
			if (code == ExceptionCode.Breakpoint)
			{
				var bp = Breakpoints.ByAddress(e.ExceptionRecord.ExceptionAddress);
			}
			else if (code == ExceptionCode.SingleStep)
			{

			}
			else
			{
				var ex = new DebugException(e.ExceptionRecord, e.dwFirstChance != 0);

				foreach (var l in EventListeners)
					l.OnException(th, ex);

				LastException = ex;
			}
		}

		public void RegisterListener(DebugEventListener listener)
		{
			EventListeners.Add(listener);
		}

		public bool UnregisterListener(DebugEventListener listener)
		{
			return EventListeners.Remove(listener);
		}
		#endregion
	}
}
