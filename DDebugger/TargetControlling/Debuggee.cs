using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CodeViewExaminer;
using DDebugger.Breakpoints;

namespace DDebugger.TargetControlling
{
	/// <summary>
	/// The central class needed for a debug session.
	/// </summary>
	public class Debuggee
	{
		#region Properties
		public readonly DebugProcess MainProcess;
		public readonly BreakpointManagement Breakpoints;
		public readonly ExecutableMetaInfo DebugInformation;
		public readonly MemoryManagement Memory;
		public readonly Stepping CodeStepping;

		readonly List<DebugEventListener> EventListeners = new List<DebugEventListener>();

		public DebugThread CurrentThread
		{
			get
			{
				return null;
			}
			set
			{

			}
		}
		#endregion

		#region Constructor/Init
		public Debuggee(Process proc, params DebugEventListener[] eventListeners)
		{
			MainProcess = new DebugProcess(this, proc);
			DebugInformation = ExecutableMetaInfo.ExtractFrom(MainProcess.ImageFile);
			
			Memory = new MemoryManagement(this);
			Breakpoints = new BreakpointManagement(this);
			CodeStepping = new Stepping(this);

			EventListeners.Add(new DefaultListener(this));
			EventListeners.AddRange(eventListeners);
		}
		#endregion

		#region Process controls
		/// <summary>
		/// Forces the debugged process to pause all threads.
		/// </summary>
		public void InterruptExecution()
		{

		}

		/// <summary>
		/// Resumes all thread activities.
		/// </summary>
		public void ContinueExecution()
		{

		}

		/// <summary>
		/// Blocking.
		/// Waits for the next debug event to occur.
		/// </summary>
		public void WaitForDebugEvent()
		{

		}
		#endregion

		#region Debug events
		class DefaultListener : DebugEventListener
		{
			public DefaultListener(Debuggee dbg) : base(dbg) { }
		}
		#endregion
	}
}
