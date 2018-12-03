using System;
using System.Windows;
using V2RaySharp.Controller;
using V2RaySharp.Regedit;
using V2RaySharp.View;

namespace V2RaySharp
{
    public partial class App : Application
    {
        private static readonly string name = "V2RaySharp";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                bool isSingle = SingleInstance.IsSingle();
                if (!isSingle)
                {
                    SingleInstance.SetForeground();
                    return;
                }

                bool isTrueDirectory = Check.IsTrueDirectory();
                if (!isTrueDirectory)
                {
                    throw new Exception(I18N.GetString("FileNotFound"));
                }

                bool isAdmin = Administrator.IsAdmin();
                if (e.Args.Length == 0 && isAdmin)
                {
                    switch (MessageBox.Show($"{I18N.GetString("Setup")}", name,
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Information))
                    {
                        case MessageBoxResult.Yes:
                            Autorun.Add();
                            DesktopMenu.Add();
                            break;
                        case MessageBoxResult.No:
                            Autorun.Remove();
                            DesktopMenu.Remove();
                            V2Ray.Stop();
                            break;
                        default:
                            break;
                    }
                }
                else if (e.Args.Length == 0 && !isAdmin)
                {
                    Administrator.RunAsAdmin();
                }
                else if (e.Args[0] == "-start")
                {
                    V2Ray.Start();
                }
                else if (e.Args[0] == "-stop")
                {
                    V2Ray.Stop();
                }
                else if (e.Args[0] == "-config")
                {
                    MainWindow window = new MainWindow();
                    window.ShowDialog();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}
