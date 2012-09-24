using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DDebugger.Breakpoints;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// Will be passed to the DDebugger engine in order to listen to incoming debug events.
	/// </summary>
	public abstract class DebugEventListener
	{
		public readonly Debuggee Debuggee;

		public DebugEventListener(Debuggee dbg)
		{
			this.Debuggee = dbg;
		}

		public virtual void OnCreateProcess(DebugProcess newProcess) { }
		public virtual void OnCreateThread(DebugThread newThread) { }
		public virtual void OnProcessExit(DebugProcess process, uint exitCode) { }
		public virtual void OnThreadExit(DebugThread thread, uint exitCode) { }
		public virtual void OnModuleLoaded(DebugProcess mainProcess, DebugProcessModule module) { }
		public virtual void OnModuleUnloaded(DebugProcess mainProcess, DebugProcessModule module) { }
		public virtual void OnDebugOutput(DebugThread thread, string outputString) { }

		public virtual void OnException(DebugThread thread, DebugException exception) { }
		public virtual void OnBreakpoint(DebugThread thread, Breakpoint breakpoint) { }

		public virtual void OnStepComplete(DebugThread thread) { }
		public virtual void OnBreakComplete(DebugThread thread) { }
		//public virtual void OnRIPEvent() { }
	}
}
