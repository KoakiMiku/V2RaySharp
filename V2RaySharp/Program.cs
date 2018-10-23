using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using V2RaySharp.Controller;
using V2RaySharp.Regedit;
using V2RaySharp.View;

namespace V2RaySharp
{
    static class Program
    {
        private static readonly string name = "V2Ray Sharp";

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            try
            {
                Mutex mutex = new Mutex(false, name);
                if (!mutex.WaitOne(0, false))
                {
                    throw new Exception(I18N.GetString("AlreadyRunning"));
                }

                bool isTrueDirectory = Check.IsTrueDirectory();
                if (!isTrueDirectory)
                {
                    throw new Exception(I18N.GetString("FileNotFound"));
                }

                bool isAdmin = Administrator.IsAdmin();
                if (args.Length == 0 && isAdmin)
                {
                    switch (MessageBox.Show($"{I18N.GetString("Setup")}", name,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                    {
                        case DialogResult.Yes:
                            Autorun.Add();
                            DesktopMenu.Add();
                            break;
                        case DialogResult.No:
                            Autorun.Remove();
                            DesktopMenu.Remove();
                            V2Ray.Stop();
                            break;
                        default:
                            break;
                    }
                }
                else if (args.Length == 0 && !isAdmin)
                {
                    Administrator.RunAsAdmin();
                }
                else if (args[0] == "-start")
                {
                    V2Ray.Start();
                }
                else if (args[0] == "-stop")
                {
                    V2Ray.Stop();
                }
                else if (args[0] == "-config")
                {
                    Application.Run(new FormMain());
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
