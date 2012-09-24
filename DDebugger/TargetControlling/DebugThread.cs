using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DDebugger.Win32;

namespace DDebugger.TargetControlling
{
	public class DebugThread : IDisposable
	{
		#region Properties / Constructor
		public readonly DebugProcess OwnerProcess;

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

		public DebugThread(DebugProcess owner, IntPtr handle, uint threadId, IntPtr startAddress, IntPtr threadBase)
		{
			this.OwnerProcess = owner;
			
			this.Handle = handle;
			this.Id = threadId;
			this.StartAddress = startAddress;
			this.ThreadBase = threadBase;
		}
		#endregion

		public void Dispose()
		{
			API.CloseHandle(this.Handle);
		}
	}
}
