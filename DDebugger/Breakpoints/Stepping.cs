using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDebugger.TargetControlling;
using DDebugger.Win32;

namespace DDebugger.Breakpoints
{
	public class Stepping
	{
		#region Properties
		public readonly Debuggee dbg;
		public readonly BreakpointManagement Breakpoints;

		private DebugEventData lastDebugEvent = new DebugEventData();
		internal Breakpoint lastUnhandledBreakpoint;
		internal DebugThread lastUnhandledBreakpointThread;
		internal bool postBreakpointResetStepCompleted = false;
		#endregion

		#region Ctor/Init
		public Stepping(Debuggee dbg)
		{
			this.dbg = dbg;
			this.Breakpoints = dbg.Breakpoints;
		}
		#endregion


		/// <summary>
		///	Continues the program execution until the next breakpoint, exception or step interrupt.
		/// </summary>
		public void ContinueUntilBreakpoint(uint maxTimeOut = Constants.INFINITE)
		{
			do
			{
				if (lastUnhandledBreakpoint != null)
					RestoreLastBreakpoint();

				dbg.MainProcess.ResumeExecution();
				API.ContinueDebugEvent(dbg.MainProcess.Id, dbg.MainProcess.Threads[0].Id, ContinueStatus.DBG_CONTINUE);

				dbg.WaitForDebugEvent(maxTimeOut);
			}
			while (lastDebugEvent.dwDebugEventCode != DebugEventCode.EXCEPTION_DEBUG_EVENT);
		}

		void RestoreLastBreakpoint()
		{
			// Immediately after the breakpoint was hit, the bp code was taken out of the code
			// and the instruction pointer was decreased, so the original instruction will be executed then afterwards.

			// 1) Enable single-stepping (SS flag)
			// 2) Step one forward (execute original instruction)
			// 3) Re-enable breakpoint
			// 4) (optionally) disable single-stepping again

			// 1)
			lastUnhandledBreakpointThread.Context.TrapFlagSet = true;

			// 2)
			API.ContinueDebugEvent(lastUnhandledBreakpointThread.OwnerProcess.Id, lastUnhandledBreakpointThread.Id, ContinueStatus.DBG_CONTINUE);

			postBreakpointResetStepCompleted = false;
			dbg.WaitForDebugEvent();

			if (!postBreakpointResetStepCompleted)
				return;

			// 3)
			lastUnhandledBreakpoint.Enable();

			// 4)
			lastUnhandledBreakpointThread.Context.TrapFlagSet = false;

			lastUnhandledBreakpointThread = null;
			lastUnhandledBreakpoint = null;
		}

		public DebugEventData WaitForDebugEventInternal(uint timeOut)
		{
			lastDebugEvent.ApplyFrom(APIIntermediate.WaitForDebugEvent(timeOut));
			return lastDebugEvent;
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
