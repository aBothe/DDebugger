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
			this.label2 = new System.Windows.Forms.Label();
			this.input_pid = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.eventLogBox = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.list_AvailableSources = new System.Windows.Forms.ListView();
			this.button5 = new System.Windows.Forms.Button();
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.button7 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Process Name/ID";
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
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new System.Drawing.Point(1, 1);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(726, 344);
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(718, 318);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Process Start/Stop";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.button7);
			this.tabPage2.Controls.Add(this.elementHost1);
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.list_AvailableSources);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(718, 318);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sources";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// list_AvailableSources
			// 
			this.list_AvailableSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.list_AvailableSources.Location = new System.Drawing.Point(3, 0);
			this.list_AvailableSources.Name = "list_AvailableSources";
			this.list_AvailableSources.Size = new System.Drawing.Size(292, 283);
			this.list_AvailableSources.TabIndex = 0;
			this.list_AvailableSources.UseCompatibleStateImageBehavior = false;
			// 
			// button5
			// 
			this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button5.Location = new System.Drawing.Point(7, 289);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 1;
			this.button5.Text = "Load";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// elementHost1
			// 
			this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.elementHost1.Location = new System.Drawing.Point(296, 0);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(422, 318);
			this.elementHost1.TabIndex = 2;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = null;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.button8);
			this.tabPage3.Controls.Add(this.button6);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(718, 318);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Process controlling";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// button7
			// 
			this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button7.Location = new System.Drawing.Point(88, 289);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(202, 23);
			this.button7.TabIndex = 3;
			this.button7.Text = "Toggle breakpoint @ current line";
			this.button7.UseVisualStyleBackColor = true;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(165, 68);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(160, 23);
			this.button6.TabIndex = 6;
			this.button6.Text = "Continue execution";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(165, 97);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(160, 23);
			this.button8.TabIndex = 7;
			this.button8.Text = "Wait for debug event";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.button8_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 499);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox3);
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
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

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
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button8;
	}
}

