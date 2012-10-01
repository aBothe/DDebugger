using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugThread : IDisposable
	{
		#region Properties / Constructor
		public readonly DebugProcess OwnerProcess;
		public readonly DebugThreadContext Context;

		public readonly uint Id;
		public readonly IntPtr Handle;
		public readonly IntPtr StartAddress;
		public readonly IntPtr ThreadBase;

		public Stackframe[] Stackframes
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Note: When setting the current instruction, ensure that the context was updated before!
		/// Otherwise, the thread and its context will be obstructed and end in program termination.
		/// </summary>
		public IntPtr CurrentInstruction
		{
			get {
				Context.Update();

				return new IntPtr(Context.lastReadCtxt.eip);
			}
			set
			{
				Context.lastReadCtxt.eip = (uint)value.ToInt32();
			}
		}

		public DebugThread(DebugProcess owner, IntPtr handle, uint threadId, IntPtr startAddress, IntPtr threadBase)
		{
			this.OwnerProcess = owner;
			
			this.Handle = handle;
			this.Id = threadId;
			this.StartAddress = startAddress;
			this.ThreadBase = threadBase;

			this.Context = new DebugThreadContext(this);
		}
		#endregion

		public void Suspend()
		{
			unchecked
			{
				if (API.SuspendThread(Handle) == (uint)-1)
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		public void Resume()
		{
			// Resume the thread until the suspend count equals 0
			int r;
			do
			{
				if ((r = API.ResumeThread(Handle)) == -1)
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			while (r > 0);
		}

		public void Terminate(int exitCode)
		{
			if (!API.TerminateThread(Handle, exitCode))
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public void Dispose()
		{
			API.CloseHandle(this.Handle);
		}
	}
}
