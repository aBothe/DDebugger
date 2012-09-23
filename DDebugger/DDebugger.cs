using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DDebugger.TargetControlling;
using DDebugger.Win32;

namespace DDebugger
{
    public class DDebugger
    {
		public static Debuggee Launch(string executable,
			string argumentString = null, string workingDirectory = null, bool suspended = true)
		{
			var si = new STARTUPINFO {
				cb = Marshal.SizeOf(typeof(STARTUPINFO))
			};
			var pi = new PROCESS_INFORMATION();

			if (argumentString == string.Empty)
				argumentString = null;
			if (workingDirectory == string.Empty)
				workingDirectory = null;

			if (!API.CreateProcess(executable, argumentString, IntPtr.Zero, IntPtr.Zero, true,
				ProcessCreationFlags.CreateNewConsole |
				(suspended ? ProcessCreationFlags.CreateSuspended : 0) |
				ProcessCreationFlags.DebugOnlyThisProcess,
				IntPtr.Zero, workingDirectory, ref si, out pi))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			// Grant debugger access to the process
			if (!API.DebugActiveProcess(pi.dwProcessId))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			// NOTE What to do with the main thread id/handles?
			var dbg = new Debuggee(Process.GetProcessById((int)pi.dwProcessId));

			API.CloseHandle(pi.hProcess);
			API.CloseHandle(pi.hThread);

			return dbg;
		}

		public static Debuggee AttachTo(Process process)
		{
			if (!API.DebugActiveProcess((uint)process.Id))
				throw new Win32Exception(Marshal.GetLastWin32Error());

			return new Debuggee(process);
		}
    }
}
