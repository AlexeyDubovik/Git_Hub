using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forbidden_Worlds
{
    internal class FW_Control
    {
        public FW_Control(string path)
        {
            DirectoryPath = GetBinPath(path);
            FilePath = DirectoryPath + "\\ForbiddenWords.txt";
            Words = new List<string>();
            Find_FW_File(FilePath);
        }
        public String DirectoryPath { get; set; }
        public String FilePath { get; set; }
        public List<String> Words { get; set; }
        private async void Find_FW_File(String Path)
        {
            FileInfo fi = new FileInfo(Path);
            if (!fi.Exists)
                fi.Create();
            else
                using (StreamReader Rstream = fi.OpenText())
                {
                    String str = await Rstream.ReadToEndAsync();
                    await Task.Run(() => ReadFWfile(str));
                }
        }
        private void ReadFWfile(String str)
        {
            var array = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            if (array.Count() != 0)
                foreach (var item in array)
                    Words.Add(item);
        }
        private String GetBinPath(String currentDir)
        {
            String tmp = currentDir;
            int binIndex = tmp.IndexOf("\\bin\\");
            String Path = (binIndex == -1 ? currentDir : currentDir.Substring(0, binIndex + ("bin").Length + 1));
            return Path;
        }
        public String OpenFolder(IWin32Window obj)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "Choose search directory";
            if (fd.ShowDialog(obj) == DialogResult.OK) { return fd.SelectedPath; }
            else { return null; }
        }
    }
}
