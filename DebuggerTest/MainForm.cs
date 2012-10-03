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
using CodeViewExaminer.CodeView;

namespace DebuggerTest
{
	public partial class MainForm : Form
	{
		TextEditor editor = new TextEditor();
		public TextMarkerService MarkerStrategy;
		string lastOpenedFile;
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
			MarkerStrategy = new TextMarkerService(editor);
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
			dbg.Breakpoints.CreateBreakpoint(new IntPtr(0x004020c8u));
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
					"\tPID #"+newProcess.Id + "\r\n");

				form.list_AvailableSources.Clear();

				if (newProcess.MainModule.ContainsSymbolData)
				{
					foreach (var section in newProcess.MainModule.ModuleMetaInfo.CodeViewSection.Data.SubsectionDirectory.Sections)
						if(section is sstSrcModule)
						{
							var srcModule = (sstSrcModule)section;

							foreach (var f in srcModule.FileInfo)
								form.list_AvailableSources.Items.Add(new ListViewItem(f.SourceFileName) { 
										Tag = f
									});
						}
				}
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
				dbg.ContinueExecution(500);
			}
		}

		private void button8_Click(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
				dbg.WaitForDebugEvent(50);
		}

		#region Breakpoint editing
		private void list_AvailableSources_DoubleClick(object sender, EventArgs e)
		{
			if (list_AvailableSources.SelectedItems.Count != 0)
			{
				var f = (sstSrcModule.SourceFileInformation)list_AvailableSources.SelectedItems[0].Tag;

				if (!File.Exists(f.SourceFileName))
				{
					MessageBox.Show(f.SourceFileName + " cannot be found!");
					return;
				}

				MarkerStrategy.RemoveAll();
				editor.Load(lastOpenedFile = f.SourceFileName);
				editor.IsReadOnly = true;

				// Highlight all lines where breakpoints can be set
				var lines = new List<int>();

				foreach(var seg in f.Segments)
					foreach (var line in seg.Lines)
					{
						var marker = new DebugInfoAvailableMarker(MarkerStrategy, editor.Document, line, line);
						MarkerStrategy.Add(marker);
						marker.Redraw();
					}
			}
		}

		#endregion
	}
}
