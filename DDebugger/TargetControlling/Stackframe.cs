using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDebugger.TargetControlling
{
	public class Stackframe
	{
		public readonly IntPtr BasePointer;
		public readonly IntPtr CodeAddress;

		public Stackframe(IntPtr bp, IntPtr codeAddr)
		{
			this.BasePointer = bp;
			this.CodeAddress = codeAddr;
		}
	}
}
