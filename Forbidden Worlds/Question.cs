﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forbidden_Worlds
{
    public partial class Question : Form
    {
        public Question()
        {
            InitializeComponent();
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void No_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
