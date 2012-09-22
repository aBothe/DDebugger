using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	public class DebugThread
	{
		#region Properties / Constructor
		public readonly DebugProcess OwnerProcess;
		public readonly ProcessThread Thread;
		public readonly uint ThreadId;
		public readonly IntPtr ThreadHandle;

		public Stackframe[] Stackframes
		{
			get
			{
				return null;
			}
		}

		public DebugThread(DebugProcess owner, uint threadId)
		{
			OwnerProcess = owner;
			this.ThreadId = threadId;

			foreach(ProcessThread pth in owner.Process.Threads)
				if (pth.Id == threadId)
				{
					this.Thread = pth;
					break;
				}
		}
		#endregion
	}
}
