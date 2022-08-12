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
using System.Text.RegularExpressions;

namespace Forbidden_Worlds
{
    public partial class CreateFW : Form
    {
        FW_Control FWControl;
        public CreateFW(object obj)
        {
            InitializeComponent();
            try
            {
                FWControl = obj as FW_Control;
            }
            catch 
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("File Invalid"); }
                this.Close();
            }
        }
        private void CreateFW_Load(object sender, EventArgs e)
        {
            FWControl.Words.Sort();
            ShowFW_Words(null);
        }
        #region Func
        private void Writer()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FWControl.DirectoryPath + "\\ForbiddenWords.txt"))
                {
                    foreach (string str in FWControl.Words)
                        writer.Write(str + " ");
                }
            }
            catch (Exception ex)
            {
                FWControl.Words.Remove(WordBox.Text);
                using (new CenterWinDialog(this)) { MessageBox.Show(ex.ToString()); }
            }
        }
        private void ShowFW_Words(Task tsk)
        {
            if (WordsListBox.Items.Count > 0)
                Invoke(new Action(() => { WordsListBox.Items.Clear(); }));
            if (FWControl.Words.Count() > 0)
                foreach (string str in FWControl.Words)
                    Invoke(new Action(() => { WordsListBox.Items.Add(str); }));
        }
        #endregion
        #region Button
        private void AddWord_Click(object sender, EventArgs e)
        {
            foreach(string str in FWControl.Words)
                if(str == WordBox.Text)
                {
                    using (new CenterWinDialog(this)) { MessageBox.Show("This word is already in list"); }
                    return;
                }
            if (WordBox.Text.Length == 0 || Regex.IsMatch(WordBox.Text, @"\d") || Regex.IsMatch(WordBox.Text, @"\W"))
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("Incorrect Format"); }
                return;
            }
            FWControl.Words.Add(WordBox.Text);
            FWControl.Words.Sort();
            Task.Run(() => { Writer(); }).ContinueWith(ShowFW_Words);
        }
        private void DeleteWord_Click(object sender, EventArgs e)
        {
            if (WordBox.Text.Length == 0 || Regex.IsMatch(WordBox.Text, @"\d") || Regex.IsMatch(WordBox.Text, @"\W"))
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("Incorrect Format"); }
                return;
            }
            if (!FWControl.Words.Exists(x => x.Contains(WordBox.Text)))
            {
                using (new CenterWinDialog(this)) { MessageBox.Show("Word undetected"); }
                return;
            }
            FWControl.Words.Remove(WordBox.Text);
            Task.Run(() => { Writer(); }).ContinueWith(ShowFW_Words);
        }
        private void WordBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (WordBox.Text == "Enter World")
                WordBox.Text = "";
        }
        #endregion
        #region Event
        private void WordsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WordBox.Text = WordsListBox.SelectedItem.ToString();
        }
        #endregion
    }
}
