using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace V2RaySharp.Regedit
{
    internal class DesktopMenu
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string desktopPath = @"DesktopBackground\Shell\V2RaySharp";
        private static readonly string commandPath = @"DesktopBackground\Shell\V2RaySharp\command";

        internal static void Add()
        {
            try
            {
                var desktop = Registry.ClassesRoot.CreateSubKey(desktopPath);
                desktop.SetValue("", "V2RaySharp(&Z)");
                desktop.SetValue("Icon", path);
                desktop.SetValue("Position", "Bottom");
                desktop.Close();
                var command = Registry.ClassesRoot.CreateSubKey(commandPath);
                command.SetValue("", $"{path} -config");
                command.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void Remove()
        {
            try
            {
                var desktop = Registry.ClassesRoot;
                desktop.DeleteSubKeyTree(desktopPath, false);
                desktop.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
