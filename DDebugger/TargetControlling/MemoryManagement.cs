using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// Provides methods to read from or to write into a process' virtual memory.
	/// </summary>
	public class MemoryManagement
	{
		public readonly Debuggee Debuggee;

		public MemoryManagement(Debuggee dbg)
		{
			this.Debuggee = dbg;
		}
	}
}
