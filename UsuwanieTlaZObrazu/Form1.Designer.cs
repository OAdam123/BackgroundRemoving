namespace UsuwanieTlaZObrazu
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxInput = new PictureBox();
            pictureBoxOutput = new PictureBox();
            buttonLoad = new Button();
            buttonProcess = new Button();
            trackTolerance = new TrackBar();
            labelColorInfo = new Label();
            panelSelectedColor = new Panel();
            groupBox = new GroupBox();
            checkAsm = new CheckBox();
            checkCpp = new CheckBox();
            statusStrip1 = new StatusStrip();
            labelStatus = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            labelTime = new ToolStripStatusLabel();
            trackThreads = new TrackBar();
            labelThreads = new Label();
            labelTolerance = new Label();
            label3 = new Label();
            labelThreadsValue = new Label();
            labelToleranceValue = new Label();
            buttonBenchmark = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackTolerance).BeginInit();
            groupBox.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackThreads).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxInput
            // 
            pictureBoxInput.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxInput.Location = new Point(12, 12);
            pictureBoxInput.Name = "pictureBoxInput";
            pictureBoxInput.Size = new Size(328, 217);
            pictureBoxInput.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxInput.TabIndex = 0;
            pictureBoxInput.TabStop = false;
            pictureBoxInput.Click += pictureBoxInput_Click;
            pictureBoxInput.MouseClick += pictureBoxInput_MouseClick;
            // 
            // pictureBoxOutput
            // 
            pictureBoxOutput.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxOutput.Location = new Point(12, 246);
            pictureBoxOutput.Name = "pictureBoxOutput";
            pictureBoxOutput.Size = new Size(328, 217);
            pictureBoxOutput.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOutput.TabIndex = 1;
            pictureBoxOutput.TabStop = false;
            pictureBoxOutput.Click += pictureBoxOutput_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(437, 34);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(139, 49);
            buttonLoad.TabIndex = 2;
            buttonLoad.Text = "Wczytaj obraz";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // buttonProcess
            // 
            buttonProcess.Location = new Point(636, 34);
            buttonProcess.Name = "buttonProcess";
            buttonProcess.Size = new Size(139, 49);
            buttonProcess.TabIndex = 3;
            buttonProcess.Text = "Usuń tło";
            buttonProcess.UseVisualStyleBackColor = true;
            buttonProcess.Click += buttonProcess_Click;
            // 
            // trackTolerance
            // 
            trackTolerance.Location = new Point(384, 142);
            trackTolerance.Maximum = 100;
            trackTolerance.Name = "trackTolerance";
            trackTolerance.Size = new Size(192, 45);
            trackTolerance.TabIndex = 4;
            trackTolerance.Value = 10;
            trackTolerance.Scroll += trackTolerance_Scroll;
            // 
            // labelColorInfo
            // 
            labelColorInfo.AutoSize = true;
            labelColorInfo.Location = new Point(496, 234);
            labelColorInfo.Name = "labelColorInfo";
            labelColorInfo.Size = new Size(199, 15);
            labelColorInfo.TabIndex = 5;
            labelColorInfo.Text = "Kliknij na obraz, aby wybrać kolor tła";
            labelColorInfo.Click += labelColorInfo_Click;
            // 
            // panelSelectedColor
            // 
            panelSelectedColor.Location = new Point(566, 269);
            panelSelectedColor.Name = "panelSelectedColor";
            panelSelectedColor.Size = new Size(50, 50);
            panelSelectedColor.TabIndex = 6;
            panelSelectedColor.Paint += panelSelectedColor_Paint;
            // 
            // groupBox
            // 
            groupBox.Controls.Add(checkAsm);
            groupBox.Controls.Add(checkCpp);
            groupBox.Location = new Point(496, 342);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(200, 100);
            groupBox.TabIndex = 7;
            groupBox.TabStop = false;
            groupBox.Text = "Wybór biblioteki";
            // 
            // checkAsm
            // 
            checkAsm.AutoSize = true;
            checkAsm.Location = new Point(21, 62);
            checkAsm.Name = "checkAsm";
            checkAsm.Size = new Size(71, 19);
            checkAsm.TabIndex = 1;
            checkAsm.Text = "ASM x64";
            checkAsm.UseVisualStyleBackColor = true;
            checkAsm.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkCpp
            // 
            checkCpp.AutoSize = true;
            checkCpp.Location = new Point(21, 37);
            checkCpp.Name = "checkCpp";
            checkCpp.Size = new Size(50, 19);
            checkCpp.TabIndex = 0;
            checkCpp.Text = "C++";
            checkCpp.UseVisualStyleBackColor = true;
            checkCpp.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { labelStatus, toolStripStatusLabel1, progressBar, labelTime });
            statusStrip1.Location = new Point(0, 466);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(829, 22);
            statusStrip1.TabIndex = 8;
            statusStrip1.Text = "statusStrip1";
            // 
            // labelStatus
            // 
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(48, 17);
            labelStatus.Text = "Gotowy";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(100, 16);
            progressBar.Visible = false;
            // 
            // labelTime
            // 
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(62, 17);
            labelTime.Text = "Czas: 0 ms";
            // 
            // trackThreads
            // 
            trackThreads.Location = new Point(615, 142);
            trackThreads.Maximum = 64;
            trackThreads.Minimum = 1;
            trackThreads.Name = "trackThreads";
            trackThreads.Size = new Size(192, 45);
            trackThreads.TabIndex = 9;
            trackThreads.Value = 10;
            trackThreads.Scroll += trackThreads_Scroll;
            // 
            // labelThreads
            // 
            labelThreads.AutoSize = true;
            labelThreads.Location = new Point(679, 124);
            labelThreads.Name = "labelThreads";
            labelThreads.Size = new Size(71, 15);
            labelThreads.TabIndex = 10;
            labelThreads.Text = "Wątki (1-64)";
            labelThreads.Click += labelThreads_Click;
            // 
            // labelTolerance
            // 
            labelTolerance.AutoSize = true;
            labelTolerance.Location = new Point(437, 124);
            labelTolerance.Name = "labelTolerance";
            labelTolerance.Size = new Size(101, 15);
            labelTolerance.TabIndex = 11;
            labelTolerance.Text = "Tolerancja (1-100)";
            labelTolerance.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(453, 214);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 12;
            // 
            // labelThreadsValue
            // 
            labelThreadsValue.AutoSize = true;
            labelThreadsValue.Location = new Point(700, 172);
            labelThreadsValue.Name = "labelThreadsValue";
            labelThreadsValue.Size = new Size(19, 15);
            labelThreadsValue.TabIndex = 13;
            labelThreadsValue.Text = "10";
            // 
            // labelToleranceValue
            // 
            labelToleranceValue.AutoSize = true;
            labelToleranceValue.Location = new Point(471, 172);
            labelToleranceValue.Name = "labelToleranceValue";
            labelToleranceValue.Size = new Size(19, 15);
            labelToleranceValue.TabIndex = 14;
            labelToleranceValue.Text = "10";
            // 
            // buttonBenchmark
            // 
            buttonBenchmark.Location = new Point(722, 379);
            buttonBenchmark.Name = "buttonBenchmark";
            buttonBenchmark.Size = new Size(75, 23);
            buttonBenchmark.TabIndex = 15;
            buttonBenchmark.Text = "Testy";
            buttonBenchmark.UseVisualStyleBackColor = true;
            buttonBenchmark.Click += buttonBenchmark_Click;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(829, 488);
            Controls.Add(buttonBenchmark);
            Controls.Add(labelToleranceValue);
            Controls.Add(labelThreadsValue);
            Controls.Add(label3);
            Controls.Add(labelTolerance);
            Controls.Add(labelThreads);
            Controls.Add(trackThreads);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox);
            Controls.Add(panelSelectedColor);
            Controls.Add(labelColorInfo);
            Controls.Add(trackTolerance);
            Controls.Add(buttonProcess);
            Controls.Add(buttonLoad);
            Controls.Add(pictureBoxOutput);
            Controls.Add(pictureBoxInput);
            Name = "Form";
            Text = "Usuwanie tła z obrazu";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutput).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackTolerance).EndInit();
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackThreads).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxInput;
        private PictureBox pictureBoxOutput;
        private Button buttonLoad;
        private Button buttonProcess;
        private TrackBar trackTolerance;
        private Label labelColorInfo;
        private Panel panelSelectedColor;
        private GroupBox groupBox;
        private CheckBox checkCpp;
        private CheckBox checkAsm;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel labelStatus;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar progressBar;
        private ToolStripStatusLabel labelTime;
        private TrackBar trackThreads;
        private Label labelThreads;
        private Label labelTolerance;
        private Label label3;
        private Label labelThreadsValue;
        private Label labelToleranceValue;
        private Button buttonBenchmark;
    }
}
