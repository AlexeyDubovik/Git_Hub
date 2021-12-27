using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forbidden_Worlds
{
    internal class log
    {
        public log(String _directory)
        {
            directory = _directory;
            replaceCount = 0;
            approachless = 0;
            fileInfo = new List<FileInfo> { };
            DateTime = DateTime.Now;
            isWrite = false;
            top = new Top10 [10];
            for (int i = 0; i < top.Count(); ++i)
                top[i] = new Top10();
            FindLogFile(directory);
            CreateDirectory(directory + "\\Found");
        }
        public DateTime DateTime { get; set; }
        public int replaceCount { get; set; }
        public int approachless { get; set; }
        public bool isWrite { get; set; }
        public List<FileInfo> fileInfo { get; set; }
        public Top10[] top { get; set; }
        public String logFile { get; set; }
        public String directory { get; set; }
        private void CreateDirectory(String path)
        {
            String directory = path;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            else return;
        }
        private async void FindLogFile(String Path)
        {
            String directory = Path + "\\log";
            CreateDirectory(directory);
            directory += "\\log.txt";
            FileInfo fi = new FileInfo(directory);
            if (!fi.Exists)
                fi.Create();
            else
                using (StreamReader Rstream = fi.OpenText())
                {
                    logFile = await Rstream.ReadToEndAsync();
                }
        }
        public void Clear()
        {
            replaceCount = 0;
            approachless = 0;
            fileInfo = new List<FileInfo> { };
            DateTime = DateTime.Now;
        }
    }
}
