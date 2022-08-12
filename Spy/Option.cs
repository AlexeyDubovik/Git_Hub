using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Spy
{
    public class Option
    {
        private static Option instance = null;
        private static object resLocker = new object();
        private Option() { }
        public String DirectoryPath { get; set; }
        public String LogFilePath { get; set; }
        public List<String> Words { get; set; }
        public List<String> Processes { get; set; }
        public static Option getInstance(string path)
        {
            try
            {
                if (instance == null)
                    lock (resLocker)
                    {
                        using (StreamReader reader = new StreamReader(GetBin(path) + "\\option.xml"))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Option));
                            instance = (Option)serializer.Deserialize(reader);
                        }
                    }
                instance.DirectoryPath = GetBin(path);
                return instance;
            }
            catch
            {
                if (instance == null)
                    lock (resLocker)
                    {
                        instance = new Option();
                        instance.DirectoryPath = GetBin(path);
                        instance.LogFilePath = instance.DirectoryPath + "\\LOG\\Logs.xml";
                        instance.Words = new List<String>();
                        instance.Processes = new List<String>();
                        instance.CreateDirectory(instance.DirectoryPath + "\\LOG");
                        instance.Create_File(instance.LogFilePath);
                    }
                return instance;
            }
        }
        public String OpenFolder(IWin32Window obj)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "Choose search directory";
            if (fd.ShowDialog(obj) == DialogResult.OK) return fd.SelectedPath; 
            else return null; 
        }
        private static String GetBin(String currentDir)
        {
            String tmp = currentDir;
            int binIndex = tmp.IndexOf("\\bin\\");
            String Path = (binIndex == -1 ? currentDir : currentDir.Substring(0, binIndex + ("bin").Length + 1));
            return Path;
        }
        private void CreateDirectory(String path)
        {
            String directory = path;
            if (!Directory.Exists(directory)) 
                Directory.CreateDirectory(directory);
            else return;
        }
        private void Create_File(String Path)
        {
            FileInfo fi = new FileInfo(Path);
            if (!fi.Exists) fi.Create();
            else return;
        }
        public void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                using (var writer = new StreamWriter(DirectoryPath + "\\option.xml"))
                { serializer.Serialize(writer, instance); }
            }catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }
    }
}
