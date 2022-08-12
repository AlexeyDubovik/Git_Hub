using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
//using System.Text.RegularExpressions;

namespace Forbidden_Worlds
{
    public partial class ForbiddenWords : Form
    {
        private FW_Control FWC;
        private ThreadControl TC;
        private DriveInfo[] drives;
        private log LOG;
        private object resLocker;
        private int progress;
        public ForbiddenWords()
        {
            FWC = new FW_Control(Application.StartupPath);
            LOG = new log(FWC.DirectoryPath);
            TC = new ThreadControl();
            InitializeComponent();
        }
        private void ForbiddenWords_Load(object sender, EventArgs e)
        {
            Process[] processList = Process.GetProcessesByName(this.Text);
            if (processList.Length > 1)
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("Is Run!"); }
                this.Close();
            }
            resLocker = new object();
            drives = DriveInfo.GetDrives();
            listBox1.Items.Add($"You have {drives.Count()} drives...");
            foreach (DriveInfo drive in drives)
                listBox1.Items.Add(drive.Name);
            StopSearch.Enabled = false;
        }
        #region Func
        private void ExeptionListbox(Exception ex)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    int tmp = listBox1.Items.Count;
                    listBox1.Items.Add("");
                    for (int i = 0; i <= listBox1.Width; i++)
                        listBox1.Items[tmp] += "-";
                    listBox1.Items.Add("!Exeption!");
                    listBox1.Items.Add("");
                    tmp += 2;
                    foreach (var item in ex.ToString())
                    {
                        if (listBox1.Items[tmp].ToString().Length > 60 && item == ' ')
                        {
                            listBox1.Items[tmp] += (item).ToString();
                            tmp++;
                            listBox1.Items.Add("");
                        }
                        else
                            listBox1.Items[tmp] += item.ToString();
                    }
                }));
            }
            catch 
            {
                return;
            }
        }
        private void Print_log()
        {
            try
            {
                if (LOG == null)
                    return;
                Invoke(new Action(() =>
                {
                    int tmp = listBox1.Items.Count;
                    string str = null;
                    listBox1.Items.Add("");
                    for (int i = 0; i <= listBox1.Width; i++)
                    {
                        if (i < 35)
                            LOG.logFile += "-";
                        listBox1.Items[tmp] += "-";
                    }
                    LOG.logFile += "\n";
                    listBox1.Items.Add("log:"); 
                    LOG.logFile += "log:\n";
                    listBox1.Items.Add("Finish time: " + LOG.DateTime.ToUniversalTime().ToString());
                    LOG.logFile += "Finish time: " + LOG.DateTime.ToUniversalTime().ToString() + "\n";
                    listBox1.Items.Add("Replaces: " + LOG.replaceCount);
                    LOG.logFile += "Replaces: " + LOG.replaceCount + "\n";
                    listBox1.Items.Add("Count of  \".txt\" files: " + LOG.fileInfo.Count());
                    LOG.logFile += "Count of  \".txt\" files: " + LOG.fileInfo.Count() + "\n";
                    listBox1.Items.Add("Approachless \".txt\" file: " + LOG.approachless);
                    LOG.logFile += "Approachless \".txt\" file: " + LOG.approachless + "\n";
                    for (int j = 0; j < LOG.fileInfo.Count(); ++j)
                    {
                        listBox1.Items.Add("Find file: " + LOG.fileInfo[j].Name);
                        LOG.logFile += "Find file: " + LOG.fileInfo[j].Name + "\n";
                    }
                    for (int i = 1; i <= LOG.top.Count(); ++i)
                    {
                        if (LOG.top[i - 1].Word != "Null")
                        {
                            str = (i == 10) ? $"{i}" : $"  {i}";
                            listBox1.Items.Add($"Top {str} word: " + $"\"{ LOG.top[i - 1].Word}\"" + "\tfreq: " + LOG.top[i - 1].frequency);
                            LOG.logFile += $"Top {str} word: " + $"\"{ LOG.top[i - 1].Word}\"" + "\tfreq: " + LOG.top[i - 1].frequency + "\n";  
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                using (new CenterWinDialog(this)) { MessageBox.Show(ex.ToString()); }
            }
        }
        private void LogWriter(bool sweetch)
        {
            if ((progress == 100 || sweetch == false) && LOG.isWrite == false)
            {
                LOG.isWrite = true;
                String directory = FWC.DirectoryPath + "\\log\\log.txt";
                TC.thread = null;
                LOG.DateTime = DateTime.Now;
                LOG.fileInfo.OrderBy(f => f.Name);
                LOG.top.OrderBy(t => t.frequency);
                Invoke(new Action(() =>
                {
                    if (sweetch == false)
                        listBox1.Items.Add("Stopped");
                    else
                        listBox1.Items.Add("Done");
                    StartSearch.Enabled = true;
                    StartSearch.BackColor = Color.YellowGreen;
                    Print_log();
                }));
                try
                {
                    using (StreamWriter writer = new StreamWriter(directory))
                    {
                        writer.Write(LOG.logFile);
                    }
                }
                catch (Exception ex)
                {
                    using (new CenterWinDialog(this)) { MessageBox.Show(ex.ToString()); }
                }
            }
            else return;
        }
        private IEnumerable<String> GetAllFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern).Union(
                   Directory.EnumerateDirectories(path).SelectMany(d =>
                   {
                       try
                       {
                           Invoke(new Action(() => { listBox1.Items[0] = d; }));
                           return GetAllFiles(d, searchPattern);
                       }
                       catch (UnauthorizedAccessException e)
                       {
                           return Enumerable.Empty<String>();
                       }
                   }));
        }
        private void WordsChecker(String str, bool ifFind, PoolThreadData threadData)
        {
            try
            {
                String filepath = threadData.path;
                FileInfo fi = new FileInfo(filepath);
                int count = 0;
                int Mcount = 0;
                foreach (String line in FWC.Words)
                {
                    if (filepath != FWC.FilePath && Regex.IsMatch(str, line, RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    {
                        if (threadData.Token.IsCancellationRequested)
                            threadData.Token.ThrowIfCancellationRequested();
                        fi.Attributes = FileAttributes.Normal;
                        fi.MoveTo(FWC.DirectoryPath + "\\Found\\" + fi.Name);
                        ifFind = true;
                        count = 0;
                        Mcount = Regex.Matches(str, line, RegexOptions.IgnoreCase).Count;
                        str = Regex.Replace(str, line, "*******", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                        lock (resLocker)
                        {
                            LOG.replaceCount += Mcount;
                            foreach (var top in LOG.top)
                            {
                                if (threadData.Token.IsCancellationRequested)
                                    return;
                                if (top.Word == line)
                                {
                                    count++;
                                    top.frequency += Mcount;
                                }
                                else if (top.Word != line && Mcount > top.frequency && count == 0)
                                {
                                    count++;
                                    top.Word = line;
                                    top.frequency = Mcount;
                                }
                            }
                        }
                    }
                }
                if (ifFind)
                {
                    lock (resLocker)
                    {
                        LOG.fileInfo.Add(new FileInfo(filepath));
                    }
                    using (StreamWriter file = new StreamWriter(filepath))
                    {
                        file.Write(str);
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ifFind)
                {
                    lock (resLocker) { LOG.approachless++; }
                    return;
                }
                else throw ex;
            }
        }
        private void FormatFileAnalizator(object par)
        {
            var threadData = par as PoolThreadData;
            if (threadData == null || threadData.Token.IsCancellationRequested) return;
            if (progressBar1.Maximum < 1000)
                Thread.Sleep(500);
            else if (progress < 50)
                Thread.Sleep(100);
            String filepath = threadData.path;
            String str = string.Empty;
            bool ifFind = false;
            try
            {
                if (PauseSearch.Enabled == false)
                    TC._manualEvent.WaitOne();
                lock (resLocker)
                {
                    Invoke(new Action(() =>
                    {
                        if (threadData.Token.IsCancellationRequested) return;
                        progress = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100.0);
                        if (progressBar1.Value < progressBar1.Maximum)
                            progressBar1.Value++;
                        if (progressBar1.Value == progressBar1.Maximum)
                            progress = 100;
                        listBox1.Items[1] = filepath;
                        listBox1.Items[2] = "Progress..." + progress.ToString() + "%";
                    }));
                }
                if (!Regex.IsMatch(filepath, @"\w*txt$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    return;
                foreach (var log in LOG.fileInfo)
                    if (log.FullName == filepath || threadData.Token.IsCancellationRequested)
                        return;
                using (StreamReader reader = File.OpenText(filepath))
                {
                    str = reader.ReadToEnd();
                    reader.Close();
                }
                WordsChecker(str, ifFind, threadData);
            }
            catch (Exception e)
            {
                if (threadData.Token.IsCancellationRequested)
                    return;
                ExeptionListbox(e);
            }
            finally
            {
                if (progressBar1.Value == progressBar1.Maximum && progress == 100)
                    LogWriter(true);
            }
        }
        private void Search(object obj)
        {
            try
            {
                String foalder = obj as String;
                int tmp = foalder == null ? drives.Count() : 1;
                for (int j = 0; j < tmp; ++j) 
                {
                    var files = GetAllFiles(foalder == null ? drives[j].Name : foalder, "*") ;
                    foreach (var file in files)
                    {
                        if (PauseSearch.Enabled == false)
                            TC._manualEvent.WaitOne();
                        Invoke(new Action(() => { progressBar1.Maximum++; }));
                        if (TC.cts.Token.IsCancellationRequested)
                            TC.cts.Token.ThrowIfCancellationRequested();
                        ThreadPool.QueueUserWorkItem(FormatFileAnalizator, new PoolThreadData { path = file, Token = TC.cts.Token });
                    }
                }
            }
            catch (Exception ex) { if (PauseSearch.Enabled && StopSearch.Enabled) ExeptionListbox(ex); }
        }
        #endregion
        #region Button
        private void StartSearch_Click(object sender, EventArgs e)
        {
            StartSearch.Text = "Start Search";
            if (FWC.Words.Count == 0)
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("No Words"); }
                return;
            }
            StartSearch.Enabled = false;
            StopSearch.Enabled = true;
            PauseSearch.Enabled = true;
            StartSearch.BackColor = Color.Empty;
            if (TC.thread == null)
            {
                LOG.isWrite = false;
                String str = null;
                Question q = new Question();
                DialogResult d = q.ShowDialog();
                if (d == DialogResult.Cancel)
                {
                    StartSearch.Enabled = true;
                    StopSearch.Enabled = false;
                    PauseSearch.Enabled = true;
                    StartSearch.BackColor = Color.YellowGreen;
                    return;
                }
                if(d == DialogResult.No)
                    str = FWC.OpenFolder(this);
                progressBar1.Maximum = 0;
                progressBar1.Value = 0;
                listBox1.Items.Clear();
                LOG.Clear();
                for (int i = 0; i < 3; ++i)
                {
                    if (listBox1.Items.Count == 3)
                        break;
                    listBox1.Items.Add("");
                }
                TC._manualEvent = new ManualResetEvent(true);
                TC.cts = new CancellationTokenSource();
                TC.thread = new Thread(Search);
                TC.thread.Start(str);
            }
            else
                TC._manualEvent.Set();
        }
        private void PauseSearch_Click(object sender, EventArgs e)
        {
            if (!StartSearch.Enabled)
            {
                PauseSearch.Enabled = false;
                TC._manualEvent.Reset();
                StartSearch.Text = "Continue Search";
                using (new CenterWinDialog(this)) { MessageBox.Show("Paused..."); }
                StartSearch.Enabled = true;
                StartSearch.BackColor = Color.YellowGreen;
            }
            else
                return;
        }
        private void StopSearch_Click(object sender, EventArgs e)
        {
            if (!PauseSearch.Enabled)
            {
                TC._manualEvent.Set();
                PauseSearch.Enabled = true;
            }
            TC._manualEvent.Dispose();
            StopSearch.Enabled = false;
            TC.cts?.Cancel();
            TC.thread = null;
            progressBar1.Maximum = 0;
            progressBar1.Value = 0;
            LogWriter(false);
            using (new CenterWinDialog(this)) { MessageBox.Show("Stopped!"); }
            StartSearch.Text = "Start Search";
            StartSearch.BackColor = Color.YellowGreen;
            StartSearch.Enabled = true;
        }
        private void CreateWords_Click(object sender, EventArgs e)
        {
            Form CreateFW = new CreateFW(FWC);
            CreateFW.ShowDialog();
        }
        #endregion

        private void ForbiddenWords_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TC.thread != null)
                StopSearch_Click(sender, e);
        }
    }
}
