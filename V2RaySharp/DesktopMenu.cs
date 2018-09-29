using Microsoft.Win32;
using System.Windows.Forms;

namespace V2RaySharp
{
    class DesktopMenu
    {
        static readonly string desktopPath = @"DesktopBackground\Shell\V2Ray";
        static readonly string shellPath = @"DesktopBackground\Shell\V2Ray\Shell";

        static readonly string switchPath = @"DesktopBackground\Shell\V2Ray\Shell\1";
        static readonly string switchCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\1\command";
        static readonly string configPath = @"DesktopBackground\Shell\V2Ray\Shell\2";
        static readonly string configCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\2\command";

        public static void Add()
        {
            #region Main Menu
            RegistryKey desktop = Registry.ClassesRoot.CreateSubKey(desktopPath);
            desktop.SetValue("MUIVerb", "V2Ray(&Z)");
            desktop.SetValue("Position", "Bottom");
            desktop.SetValue("SubCommands", "");
            desktop.Close();
            RegistryKey shell = Registry.ClassesRoot.CreateSubKey(shellPath);
            shell.Close();
            #endregion

            #region Restart
            RegistryKey switchKey = Registry.ClassesRoot.CreateSubKey(switchPath);
            switchKey.SetValue("MUIVerb", $"{Language.GetString("Switch")}(&S)");
            switchKey.Close();
            RegistryKey switchCmd = Registry.ClassesRoot.CreateSubKey(switchCommandPath);
            switchCmd.SetValue("", $"{Application.ExecutablePath} -switch");
            switchCmd.Close();
            #endregion

            #region Config
            RegistryKey configKey = Registry.ClassesRoot.CreateSubKey(configPath);
            configKey.SetValue("MUIVerb", $"{Language.GetString("Config")}(&C)");
            configKey.Close();
            RegistryKey configCmd = Registry.ClassesRoot.CreateSubKey(configCommandPath);
            configCmd.SetValue("", $"{Application.ExecutablePath} -config");
            configCmd.Close();
            #endregion
        }

        public static void Remove()
        {
            RegistryKey desktop = Registry.ClassesRoot;
            desktop.DeleteSubKeyTree(desktopPath, false);
            desktop.Close();
        }
    }
}
