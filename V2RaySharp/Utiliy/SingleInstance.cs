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
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        internal static void SetForeground()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
