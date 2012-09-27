using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CodeViewExaminer;
using DDebugger.TargetControlling;
using DDebugger.Win32;

namespace DDebugger
{
    public class DDebugger
    {
		public static readonly List<DebugEventListener> EventListeners = new List<DebugEventListener>();

		public static Debuggee Launch(string executable,
			string argumentString = null, string workingDirectory = null)
		{
			var si = new STARTUPINFO {
				cb = Marshal.SizeOf(typeof(STARTUPINFO)),
			};
			var pi = new PROCESS_INFORMATION();

			if (argumentString == string.Empty)
				argumentString = null;
			if (workingDirectory == string.Empty)
				workingDirectory = null;

			if (!API.CreateProcess(executable, argumentString, IntPtr.Zero, IntPtr.Zero, true,
				ProcessCreationFlags.CreateNewConsole | // Create extra console for the process
				ProcessCreationFlags.DebugOnlyThisProcess // Grant debugger access to the process
				,IntPtr.Zero, workingDirectory, ref si, out pi))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			
			var dbg = new Debuggee(executable, 
				pi.hProcess, pi.dwProcessId, 
				pi.hThread, pi.dwThreadId,
				ExecutableMetaInfo.ExtractFrom(executable)); 

			return dbg;
		}

		//public static Debuggee AttachTo(Process process)
		//{
		//	if (!API.DebugActiveProcess((uint)process.Id))
		//		throw new Win32Exception(Marshal.GetLastWin32Error());

		//	var dbg = new Debuggee(process.Main);
		//	dbg.WaitForDebugEvent();

		//	return dbg;
		//}
    }
}
