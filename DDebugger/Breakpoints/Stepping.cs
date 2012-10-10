using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDebugger.Disassembly;
using DDebugger.TargetControlling;
using DDebugger.Win32;

namespace DDebugger.Breakpoints
{
	public class Stepping
	{
		#region Properties
		/// <summary>
		/// True if debug information shall be used for stepping across code lines, not across (e.g. single) assembler instructions
		/// </summary>
		public bool SourceBoundStepping = false;
		public readonly Debuggee Debuggee;
		public readonly BreakpointManagement Breakpoints;

		private DebugEventData lastDebugEvent = new DebugEventData();

		// Required for the time between a breakpoint interrupt and the continuation of the thread/process.
		internal bool expectsSingleStep;
		internal Breakpoint lastUnhandledBreakpoint;
		internal DebugThread lastUnhandledBreakpointThread;
		internal bool postBreakpointResetStepCompleted = false;
		#endregion

		#region Ctor/Init
		public Stepping(Debuggee dbg)
		{
			this.Debuggee = dbg;
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
					SkipAndRestoreLastBreakpoint();

				if (!Debuggee.IsAlive)
					return;

				Debuggee.MainProcess.ResumeExecution();
				API.ContinueDebugEvent(Debuggee.MainProcess.Id, Debuggee.MainProcess.Threads[0].Id, ContinueStatus.DBG_CONTINUE);

				Debuggee.WaitForDebugEvent(maxTimeOut);
			}
			while (lastDebugEvent.dwDebugEventCode != DebugEventCode.EXCEPTION_DEBUG_EVENT);
		}

		void SkipAndRestoreLastBreakpoint(bool resetStepFlag = true)
		{
			if (lastUnhandledBreakpoint == null)
				throw new Exception("lastUnhandledBreakpoint must not be null");
			if (!lastUnhandledBreakpoint.temporarilyDisabled)
				throw new Exception("breakpoint must be marked as temporarily disabled!");
			if (lastUnhandledBreakpointThread == null)
				throw new Exception("lastUnhandledBreakpointThread must not be null");
			if (postBreakpointResetStepCompleted)
				throw new Exception("why is postBreakpointResetStepCompleted set to true?");

			// Immediately after the breakpoint was hit, the bp code was taken out of the code
			// and the instruction pointer was decreased, so the original instruction will be executed then afterwards.

			// 1) Enable single-stepping (SS flag)
			// 2) Step one forward (execute original instruction)
			// 3) Re-enable breakpoint
			// 4) (optionally) disable single-stepping again

			// 1)
			lastUnhandledBreakpointThread.Context.TrapFlagSet = true;

			// 2)
			lastUnhandledBreakpointThread.ContinueDebugging();

			postBreakpointResetStepCompleted = false;
			Debuggee.WaitForDebugEvent();

			if (!postBreakpointResetStepCompleted)
				return;

			// 3)
			lastUnhandledBreakpoint.Enable();

			// 4)
			if(resetStepFlag)
				lastUnhandledBreakpointThread.Context.TrapFlagSet = false;

			lastUnhandledBreakpointThread = null;
			lastUnhandledBreakpoint = null;
			postBreakpointResetStepCompleted = false;
		}

		internal DebugEventData WaitForDebugEventInternal(uint timeOut)
		{
			lastDebugEvent.ApplyFrom(APIIntermediate.WaitForDebugEvent(timeOut));
			return lastDebugEvent;
		}

		void DoSingleStep(DebugThread th)
		{
			if (SourceBoundStepping)
				StepToNextSrcLine(th);
			else
				StepToNextInstruction(th);
		}

		/// <summary>
		/// Steps until there's an associated code location for the currently executed instruction 
		/// or if there aren't any debug information.
		/// </summary>
		void StepToNextSrcLine(DebugThread th)
		{
			string file = null;
			ushort line = 0;
			var mainMod = Debuggee.MainProcess.MainModule;
			var modMetaInfo = mainMod.ModuleMetaInfo;

			do{
				StepToNextInstruction(th);
			}
			while(Debuggee.IsAlive && 
				mainMod.ContainsSymbolData && 
				!modMetaInfo.TryDetermineCodeLocation((uint)Debuggee.CurrentThread.CurrentInstruction.ToInt32(), out file, out line));
		}

		/// <summary>
		/// Executes the next single code instruction.
		/// Returns false if the single step could be executed but wasn't completed due to a breakpoint or an other exception/debug event.
		/// </summary>
		bool StepToNextInstruction(DebugThread th)
		{
			if (!Debuggee.IsAlive)
				return false;

			if (lastUnhandledBreakpoint != null)
				SkipAndRestoreLastBreakpoint(false);

			bool lastStepState = th.Context.TrapFlagSet;
			if (!lastStepState)
				th.Context.TrapFlagSet = true;

			th.ContinueDebugging();

			expectsSingleStep = true;
			Debuggee.WaitForDebugEvent();

			//TODO: What if there's a non-ss exception?
			// Answer: return false

			if (!lastStepState)
				th.Context.TrapFlagSet = false;

			return !expectsSingleStep;
		}
		
		/// <summary>
		/// Executes the next instruction.
		/// </summary>
		public void StepIn(DebugThread th)
		{
			DoSingleStep(th);
		}

		/// <summary>
		/// See <see cref="StepIn"/>.
		/// If there's a call as next instruction, it'll be skipped.
		/// </summary>
		public void StepOver(DebugThread th)
		{
			var code = APIIntermediate.ReadArray<byte>(th.OwnerProcess.Handle, th.CurrentInstruction, DisAsm86.MaximumInstructionLength);

			int instructionLength = 0;
			var instrType = DisAsm86.GetInstructionType(code, false, out instructionLength);

			/*
			 * If there's a call, set a breakpoint right after the call to skip the called subroutine
			 */
			if (instrType == InstructionType.Call)
			{
				var bpAddr = IntPtr.Add(th.CurrentInstruction, instructionLength);

				var tempBreakPoint = Breakpoints.ByAddress(bpAddr);
				bool keepBpAfterStepComplete = false;
				if (keepBpAfterStepComplete = tempBreakPoint == null)
					tempBreakPoint = Breakpoints.CreateBreakpoint(bpAddr);

				th.ContinueDebugging();
				Debuggee.WaitForDebugEvent();

				if (!keepBpAfterStepComplete)
					Breakpoints.Remove(tempBreakPoint);
			}
			else
				StepIn(th);
		}

		/// <summary>
		/// Executes until the currently executed method's point of return has been reached.
		/// </summary>
		public void StepOut(DebugThread th)
		{
			var returnPtr = APIIntermediate.Read<IntPtr>(th.OwnerProcess.Handle, new IntPtr( th.Context.lastReadCtxt.ebp + 4));

			var tempBreakPoint = Breakpoints.ByAddress(returnPtr);
			bool keepBpAfterStepComplete = false;
			if (keepBpAfterStepComplete = tempBreakPoint == null)
				tempBreakPoint = Breakpoints.CreateBreakpoint(returnPtr);

			th.ContinueDebugging();
			Debuggee.WaitForDebugEvent();

			if (!keepBpAfterStepComplete)
				Breakpoints.Remove(tempBreakPoint);
		}
	}
}
