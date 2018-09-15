using System;
using System.Windows.Forms;

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
                            break;
                        default:
                            break;
                    }
                }
                else if (args.Length == 0 && !isAdmin)
                {
                    Administrator.RunAsAdmin();
                }
                else
                {
                    switch (args[0])
                    {
                        case "-start":
                            V2Ray.Start();
                            SystemProxy.Enable();
                            break;
                        case "-restart":
                            V2Ray.Restart();
                            SystemProxy.Enable();
                            break;
                        case "-config":
                            V2Ray.Config();
                            break;
                        case "-exit":
                            V2Ray.Exit();
                            SystemProxy.Disable();
                            break;
                        default:
                            break;
                    }
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
