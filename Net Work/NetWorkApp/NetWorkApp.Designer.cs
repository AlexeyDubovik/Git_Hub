namespace NetWorkApp
{
    partial class NetWorkApp
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ShutDown = new System.Windows.Forms.Button();
            this.Start = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearText = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Port_Box = new System.Windows.Forms.TextBox();
            this.IP_Box = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(88, 76);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(493, 344);
            this.listBox1.TabIndex = 0;
            // 
            // ShutDown
            // 
            this.ShutDown.BackColor = System.Drawing.Color.IndianRed;
            this.ShutDown.Enabled = false;
            this.ShutDown.Location = new System.Drawing.Point(12, 140);
            this.ShutDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ShutDown.Name = "ShutDown";
            this.ShutDown.Size = new System.Drawing.Size(63, 54);
            this.ShutDown.TabIndex = 1;
            this.ShutDown.Text = "Shut Down";
            this.ShutDown.UseVisualStyleBackColor = false;
            this.ShutDown.Click += new System.EventHandler(this.ShutDown_Click);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.GreenYellow;
            this.Start.Location = new System.Drawing.Point(12, 76);
            this.Start.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(63, 54);
            this.Start.TabIndex = 2;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = false;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(88, 427);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(493, 27);
            this.textBox1.TabIndex = 3;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Console";
            // 
            // ClearText
            // 
            this.ClearText.Location = new System.Drawing.Point(12, 204);
            this.ClearText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ClearText.Name = "ClearText";
            this.ClearText.Size = new System.Drawing.Size(63, 54);
            this.ClearText.TabIndex = 5;
            this.ClearText.Text = "Clear Text";
            this.ClearText.UseVisualStyleBackColor = true;
            this.ClearText.Click += new System.EventHandler(this.ClearText_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(300, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 22);
            this.label2.TabIndex = 6;
            // 
            // Port_Box
            // 
            this.Port_Box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Port_Box.Location = new System.Drawing.Point(88, 461);
            this.Port_Box.Name = "Port_Box";
            this.Port_Box.Size = new System.Drawing.Size(125, 27);
            this.Port_Box.TabIndex = 7;
            this.Port_Box.Text = "3333";
            this.Port_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IP_Box
            // 
            this.IP_Box.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.IP_Box.Location = new System.Drawing.Point(456, 461);
            this.IP_Box.Name = "IP_Box";
            this.IP_Box.Size = new System.Drawing.Size(125, 27);
            this.IP_Box.TabIndex = 8;
            this.IP_Box.Text = "127.0.0.1";
            this.IP_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 464);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(426, 464);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP:";
            // 
            // NetWorkApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(662, 510);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IP_Box);
            this.Controls.Add(this.Port_Box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ClearText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.ShutDown);
            this.Controls.Add(this.listBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(680, 557);
            this.Name = "NetWorkApp";
            this.Text = "NetWorkApp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Server_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button ShutDown;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearText;
        private System.Windows.Forms.Label label2;
        private TextBox Port_Box;
        private TextBox IP_Box;
        private Label label3;
        private Label label4;
    }
}