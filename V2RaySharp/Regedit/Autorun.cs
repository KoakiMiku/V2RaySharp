using Microsoft.Win32;
using System.Windows.Forms;

namespace V2RaySharp.Regedit
{
    class Autorun
    {
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Add()
        {
            RegistryKey autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
            autorun.SetValue("V2RaySharp", $"{Application.ExecutablePath} -start");
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
