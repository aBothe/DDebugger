using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDebugger.TargetControlling;

namespace DDebugger.Breakpoints
{
	public class Stepping
	{
		public readonly Debuggee Debuggee;
		public readonly BreakpointManagement Breakpoints;

		public Stepping(Debuggee dbg)
		{
			this.Debuggee = dbg;
			this.Breakpoints = dbg.Breakpoints;
		}

		void StepIn()
		{

		}

		void StepOver()
		{

		}

		void StepOut()
		{

		}
	}
}
