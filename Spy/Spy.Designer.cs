namespace Spy
{
    partial class Spy
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartTracking = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ReportLabel = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.ForbiddenListBox = new System.Windows.Forms.ListBox();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartTracking
            // 
            this.StartTracking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartTracking.BackColor = System.Drawing.Color.DarkKhaki;
            this.StartTracking.Location = new System.Drawing.Point(509, 56);
            this.StartTracking.Name = "StartTracking";
            this.StartTracking.Size = new System.Drawing.Size(86, 44);
            this.StartTracking.TabIndex = 3;
            this.StartTracking.Text = "Start Tracking";
            this.StartTracking.UseVisualStyleBackColor = false;
            this.StartTracking.Click += new System.EventHandler(this.StartTracking_Click);
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Stop.BackColor = System.Drawing.Color.DarkKhaki;
            this.Stop.Location = new System.Drawing.Point(509, 106);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(86, 44);
            this.Stop.TabIndex = 4;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = false;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(132, 353);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(362, 24);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ReportLabel
            // 
            this.ReportLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReportLabel.AutoSize = true;
            this.ReportLabel.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.ReportLabel.Location = new System.Drawing.Point(29, 356);
            this.ReportLabel.Name = "ReportLabel";
            this.ReportLabel.Size = new System.Drawing.Size(88, 16);
            this.ReportLabel.TabIndex = 6;
            this.ReportLabel.Text = "Report folder:";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.DimGray;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ForeColor = System.Drawing.Color.DarkSalmon;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkedListBox1.IntegralHeight = false;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Processes",
            "Words"});
            this.checkedListBox1.Location = new System.Drawing.Point(12, 50);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox1.Size = new System.Drawing.Size(105, 41);
            this.checkedListBox1.TabIndex = 7;
            this.checkedListBox1.Tag = "";
            this.checkedListBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkedListBox1_MouseClick);
            // 
            // ForbiddenListBox
            // 
            this.ForbiddenListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForbiddenListBox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ForbiddenListBox.ForeColor = System.Drawing.Color.Lime;
            this.ForbiddenListBox.FormattingEnabled = true;
            this.ForbiddenListBox.ItemHeight = 16;
            this.ForbiddenListBox.Location = new System.Drawing.Point(132, 12);
            this.ForbiddenListBox.Name = "ForbiddenListBox";
            this.ForbiddenListBox.Size = new System.Drawing.Size(362, 292);
            this.ForbiddenListBox.TabIndex = 8;
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleTextBox.Location = new System.Drawing.Point(132, 320);
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.Size = new System.Drawing.Size(362, 22);
            this.ConsoleTextBox.TabIndex = 9;
            this.ConsoleTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleTextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(57, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Console:";
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.BackColor = System.Drawing.Color.DimGray;
            this.checkedListBox2.CheckOnClick = true;
            this.checkedListBox2.ForeColor = System.Drawing.Color.DarkSalmon;
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkedListBox2.IntegralHeight = false;
            this.checkedListBox2.Items.AddRange(new object[] {
            "Statistics",
            "Modding"});
            this.checkedListBox2.Location = new System.Drawing.Point(11, 122);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(106, 42);
            this.checkedListBox2.TabIndex = 11;
            this.checkedListBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkedListBox2_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Show Forbidden";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label3.Location = new System.Drawing.Point(12, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Tracking Mode";
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.BackColor = System.Drawing.Color.Red;
            this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox2.Enabled = false;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(509, 31);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(18, 16);
            this.listBox2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Location = new System.Drawing.Point(532, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "Сondition";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DarkKhaki;
            this.button1.Location = new System.Drawing.Point(509, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 44);
            this.button1.TabIndex = 16;
            this.button1.Text = "Log Looking";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Spy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(617, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConsoleTextBox);
            this.Controls.Add(this.ForbiddenListBox);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.ReportLabel);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.StartTracking);
            this.MinimumSize = new System.Drawing.Size(635, 439);
            this.Name = "Spy";
            this.Text = "Spy";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Spy_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartTracking;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label ReportLabel;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ListBox ForbiddenListBox;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

