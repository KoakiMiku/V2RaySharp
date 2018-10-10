using Microsoft.Win32;
using System.Windows.Forms;

namespace V2RaySharp.Regedit
{
    class DesktopMenu
    {
        private static readonly string desktopPath = @"DesktopBackground\Shell\V2RaySharp";
        private static readonly string commandPath = @"DesktopBackground\Shell\V2RaySharp\command";

        public static void Add()
        {
            RegistryKey desktop = Registry.ClassesRoot.CreateSubKey(desktopPath);
            desktop.SetValue("", "V2Ray Sharp (&Z)");
            desktop.SetValue("Icon", Application.ExecutablePath);
            desktop.SetValue("Position", "Bottom");
            desktop.Close();
            RegistryKey command = Registry.ClassesRoot.CreateSubKey(commandPath);
            command.SetValue("", $"{Application.ExecutablePath} -config");
            command.Close();
        }

        public static void Remove()
        {
            RegistryKey desktop = Registry.ClassesRoot;
            desktop.DeleteSubKeyTree(desktopPath, false);
            desktop.Close();
        }
    }
}
