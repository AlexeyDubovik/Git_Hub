using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Spy
{
    internal class Processes
    {
        private static Processes instance = null;
        private static object resLocker = new object();
        private Processes() { }
        public static Processes Instance
        {
            get
            {
                if (instance == null) 
                {
                    lock (resLocker)
                    {
                        instance = new Processes();
                    }
                }
                return instance;
            }
        }
        Process[] processes;
        public void ManageProcesses(Logs log, List<String> FProc, CancellationToken token)
        {
            int count = 0;
            int tmp = 0;
            while (!token.IsCancellationRequested)
            {
                count = Process.GetProcesses().Count();
                if (count != tmp)
                {
                    tmp = count;
                    processes = Process.GetProcesses();
                    foreach (var process in processes)
                        try
                        {
                            foreach (var fproc in FProc)
                                if (process.ProcessName == fproc)
                                {
                                    lock (resLocker)
                                    {
                                        log.ManageProcesses[log.ManageProcesses.Count - 1] += 
                                            "Process ID:" + process.Id + " name:" + process.ProcessName + "\n";
                                    }
                                    process.Kill();
                                }
                        }
                        catch
                        {

                        }
                }
            }
        }
        public void StatisticsProcesses(Logs log, CancellationToken token)
        {
            DateTime start = DateTime.Now;
            Process tmpProc = Process.GetCurrentProcess();
            processes = Process.GetProcesses();
            while (!token.IsCancellationRequested)
            {
                processes = Process.GetProcesses();
                foreach (var process in processes)
                    try
                    {
                        if (process.StartTime > start)
                            if (process.StartTime > tmpProc.StartTime && process.Id != tmpProc.Id)
                            {
                                lock (resLocker)
                                {
                                    log.StatisticsProcesseRun[log.StatisticsProcesseRun.Count - 1] += 
                                        "Process StartTime:" + process.StartTime + " ID:" + process.Id + " name:" + process.ProcessName + "\n";
                                }
                                tmpProc = process;
                            }
                    }
                    catch
                    {

                    }
            }
        }
    }
}
