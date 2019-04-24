using Microsoft.Win32;
using System.Diagnostics;

namespace V2RaySharp.Regedit
{
    internal class Autorun
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        internal static void Add()
        {
            var autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autorun.SetValue("V2RaySharp", $"{path} -start");
            autorun.Close();
        }

        internal static void Remove()
        {
            var autoRun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autoRun.DeleteValue("V2RaySharp", false);
            autoRun.Close();
        }
    }
}
