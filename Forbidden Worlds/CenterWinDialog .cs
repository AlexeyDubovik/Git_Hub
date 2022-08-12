using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Forms.VisualStyles;

namespace Forbidden_Worlds
{
    internal class CenterWinDialog : IDisposable
    {
        private Int32 _mTries;
        private readonly Form _mOwner;
        public CenterWinDialog(Form owner)
        {
            _mOwner = owner;
            Dispatcher.CurrentDispatcher.BeginInvoke(new MethodInvoker(FindDialog));
        }
        private void FindDialog()
        {
            if (_mTries < 0) return;
            var callback = new EnumThreadWndProc(CheckWindow);
            if (EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero))
            {
                if (++_mTries < 10)
                    Dispatcher.CurrentDispatcher.BeginInvoke(new MethodInvoker(FindDialog));
            }
        }
        private bool CheckWindow(IntPtr hWnd, IntPtr lp)
        {
            var sb = new StringBuilder(260);
            GetClassName(hWnd, sb, sb.Capacity);
            if (sb.ToString() != "#32770") return true;
            var frmRect = new Rectangle(new System.Drawing.Point((Int32)_mOwner.Left, (Int32)_mOwner.Top),
                                              new System.Drawing.Size((Int32)_mOwner.Width, (Int32)_mOwner.Height));
            Rect dlgRect;
            GetWindowRect(hWnd, out dlgRect);
            MoveWindow(hWnd,
                frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) / 2,
                frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) / 2,
                dlgRect.Right - dlgRect.Left,
                dlgRect.Bottom - dlgRect.Top, true);
            return false;
        }
        public void Dispose()
        {
            _mTries = -1;
        }
        private delegate Boolean EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
        [DllImport("user32.dll")]
        private static extern Boolean EnumThreadWindows(Int32 tid, EnumThreadWndProc callback, IntPtr lp);
        [DllImport("kernel32.dll")]
        private static extern Int32 GetCurrentThreadId();
        [DllImport("user32.dll")]
        private static extern Int32 GetClassName(IntPtr hWnd, StringBuilder buffer, Int32 buflen);
        [DllImport("user32.dll")]
        private static extern Boolean GetWindowRect(IntPtr hWnd, out Rect rc);
        [DllImport("user32.dll")]
        private static extern Boolean MoveWindow(IntPtr hWnd, Int32 x, Int32 y, Int32 w, Int32 h, Boolean repaint);
        private struct Rect { public Int32 Left; public Int32 Top; public Int32 Right; public Int32 Bottom; }
    }
}
