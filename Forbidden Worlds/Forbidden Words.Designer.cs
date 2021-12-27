namespace Forbidden_Worlds
{
    partial class ForbiddenWords
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
            this.StartSearch = new System.Windows.Forms.Button();
            this.PauseSearch = new System.Windows.Forms.Button();
            this.StopSearch = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateWords = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartSearch
            // 
            this.StartSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartSearch.BackColor = System.Drawing.Color.YellowGreen;
            this.StartSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.StartSearch.Location = new System.Drawing.Point(23, 28);
            this.StartSearch.MaximumSize = new System.Drawing.Size(115, 54);
            this.StartSearch.MinimumSize = new System.Drawing.Size(90, 34);
            this.StartSearch.Name = "StartSearch";
            this.StartSearch.Size = new System.Drawing.Size(90, 54);
            this.StartSearch.TabIndex = 0;
            this.StartSearch.Text = "Start Search";
            this.StartSearch.UseVisualStyleBackColor = false;
            this.StartSearch.Click += new System.EventHandler(this.StartSearch_Click);
            // 
            // PauseSearch
            // 
            this.PauseSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PauseSearch.BackColor = System.Drawing.Color.PeachPuff;
            this.PauseSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PauseSearch.Location = new System.Drawing.Point(23, 103);
            this.PauseSearch.MaximumSize = new System.Drawing.Size(115, 54);
            this.PauseSearch.MinimumSize = new System.Drawing.Size(90, 34);
            this.PauseSearch.Name = "PauseSearch";
            this.PauseSearch.Size = new System.Drawing.Size(90, 54);
            this.PauseSearch.TabIndex = 1;
            this.PauseSearch.Text = "Pause Search";
            this.PauseSearch.UseVisualStyleBackColor = false;
            this.PauseSearch.Click += new System.EventHandler(this.PauseSearch_Click);
            // 
            // StopSearch
            // 
            this.StopSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StopSearch.BackColor = System.Drawing.Color.Crimson;
            this.StopSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.StopSearch.Location = new System.Drawing.Point(23, 180);
            this.StopSearch.MaximumSize = new System.Drawing.Size(115, 54);
            this.StopSearch.MinimumSize = new System.Drawing.Size(90, 34);
            this.StopSearch.Name = "StopSearch";
            this.StopSearch.Size = new System.Drawing.Size(90, 54);
            this.StopSearch.TabIndex = 2;
            this.StopSearch.Text = "Stop Search";
            this.StopSearch.UseVisualStyleBackColor = false;
            this.StopSearch.Click += new System.EventHandler(this.StopSearch_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(156, 159);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1001, 404);
            this.listBox1.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(156, 121);
            this.progressBar1.Maximum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1001, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MingLiU_HKSCS-ExtB", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Salmon;
            this.label1.Location = new System.Drawing.Point(425, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(395, 47);
            this.label1.TabIndex = 5;
            this.label1.Text = "Forbidden Words";
            // 
            // CreateWords
            // 
            this.CreateWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateWords.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CreateWords.Location = new System.Drawing.Point(23, 254);
            this.CreateWords.MaximumSize = new System.Drawing.Size(115, 54);
            this.CreateWords.MinimumSize = new System.Drawing.Size(90, 34);
            this.CreateWords.Name = "CreateWords";
            this.CreateWords.Size = new System.Drawing.Size(90, 54);
            this.CreateWords.TabIndex = 6;
            this.CreateWords.Text = "Create Words";
            this.CreateWords.UseVisualStyleBackColor = true;
            this.CreateWords.Click += new System.EventHandler(this.CreateWords_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StartSearch);
            this.groupBox1.Controls.Add(this.CreateWords);
            this.groupBox1.Controls.Add(this.PauseSearch);
            this.groupBox1.Controls.Add(this.StopSearch);
            this.groupBox1.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox1.Location = new System.Drawing.Point(12, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 330);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Panel";
            // 
            // ForbiddenWords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1226, 638);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listBox1);
            this.MinimumSize = new System.Drawing.Size(1244, 685);
            this.Name = "ForbiddenWords";
            this.Text = "Forbidden Words";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ForbiddenWords_FormClosing);
            this.Load += new System.EventHandler(this.ForbiddenWords_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartSearch;
        private System.Windows.Forms.Button PauseSearch;
        private System.Windows.Forms.Button StopSearch;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateWords;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

