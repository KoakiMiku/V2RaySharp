using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace V2RaySharp.Regedit
{
    class Autorun
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Add()
        {
            try
            {
                RegistryKey autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
                autorun.SetValue("V2RaySharp", $"{path} -start");
                autorun.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Remove()
        {
            try
            {
                RegistryKey autoRun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
                autoRun.DeleteValue("V2RaySharp", false);
                autoRun.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
