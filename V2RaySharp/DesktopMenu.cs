using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace V2RaySharp
{
    class DesktopMenu
    {
        static readonly string desktopPath = @"DesktopBackground\Shell\V2Ray";
        static readonly string shellPath = @"DesktopBackground\Shell\V2Ray\Shell";

        static readonly string startPath = @"DesktopBackground\Shell\V2Ray\Shell\1";
        static readonly string startCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\1\command";
        static readonly string restartPath = @"DesktopBackground\Shell\V2Ray\Shell\2";
        static readonly string restartCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\2\command";
        static readonly string configPath = @"DesktopBackground\Shell\V2Ray\Shell\3";
        static readonly string configCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\3\command";
        static readonly string exitPath = @"DesktopBackground\Shell\V2Ray\Shell\4";
        static readonly string exitCommandPath = @"DesktopBackground\Shell\V2Ray\Shell\4\command";

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

            #region Start
            RegistryKey start = Registry.ClassesRoot.CreateSubKey(startPath);
            start.SetValue("MUIVerb", $"{Language.GetString("Start")}(&S)");
            start.Close();
            RegistryKey startCmd = Registry.ClassesRoot.CreateSubKey(startCommandPath);
            startCmd.SetValue("", $"{Application.ExecutablePath} -start");
            startCmd.Close();
            #endregion

            #region Restart
            RegistryKey restart = Registry.ClassesRoot.CreateSubKey(restartPath);
            restart.SetValue("MUIVerb", $"{Language.GetString("Restart")}(&R)");
            restart.Close();
            RegistryKey restartCmd = Registry.ClassesRoot.CreateSubKey(restartCommandPath);
            restartCmd.SetValue("", $"{Application.ExecutablePath} -restart");
            restartCmd.Close();
            #endregion

            #region Config
            RegistryKey config = Registry.ClassesRoot.CreateSubKey(configPath);
            config.SetValue("MUIVerb", $"{Language.GetString("Config")}(&C)");
            config.Close();
            RegistryKey configCmd = Registry.ClassesRoot.CreateSubKey(configCommandPath);
            configCmd.SetValue("", $"{Application.ExecutablePath} -config");
            configCmd.Close();
            #endregion

            #region Exit
            RegistryKey exit = Registry.ClassesRoot.CreateSubKey(exitPath);
            exit.SetValue("MUIVerb", $"{Language.GetString("Exit")}(&E)");
            exit.Close();
            RegistryKey exitCmd = Registry.ClassesRoot.CreateSubKey(exitCommandPath);
            exitCmd.SetValue("", $"{Application.ExecutablePath} -exit");
            exitCmd.Close();
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
