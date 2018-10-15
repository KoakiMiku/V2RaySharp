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
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            try
            {
                Mutex mutex = new Mutex(false, Application.ProductName);
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show($"{Language.GetString("AlreadyRunning")}", Application.ProductName,
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool isAdmin = Administrator.IsAdmin();
                if (args.Length == 0 && isAdmin)
                {
                    switch (MessageBox.Show($"{Language.GetString("Setup")}", Application.ProductName,
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
                MessageBox.Show(ex.Message, Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
