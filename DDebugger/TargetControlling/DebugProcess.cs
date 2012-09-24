using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

		public DebugProcess(Win32.CREATE_PROCESS_DEBUG_INFO info, uint id, uint threadId)
		{
			Handle = info.hProcess;
			Id = id == 0 ? API.GetProcessId(Handle) : id;
			
			// Deduce main module
			MainModule = new DebugProcessModule(info.lpBaseOfImage, info.lpStartAddress,
				DebugProcessModule.GetModuleFileName(info.lpImageName, info.fUnicode != 0, id),
				CodeViewReader.Read(info.hFile, (long)info.dwDebugInfoFileOffset, (long)info.nDebugInfoSize));
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
	}
}
