using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Spy
{
    public class Logs
    {
        private static Logs instance = null;
        private static object resLocker = new object();
        public String LogFilePath { get; set; }
        public List<String> StatisticsKeyDown { get; set; }
        public List<String> StatisticsProcesseRun { get; set; }
        public List<String> ManageInputWords { get; set; }
        public List<String> ManageProcesses { get; set; }
        private Logs() { }
        public static Logs getInstance(string path)
        {
            try
            {
                if (instance == null)
                    lock (resLocker)
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(Logs));
                            instance = (Logs)serializer.Deserialize(reader);
                        }
                    }
                return instance;
            }
            catch
            {
                if (instance == null)
                    lock (resLocker)
                    {
                        instance = new Logs();
                        instance.LogFilePath = path;
                        instance.StatisticsKeyDown = new List<String>();
                        instance.StatisticsProcesseRun = new List<String>();
                        instance.ManageInputWords = new List<String>();
                        instance.ManageProcesses = new List<String>();
                    }
                return instance;
            }
        }
        public void Save(Option option)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                using (var writer = new StreamWriter(option.LogFilePath))
                { serializer.Serialize(writer, instance); }
            }
            catch (Exception ex)
            {
                option.LogFilePath = option.DirectoryPath + "\\LOG\\Logs.xml";
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                using (var writer = new StreamWriter(option.LogFilePath))
                { serializer.Serialize(writer, instance); }
                MessageBox.Show(ex.Message + $"New log path {option.LogFilePath}");
            }
        }
    }
}
