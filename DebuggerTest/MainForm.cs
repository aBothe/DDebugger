using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using DDebugger;
using DDebugger.TargetControlling;
using System.IO;
using CodeViewExaminer;

namespace DebuggerTest
{
	public partial class MainForm : Form
	{
		TextEditor editor = new TextEditor();
		EventLogger logger;
		Debuggee dbg;

		public MainForm()
		{
			InitializeComponent();
			InitEditor();
		}

		void InitEditor()
		{
			elementHost1.Child = editor;
			editor.ShowLineNumbers = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var exe = input_executable.Text;

			if (!Path.IsPathRooted(exe))
				exe = Environment.CurrentDirectory + "\\" + exe;

			if (!File.Exists(exe))
			{
				MessageBox.Show(exe + " doesn't exist!", "Execute program", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DDebugger.DDebugger.EventListeners.Clear();
			DDebugger.DDebugger.EventListeners.Add(new EventLogger(this, dbg));
			dbg = DDebugger.DDebugger.Launch(exe);

			dbg.WaitForDebugEvent();
			//dbg.Breakpoints.SetProgramEntryBreakpoint();
			dbg.Breakpoints.CreateBreakpoint(new IntPtr(0x0041C6B4u));
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
				dbg.MainProcess.Terminate(0);
		}

		class EventLogger : DebugEventListener
		{
			readonly MainForm form;
			public EventLogger(MainForm f,Debuggee dbg) : base(dbg) {
				form = f;
			}

			public override void OnCreateProcess(DebugProcess newProcess)
			{
				form.eventLogBox.AppendText(
					"Program " + newProcess.MainModule.ImageFile+" was launched\r\n"+
					"\tPID #"+newProcess.Id+"\r\n\tDebug information found: "+
					(newProcess.MainModule.ContainsSymbolData?"yes":"no") + "\r\n");
			}

			public override void OnBreakComplete(DebugThread thread)
			{
				base.OnBreakComplete(thread);
			}

			public override void OnBreakpoint(DebugThread thread, DDebugger.Breakpoints.Breakpoint breakpoint)
			{
				base.OnBreakpoint(thread, breakpoint);
			}

			public override void OnCreateThread(DebugThread newThread)
			{
				base.OnCreateThread(newThread);
			}

			public override void OnDebugOutput(DebugThread thread, string outputString)
			{
				base.OnDebugOutput(thread, outputString);
			}

			public override void OnException(DebugThread thread, DebugException exception)
			{
				base.OnException(thread, exception);
			}

			public override void OnModuleLoaded(DebugProcess mainProcess, DebugProcessModule module)
			{
				form.eventLogBox.AppendText(module.ImageFile+" loaded (0x"+string.Format("{0,8:X}",module.ImageBase)+")\r\n");
			}

			public override void OnModuleUnloaded(DebugProcess mainProcess, DebugProcessModule module)
			{
				if (module != null)
					form.eventLogBox.AppendText(module.ImageFile + " unloaded (0x" + string.Format("{0,8:X}", module.ImageBase) + ")\r\n");
				else
					form.eventLogBox.AppendText("Some module was unloaded\r\n");
			}

			public override void OnProcessExit(DebugProcess process, uint exitCode)
			{
				base.OnProcessExit(process, exitCode);
			}

			public override void OnStepComplete(DebugThread thread)
			{
				base.OnStepComplete(thread);
			}

			public override void OnThreadExit(DebugThread thread, uint exitCode)
			{
				base.OnThreadExit(thread, exitCode);
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
			{
				dbg.ContinueUntilBreakpoint(500);
			}
		}

		private void button8_Click(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
				dbg.WaitForDebugEvent(50);
		}
	}
}
