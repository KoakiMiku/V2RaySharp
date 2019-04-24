using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace V2RaySharp.Utiliy
{
    internal class SingleInstance
    {
        private static readonly string name = "V2RaySharp";
        private static Process process = Process.GetCurrentProcess();
        private static Process[] processes = Process.GetProcessesByName(name);

        internal static bool IsSingle()
        {
            if (processes.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static void SetForeground()
        {
            foreach (var item in processes)
            {
                if (item.Id != process.Id)
                {
                    SetForegroundWindow(item.MainWindowHandle);
                    return;
                }
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
