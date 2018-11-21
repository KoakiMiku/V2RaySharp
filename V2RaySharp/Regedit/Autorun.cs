using Microsoft.Win32;
using System.Diagnostics;

namespace V2RaySharp.Regedit
{
    class Autorun
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Add()
        {
            RegistryKey autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autorun.SetValue("V2RaySharp", $"{path} -start");
            autorun.Close();
        }

        public static void Remove()
        {
            RegistryKey autoRun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autoRun.DeleteValue("V2RaySharp", false);
            autoRun.Close();
        }
    }
}
