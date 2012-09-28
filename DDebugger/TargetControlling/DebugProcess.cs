using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CodeViewExaminer;
using CodeViewExaminer.CodeView;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugProcess : IDisposable
	{
		#region Properties / Constructor
		public readonly Debuggee Debuggee;

		public readonly uint Id;
		public readonly IntPtr Handle;

		readonly List<DebugProcessModule> modules = new List<DebugProcessModule>();
		public readonly DebugProcessModule MainModule;
		public DebugProcessModule[] Modules	{ get{ return modules.ToArray(); } }

		readonly List<DebugThread> threads = new List<DebugThread>();
		public readonly DebugThread MainThread;
		public DebugThread[] Threads { get { return threads.ToArray(); } }

		public uint ExitCode
		{
			get
			{
				uint exitCode = 0;
				if (!API.GetExitCodeProcess(Handle, out exitCode))
					throw new Win32Exception(Marshal.GetLastWin32Error());
				return exitCode;
			}
		}

		public bool IsAlive { get { return ExitCode == Constants.STILL_ACTIVE; } }

		public DebugProcess(Debuggee dbg,
			string executableFile,
			IntPtr processHandle, uint processId,
			IntPtr mainThreadHandle, uint mainThreadId,
			ExecutableMetaInfo emi)
		{
			this.Debuggee = dbg;
			Handle = processHandle;
			Id = processId;

			MainModule = new DebugProcessModule(new IntPtr(emi.PEHeader.OptionalHeader32.ImageBase),executableFile, emi);
			RegModule(MainModule);

			MainThread = new DebugThread(this, mainThreadHandle, mainThreadId, MainModule.StartAddress, IntPtr.Zero);
			RegThread(MainThread);
		}

		public DebugProcess(Debuggee dbg, Win32.CREATE_PROCESS_DEBUG_INFO info, uint id, uint threadId)
		{
			this.Debuggee = dbg;
			Handle = info.hProcess;
			Id = id == 0 ? API.GetProcessId(Handle) : id;
			
			var moduleFile = APIIntermediate.GetModulePath(Handle, info.lpBaseOfImage, info.hFile);

			// Deduce main module
			MainModule = new DebugProcessModule(info.lpBaseOfImage, moduleFile, ExecutableMetaInfo.ExtractFrom(moduleFile));
			RegModule(MainModule);
			
			// Create main thread
			MainThread = new DebugThread(this, 
				info.hThread, 
				threadId == 0 ? API.GetThreadId(info.hThread) : threadId, 
				info.lpStartAddress, 
				info.lpThreadLocalBase);
			RegThread(MainThread);
		}
		#endregion

		public DebugThread ThreadById(uint dwThreadId)
		{
			foreach (var th in threads)
				if (th.Id == dwThreadId)
					return th;
			return null;
		}

		public DebugProcessModule ModuleByBase(IntPtr dllBase)
		{
			foreach (var mod in modules)
				if (mod.ImageBase == dllBase)
					return mod;

			return null;
		}

		internal void RegModule(DebugProcessModule mod)
		{
			modules.Add(mod);
		}

		internal bool RemModule(DebugProcessModule mod)
		{
			return modules.Remove(mod);
		}

		internal void RegThread(DebugThread th)
		{
			threads.Add(th);
		}

		internal bool RemThread(DebugThread th)
		{
			return threads.Remove(th);
		}

		public void Dispose()
		{
			API.CloseHandle(Handle);
		}

		public void ResumeExecution()
		{
			foreach (var th in threads)
				th.Resume();
		}

		/// <summary>
		/// Suspends the execution of all process threads
		/// </summary>
		public void SuspendExecution()
		{
			foreach (var th in threads)
				th.Suspend();
		}

		public void Terminate(uint exitCode = 0)
		{
			API.TerminateProcess(Handle, exitCode);
			API.CloseHandle(Handle);
		}
	}
}
