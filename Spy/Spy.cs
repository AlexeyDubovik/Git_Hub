using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Spy
{
    public partial class Spy : Form
    {
        private static Option option;
        private static Logs log;
        private Processes processes;
        private Hook hook;
        private ThreadControl ThreadTracking;
        private static String ManageInputWords;
        private static String ManageProcesses;
        public Spy()
        {
            ManageInputWords = String.Empty;
            ManageProcesses = String.Empty;
            option = Option.getInstance(Application.StartupPath);
            processes = Processes.Instance;
            hook = Hook.Instance;
            log = Logs.getInstance(option.LogFilePath);
            ThreadTracking = null;
            InitializeComponent();
            LogFileFolderBox(option.LogFilePath);
            checkedListBox1_MouseClick(null, null);
        }
        private void LogFileFolderBox(String path)
        {
            comboBox1.Items.Add(path);
            comboBox1.Text = path;
            comboBox1.Items.Add("Select Path");
        }
        private void _Tracking(object obj)
        {
            if (obj == null)
                return;
            try
            {
                if (checkedListBox2.GetItemCheckState(0) == CheckState.Checked)
                {
                    listBox2.BackColor = Color.Green;
                    Invoke(new Action(() => ForbiddenListBox.Items.Clear()));
                    Invoke(new Action(() => ForbiddenListBox.Items.Add("Start Get Statistics: " + DateTime.Now.ToString())));
                    log.StatisticsProcesseRun.Add("Start: " + DateTime.Now.ToString() + "\n");
                    log.StatisticsKeyDown.Add("Start: " + DateTime.Now.ToString() + "\n");
                    Task.Run(() => processes.StatisticsProcesses(log, ThreadTracking.cts.Token), ThreadTracking.cts.Token);
                    Task.Run(() => hook.StatisticsKeydown(log, ThreadTracking.cts.Token), ThreadTracking.cts.Token);
                }
                if (checkedListBox2.GetItemCheckState(1) == CheckState.Checked)
                {
                    listBox2.BackColor = Color.Green;
                    Invoke(new Action(() => ForbiddenListBox.Items.Clear()));
                    Invoke(new Action(() => ForbiddenListBox.Items.Add("Start Manage: " + DateTime.Now.ToString())));
                    log.ManageInputWords.Add("Start: " + DateTime.Now.ToString() + "\n");
                    log.ManageProcesses.Add("Start: " + DateTime.Now.ToString() + "\n");
                    Task.Run(() => processes.ManageProcesses(log, option.Processes, ThreadTracking.cts.Token), ThreadTracking.cts.Token);
                    Task.Run(() => hook.ManageKeydown(log, option.Words, ThreadTracking.cts.Token), ThreadTracking.cts.Token);
                }
                if (ThreadTracking.cts.IsCancellationRequested)
                {
                    ThreadTracking.cts.Token.ThrowIfCancellationRequested();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void StartTracking_Click(object sender, EventArgs e)
        {
            if (ThreadTracking != null ||
                checkedListBox2.GetItemCheckState(0) == CheckState.Unchecked &&
                checkedListBox2.GetItemCheckState(1) == CheckState.Unchecked)
                return;
            hook.StartHook();
            ThreadTracking = new ThreadControl();
            ThreadTracking.cts = new CancellationTokenSource();
            ThreadTracking.thread = new Thread(_Tracking);
            ThreadTracking.thread.Start(true);
        }
        private void Stop_Click(object sender, EventArgs e)
        {
            if (ThreadTracking == null || ThreadTracking.cts == null)
                return;
            if (ForbiddenListBox != null)
            {
                listBox2.BackColor = Color.Red;
                ForbiddenListBox.Items.Clear();
                ForbiddenListBox.Items.Add("Stopped Tracking");
            }
            ThreadTracking.cts.Cancel();
            hook.StopHook();
            ThreadTracking = null;
        }
        private void Spy_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop_Click(sender, e);
            option.Save();
            log.Save(option.LogFilePath);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == comboBox1.Items[comboBox1.Items.Count - 1] && comboBox1.Items.Count > 1)
            {
                comboBox1.Text = "";
                string tmp = option.OpenFolder(this);
                comboBox1.Items[0] = tmp;
                comboBox1.Text = tmp + "Logs.xml";
                FileInfo fi = new FileInfo(option.LogFilePath);
                Task.Run(() => fi.MoveTo(comboBox1.Text));
                option.LogFilePath = comboBox1.Text;
                comboBox1.SelectedIndex = 0;
            }
        }
        private void LoopToForbiddenList(object obj)
        {
            foreach (var item in obj as List<String>)
            {
                Invoke(new Action(() => ForbiddenListBox.Items.Add("")));
                for (int i = 0; i < item.Count(); i++)
                {
                    Thread.Sleep(5);
                    Invoke(new Action(() => ForbiddenListBox.Items[ForbiddenListBox.Items.Count - 1] += item.ToArray()[i].ToString()));
                }
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
                                ForbiddenListBox.Items.Clear();
                                List<string> list = i == 0 ? option.Processes : option.Words;
                                list.Sort();
                                if (list.Count > 0)
                                    new Thread(LoopToForbiddenList).Start(list);
                                else
                                    ForbiddenListBox.Items.Add(i == 0 ? "Empty processes" : "Empty words");
                                break;
                            case CheckState.Unchecked:
                                checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                                break;
                        }
            foreach (int i in checkedListBox1.CheckedIndices)
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            checkedListBox1.SelectionMode = SelectionMode.None;
            checkedListBox1.SelectionMode = SelectionMode.One;
        }
        private void checkedListBox2_MouseClick(object sender, MouseEventArgs e)
        {
            for (int x = 0; x < 2; x++)
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                    if (checkedListBox2.GetItemRectangle(i).Contains(checkedListBox2.PointToClient(MousePosition)))
                        switch (checkedListBox2.GetItemCheckState(i))
                        {
                            case CheckState.Checked:
                                checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);
                                break;
                            case CheckState.Unchecked:
                                checkedListBox2.SetItemCheckState(i, CheckState.Checked);
                                break;
                        }
            foreach (int i in checkedListBox2.CheckedIndices)
                checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);
            checkedListBox2.SelectionMode = SelectionMode.None;
            checkedListBox2.SelectionMode = SelectionMode.One;
        }
        private void ConsoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ConsoleTextBox.Text.Length > 0)
            {
                e.SuppressKeyPress = true;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    switch (checkedListBox1.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            List<string> list = i == 0 ? option.Processes : option.Words;
                            foreach (var str in list)
                                if (i == 0 ? (str == ConsoleTextBox.Text) : Regex.IsMatch(str, ConsoleTextBox.Text, RegexOptions.IgnoreCase))
                                {
                                    MessageBox.Show(i == 0 ? "This process already here" : "This word already here");
                                    return;
                                }
                            ForbiddenListBox.Items.Clear();
                            (i == 0 ? option.Processes : option.Words).Add(ConsoleTextBox.Text);
                            list = i == 0 ? option.Processes : option.Words;
                            list.Sort();
                            if (list.Count > 0)
                                new Thread(LoopToForbiddenList).Start(list);
                            ConsoleTextBox.Text = "";
                            break;
                        case CheckState.Unchecked:
                            break;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new LogView(log).ShowDialog();
        }
    }
}
