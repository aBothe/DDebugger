﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDebugger.TargetControlling;
using DDebugger.Win32;

namespace DDebugger.Breakpoints
{
	public class Breakpoint
	{
		#region Properties
		public readonly DebugProcess Owner;
		public readonly IntPtr Address;

		/// <summary>
		/// This variable stores the instruction code that will be overwritten by the int3 instruction when enabling the breakpoint.
		/// After a Disable() call, this instruction will be written into the process memory back then. 
		/// </summary>
		byte originalInstruction;

		int hitCount;
		public int HitCount { get { return hitCount; } }

		/// <summary>
		/// Only true for the time between the bp was hit and the normal execution afterwards.
		/// </summary>
		internal bool temporarilyDisabled;
		bool enabled;
		public bool Enabled { get { return enabled || !temporarilyDisabled; }
			set
			{
				if (value)
					Enable();
				else
					Disable();
			}
		}

		public Action Hit;
		#endregion

		#region Constructor
		public Breakpoint(DebugProcess proc, IntPtr breakpointAddress)
		{
			this.Owner = proc;
			this.Address = breakpointAddress;
		}
		#endregion

		#region Injection/Removal
		/// <summary>
		/// Toggles the breakpoint.
		/// </summary>
		/// <returns>Returns true if the breakpoint was set, false if the breakpoint was removed.</returns>
		public bool Toggle()
		{
			return Enabled != Enabled;
		}

		/// <summary>
		/// Inserts the breakpoint into the code
		/// </summary>
		public void Enable()
		{
			if (!enabled)
			{
				APIIntermediate.SetInterrupt(Owner.Handle,Address, out originalInstruction);
				enabled = true;
				temporarilyDisabled = false;
			}
		}

		/// <summary>
		/// Removes the breakpoint from the code
		/// </summary>
		public void Disable()
		{
			if(enabled)
				APIIntermediate.RemoveInterrupt(Owner.Handle, Address, originalInstruction);

			enabled = false;
		}
		#endregion

		/// <summary>
		/// Called when the breakpoint has been hit.
		/// Does only increase the hit counter and calls internals callback events.
		/// </summary>
		internal void WasHit()
		{
			hitCount++;

			if (Hit != null)
				Hit();
		}

		public void Dispose()
		{
			Disable();
		}
	}
}
