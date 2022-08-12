using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spy
{
    public partial class LogView : Form
    {
        private static Logs log;
        public LogView(Logs _log)
        {
            log = _log;
            InitializeComponent();
        }
        private void ShowLog(object str)
        {
            if (str == null)
                return;
            Invoke(new Action(() => listBox1.Items.Clear()));
            Invoke(new Action(() => listBox1.Items.Add("")));
            foreach (var item in str as List<String>)
            {
                for (int i = 0; i < item.Count(); i++)
                {
                    Thread.Sleep(5);
                    if (item.ToArray()[i] == '\n')
                        Invoke(new Action(() => listBox1.Items.Add(item.ToArray()[i])));
                    else
                        Invoke(new Action(() => listBox1.Items[listBox1.Items.Count - 1] += item.ToArray()[i].ToString()));
                }
            }
        }
        private void SelectedLog()
        {
            try
            {
                if (checkedListBox1.GetItemCheckState(0) == CheckState.Checked)
                    new Thread(ShowLog).Start(log.StatisticsKeyDown);
                if (checkedListBox1.GetItemCheckState(1) == CheckState.Checked)
                    new Thread(ShowLog).Start(log.StatisticsProcesseRun);
                if (checkedListBox1.GetItemCheckState(2) == CheckState.Checked)
                    new Thread(ShowLog).Start(log.ManageInputWords);
                if (checkedListBox1.GetItemCheckState(3) == CheckState.Checked)
                    new Thread(ShowLog).Start(log.ManageProcesses);
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int x = 0; x < 2; x++)
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    if (checkedListBox1.GetItemRectangle(i).Contains(checkedListBox1.PointToClient(MousePosition)))
                        switch (checkedListBox1.GetItemCheckState(i))
                        {
                            case CheckState.Checked:
                                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                                break;
                            case CheckState.Unchecked:
                                checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                                break;
                        }
            foreach (int i in checkedListBox1.CheckedIndices)
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            checkedListBox1.SelectionMode = SelectionMode.None;
            checkedListBox1.SelectionMode = SelectionMode.One;
            SelectedLog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
