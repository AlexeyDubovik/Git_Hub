namespace Forbidden_Worlds
{
    partial class CreateFW
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WordsListBox = new System.Windows.Forms.ListBox();
            this.WordBox = new System.Windows.Forms.TextBox();
            this.AddWord = new System.Windows.Forms.Button();
            this.DeleteWord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WordsListBox
            // 
            this.WordsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WordsListBox.FormattingEnabled = true;
            this.WordsListBox.ItemHeight = 16;
            this.WordsListBox.Location = new System.Drawing.Point(12, 12);
            this.WordsListBox.Name = "WordsListBox";
            this.WordsListBox.Size = new System.Drawing.Size(422, 212);
            this.WordsListBox.TabIndex = 0;
            this.WordsListBox.SelectedIndexChanged += new System.EventHandler(this.WordsListBox_SelectedIndexChanged);
            // 
            // WordBox
            // 
            this.WordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WordBox.Location = new System.Drawing.Point(12, 230);
            this.WordBox.Name = "WordBox";
            this.WordBox.Size = new System.Drawing.Size(422, 22);
            this.WordBox.TabIndex = 1;
            this.WordBox.Text = "Enter World";
            this.WordBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WordBox_MouseClick);
            // 
            // AddWord
            // 
            this.AddWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddWord.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.AddWord.Location = new System.Drawing.Point(12, 258);
            this.AddWord.Name = "AddWord";
            this.AddWord.Size = new System.Drawing.Size(106, 54);
            this.AddWord.TabIndex = 2;
            this.AddWord.Text = "Add Word";
            this.AddWord.UseVisualStyleBackColor = false;
            this.AddWord.Click += new System.EventHandler(this.AddWord_Click);
            // 
            // DeleteWord
            // 
            this.DeleteWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteWord.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DeleteWord.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.DeleteWord.Location = new System.Drawing.Point(328, 258);
            this.DeleteWord.Name = "DeleteWord";
            this.DeleteWord.Size = new System.Drawing.Size(106, 54);
            this.DeleteWord.TabIndex = 3;
            this.DeleteWord.Text = "Delete Word";
            this.DeleteWord.UseVisualStyleBackColor = false;
            this.DeleteWord.Click += new System.EventHandler(this.DeleteWord_Click);
            // 
            // CreateFW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(451, 327);
            this.Controls.Add(this.DeleteWord);
            this.Controls.Add(this.AddWord);
            this.Controls.Add(this.WordBox);
            this.Controls.Add(this.WordsListBox);
            this.MinimumSize = new System.Drawing.Size(469, 374);
            this.Name = "CreateFW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateFW";
            this.Load += new System.EventHandler(this.CreateFW_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ListBox WordsListBox;
        private System.Windows.Forms.TextBox WordBox;
        private System.Windows.Forms.Button AddWord;
        private System.Windows.Forms.Button DeleteWord;
    }
}