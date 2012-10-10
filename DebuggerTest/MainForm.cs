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
using DDebugger.Breakpoints;

namespace DebuggerTest
{
	public partial class MainForm : Form
	{
		TextEditor editor = new TextEditor();
		public TextMarkerService MarkerStrategy;
		CurrentFrameMarker currentFrameMarker;
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
			//dbg.Breakpoints.CreateBreakpoint(new IntPtr(0x004020c8u));
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
				dbg.MainProcess.Terminate(0);
		}

		public void Log(string msg)
		{
			eventLogBox.AppendText(msg + "\r\n");
		}

		void HighlightCurrentInstruction(DebugThread th)
		{
			if (currentFrameMarker != null)
				currentFrameMarker.Delete();

			ushort line = 0;
			string file = null;

			if (th.OwnerProcess.MainModule.ContainsSymbolData &&
				th.OwnerProcess.MainModule.ModuleMetaInfo.TryDetermineCodeLocation((uint)th.CurrentInstruction.ToInt32(), out file, out line))
			{
				LoadSourceFileIntoEditor(file);

				currentFrameMarker = new CurrentFrameMarker(MarkerStrategy, editor.Document, line);
				MarkerStrategy.Add(currentFrameMarker);
				currentFrameMarker.Redraw();
			}
		}

		public static sstSrcModule.SourceFileInformation? GetFileInfo(DebugProcessModule module, string file)
		{
			foreach (var section in module.ModuleMetaInfo.CodeViewSection.Data.SubsectionDirectory.Sections)
				if (section is sstSrcModule)
				{
					var srcModule = (sstSrcModule)section;

					foreach (var f in srcModule.FileInfo)
						if (f.SourceFileName == file)
							return f;
				}

			return null;
		}

		class EventLogger : DebugEventListener
		{
			readonly MainForm form;
			public EventLogger(MainForm f,Debuggee dbg) : base(dbg) {
				form = f;
			}

			public override void OnCreateProcess(DebugProcess newProcess)
			{
				form.Log(
					"Program " + newProcess.MainModule.ImageFile+" was launched\r\n"+
					"\tPID #"+newProcess.Id);

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
				form.HighlightCurrentInstruction(thread);
			}

			public override void OnBreakpoint(DebugThread thread, DDebugger.Breakpoints.Breakpoint breakpoint)
			{
				form.HighlightCurrentInstruction(thread);
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
				form.HighlightCurrentInstruction(thread);
			}

			public override void OnModuleLoaded(DebugProcess mainProcess, DebugProcessModule module)
			{
				form.Log(module.ImageFile+" loaded (0x"+string.Format("{0,8:X}",module.ImageBase)+")");
			}

			public override void OnModuleUnloaded(DebugProcess mainProcess, DebugProcessModule module)
			{
				if (module != null)
					form.Log(module.ImageFile + " unloaded (0x" + string.Format("{0,8:X}", module.ImageBase) + ")");
				else
					form.Log("Some module was unloaded");
			}

			public override void OnProcessExit(DebugProcess process, uint exitCode)
			{
				form.eventLogBox.AppendText("Process exited with code 0x" + string.Format("{0,X}", exitCode));
			}

			public override void OnStepComplete(DebugThread thread)
			{
				form.HighlightCurrentInstruction(thread);
			}

			public override void OnThreadExit(DebugThread thread, uint exitCode)
			{
				base.OnThreadExit(thread, exitCode);
			}
		}

		private void continueExecution(object sender, EventArgs e)
		{
			if (dbg != null && dbg.IsAlive)
			{
				dbg.ContinueExecution(500);
			}
		}

		#region Breakpoint editing
		private void list_AvailableSources_DoubleClick(object sender, EventArgs e)
		{
			if (list_AvailableSources.SelectedItems.Count != 0)
			{
				var f = (sstSrcModule.SourceFileInformation)list_AvailableSources.SelectedItems[0].Tag;

				LoadSourceFileIntoEditor(f.SourceFileName,f);
			}
		}

		void LoadSourceFileIntoEditor(string file, sstSrcModule.SourceFileInformation? f = null)
		{
			// Assume all files not be modified since the last time of load. Better performance.
			if (file == lastOpenedFile)
				return;

			if (!File.Exists(file))
			{
				MessageBox.Show(file + " cannot be found!");
				return;
			}

			if (!f.HasValue)
				f = GetFileInfo(dbg.MainProcess.MainModule, file);

			MarkerStrategy.RemoveAll();
			currentFrameMarker = null;
			editor.Load(lastOpenedFile = file);
			editor.IsReadOnly = true;

			// Highlight all lines where breakpoints can be set
			var lines = new List<int>();

			if(f.HasValue)
				foreach (var seg in f.Value.Segments)
					for (int i = 0; i < seg.Lines.Length; i++)
					{
						var ln = seg.Lines[i];
						var marker = new DebugInfoAvailableMarker(MarkerStrategy, editor.Document, ln, ln);
						MarkerStrategy.Add(marker);
						marker.Redraw();
						marker.Tag = seg.Offsets[i];

						// And highlight previously set breakpoints
						var bp = dbg.Breakpoints.ByAddress(dbg.MainProcess.MainModule.ToVirtualAddress((int)seg.Offsets[i]));
						if (bp != null)
						{
							var bpM = new BreakpointMarker(MarkerStrategy, bp, editor.Document, ln, ln);
							MarkerStrategy.Add(bpM);
							bpM.Redraw();
						}
					}
		}

		private void toggleBreakpointClick(object sender, EventArgs e)
		{
			var markers = MarkerStrategy.GetMarkersAtOffset(editor.CaretOffset);
			var m = markers.FirstOrDefault();
			if (m == null)
			{
				MessageBox.Show("Breakpoints can only be set at green highlighted lines");
				return;
			}
			var modMetaInfo = dbg.MainProcess.MainModule.ModuleMetaInfo;
			var addr = new IntPtr(modMetaInfo.PEHeader.OptionalHeader32.ImageBase + modMetaInfo.PEHeader.OptionalHeader32.BaseOfCode + (uint)m.Tag);

			var bp = dbg.Breakpoints.ByAddress(addr);
			if (bp == null)
				bp = dbg.Breakpoints.CreateBreakpoint(addr);
			else
			{
				foreach (var _m in markers.ToArray())
					if(_m is BreakpointMarker)
						_m.Delete();
				dbg.Breakpoints.Remove(bp);
				return;
			}
			
			var line = editor.Document.GetLineByOffset(m.StartOffset).LineNumber;
			var newMrker = new BreakpointMarker(MarkerStrategy, bp, editor.Document, line, line);
			MarkerStrategy.Add(newMrker);
			newMrker.Redraw();
		}
		#endregion

		private void stepInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dbg.CodeStepping.StepIn(dbg.CurrentThread);
		}

		private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dbg.CodeStepping.StepOver(dbg.CurrentThread);
		}

		private void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dbg.CodeStepping.StepOut(dbg.CurrentThread);
		}
	}
}
