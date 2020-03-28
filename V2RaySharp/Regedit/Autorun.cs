using System.Diagnostics;
using Microsoft.Win32;

namespace V2RaySharp.Regedit
{
    class Autorun
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Add()
        {
            var autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autorun.SetValue("V2RaySharp", $"{path} -start");
            autorun.Close();
        }

        public static void Remove()
        {
            var autoRun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autoRun.DeleteValue("V2RaySharp", false);
            autoRun.Close();
        }
    }
}
