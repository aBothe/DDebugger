using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDebugger.TargetControlling;

namespace DDebugger.Breakpoints
{
	public class BreakpointManagement
	{
		public readonly Debuggee Debuggee;
		readonly List<Breakpoint> breakpoints = new List<Breakpoint>();

		public Breakpoint[] Breakpoints
		{
			get
			{
				return breakpoints.ToArray();
			}
		}

		internal BreakpointManagement(Debuggee debuggee)
		{
			this.Debuggee = debuggee;
		}

		public Breakpoint CreateBreakpoint(IntPtr address, bool enabled = true)
		{
			var bp = new Breakpoint(Debuggee.MainProcess, address)
			{
				Enabled = true
			};

			breakpoints.Add(bp);

			return bp;
		}

		public bool Remove(Breakpoint bp)
		{
			if (bp != null)
			{
				breakpoints.Remove(bp);
				bp.Disable();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Puts in a breakpoint at the program's entry point
		/// </summary>
		public void SetProgramEntryBreakpoint()
		{
			CreateBreakpoint(Debuggee.MainProcess.MainModule.StartAddress);
		}

		public Breakpoint ByAddress(IntPtr breakpointAddress)
		{
			foreach (var bp in breakpoints)
				if (bp.Address == breakpointAddress)
					return bp;
			return null;
		}
	}
}
