using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// The central class needed for a debug session.
	/// </summary>
	public class Debuggee
	{
		#region Properties
		public readonly Process MainProcess;

		public Process[] ChildProcesses
		{
			get
			{
				return null;
			}
		}

		public readonly BreakpointManagement Breakpoints;

		public DebugThread[] Threads
		{
			get
			{
				return null;
			}
		}

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

		#region Constructor
		public Debuggee(Process proc)
		{
			this.MainProcess = proc;
		}
		#endregion
	}
}
