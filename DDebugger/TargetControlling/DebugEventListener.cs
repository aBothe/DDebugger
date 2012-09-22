using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

		public virtual void OnCreateProcess(Process newProcess) { }
		public virtual void OnCreateThread() { }
		public virtual void OnProcessExit() { }
		public virtual void OnThreadExit() { }

		public virtual void OnException() { }
		public virtual void OnLibraryLoaded() { }
		public virtual void OnLibraryUnloaded() { }
		public virtual void OnDebugOutput(string outputString) { }
		public virtual void OnRIPEvent() { }
	}
}
