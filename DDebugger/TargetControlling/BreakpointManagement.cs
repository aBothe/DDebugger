using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	public class BreakpointManagement
	{
		public readonly Debuggee Debuggee;

		internal BreakpointManagement(Debuggee debuggee)
		{
			this.Debuggee = debuggee;
		}
	}
}
