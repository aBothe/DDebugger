namespace DebuggerTest
{
	partial class MainForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.input_executable = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button2 = new System.Windows.Forms.Button();
			this.input_pid = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.eventLogBox = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.list_AvailableSources = new System.Windows.Forms.ListView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.steppingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.goToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.breakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// input_executable
			// 
			this.input_executable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.input_executable.Location = new System.Drawing.Point(9, 32);
			this.input_executable.Name = "input_executable";
			this.input_executable.Size = new System.Drawing.Size(609, 20);
			this.input_executable.TabIndex = 0;
			this.input_executable.Text = "..\\..\\testdebuggee\\a.exe";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Executable File";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(624, 30);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Execute";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.input_pid);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.input_executable);
			this.groupBox1.Location = new System.Drawing.Point(7, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(705, 100);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Launch/Attach";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(400, 69);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Attach";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// input_pid
			// 
			this.input_pid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.input_pid.Location = new System.Drawing.Point(9, 71);
			this.input_pid.Name = "input_pid";
			this.input_pid.Size = new System.Drawing.Size(385, 20);
			this.input_pid.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Process Name/ID";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.button4);
			this.groupBox2.Controls.Add(this.button3);
			this.groupBox2.Location = new System.Drawing.Point(6, 109);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(706, 82);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Terminate/Detach";
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.Location = new System.Drawing.Point(9, 48);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(691, 23);
			this.button4.TabIndex = 1;
			this.button4.Text = "Detach from debuggee";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(9, 19);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(691, 23);
			this.button3.TabIndex = 0;
			this.button3.Text = "Terminate debuggee";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.eventLogBox);
			this.groupBox3.Location = new System.Drawing.Point(1, 351);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(726, 147);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Event log";
			// 
			// eventLogBox
			// 
			this.eventLogBox.BackColor = System.Drawing.Color.White;
			this.eventLogBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventLogBox.Location = new System.Drawing.Point(3, 16);
			this.eventLogBox.Multiline = true;
			this.eventLogBox.Name = "eventLogBox";
			this.eventLogBox.ReadOnly = true;
			this.eventLogBox.Size = new System.Drawing.Size(720, 128);
			this.eventLogBox.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(1, 27);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(726, 318);
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(718, 292);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Process Start/Stop";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.elementHost1);
			this.tabPage2.Controls.Add(this.list_AvailableSources);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(718, 292);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sources";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// elementHost1
			// 
			this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.elementHost1.Location = new System.Drawing.Point(296, 0);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(422, 292);
			this.elementHost1.TabIndex = 2;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = null;
			// 
			// list_AvailableSources
			// 
			this.list_AvailableSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.list_AvailableSources.Location = new System.Drawing.Point(3, 0);
			this.list_AvailableSources.Name = "list_AvailableSources";
			this.list_AvailableSources.Size = new System.Drawing.Size(292, 292);
			this.list_AvailableSources.TabIndex = 0;
			this.list_AvailableSources.UseCompatibleStateImageBehavior = false;
			this.list_AvailableSources.View = System.Windows.Forms.View.List;
			this.list_AvailableSources.DoubleClick += new System.EventHandler(this.list_AvailableSources_DoubleClick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processToolStripMenuItem,
            this.breakpointsToolStripMenuItem,
            this.steppingToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(728, 24);
			this.menuStrip1.TabIndex = 7;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// processToolStripMenuItem
			// 
			this.processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripMenuItem});
			this.processToolStripMenuItem.Name = "processToolStripMenuItem";
			this.processToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.processToolStripMenuItem.Text = "Process";
			// 
			// executeToolStripMenuItem
			// 
			this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
			this.executeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.executeToolStripMenuItem.Text = "Execute";
			// 
			// breakpointsToolStripMenuItem
			// 
			this.breakpointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleToolStripMenuItem});
			this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
			this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
			this.breakpointsToolStripMenuItem.Text = "Breakpoints";
			// 
			// toggleToolStripMenuItem
			// 
			this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
			this.toggleToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.toggleToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.toggleToolStripMenuItem.Text = "Toggle";
			this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleBreakpointClick);
			// 
			// steppingToolStripMenuItem
			// 
			this.steppingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stepInToolStripMenuItem,
            this.stepOverToolStripMenuItem,
            this.stepOutToolStripMenuItem,
            this.toolStripSeparator1,
            this.goToolStripMenuItem,
            this.breakToolStripMenuItem});
			this.steppingToolStripMenuItem.Name = "steppingToolStripMenuItem";
			this.steppingToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
			this.steppingToolStripMenuItem.Text = "Stepping";
			// 
			// stepInToolStripMenuItem
			// 
			this.stepInToolStripMenuItem.Name = "stepInToolStripMenuItem";
			this.stepInToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.stepInToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.stepInToolStripMenuItem.Text = "Step In";
			this.stepInToolStripMenuItem.Click += new System.EventHandler(this.stepInToolStripMenuItem_Click);
			// 
			// stepOverToolStripMenuItem
			// 
			this.stepOverToolStripMenuItem.Name = "stepOverToolStripMenuItem";
			this.stepOverToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.stepOverToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.stepOverToolStripMenuItem.Text = "Step Over";
			this.stepOverToolStripMenuItem.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
			// 
			// stepOutToolStripMenuItem
			// 
			this.stepOutToolStripMenuItem.Name = "stepOutToolStripMenuItem";
			this.stepOutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.stepOutToolStripMenuItem.Text = "Step Out";
			this.stepOutToolStripMenuItem.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// goToolStripMenuItem
			// 
			this.goToolStripMenuItem.Name = "goToolStripMenuItem";
			this.goToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.goToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.goToolStripMenuItem.Text = "Go";
			this.goToolStripMenuItem.Click += new System.EventHandler(this.continueExecution);
			// 
			// breakToolStripMenuItem
			// 
			this.breakToolStripMenuItem.Name = "breakToolStripMenuItem";
			this.breakToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.breakToolStripMenuItem.Text = "Break";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 499);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.menuStrip1);
			this.Name = "MainForm";
			this.Text = "DDebug test app";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox input_executable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox input_pid;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox eventLogBox;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListView list_AvailableSources;
		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem steppingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepOverToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stepOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem goToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem breakToolStripMenuItem;
	}
}

