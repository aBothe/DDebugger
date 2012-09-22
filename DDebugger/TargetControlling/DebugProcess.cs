using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	public class DebugProcess
	{
		#region Properties / Constructor
		public readonly Debuggee Debuggee;
		public readonly Process Process;

		public string ImageFile
		{
			get
			{
				return Process.MainModule.FileName;
			}
		}

		public Process[] ChildProcesses
		{
			get
			{
				return null;
			}
		}

		public DebugThread[] Threads
		{
			get
			{
				return null;
			}
		}

		public DebugProcess(Debuggee dbg, Process proc)
		{
			this.Debuggee = dbg;
			this.Process = proc;
		}
		#endregion
	}
}
