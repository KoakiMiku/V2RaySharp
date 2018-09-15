using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace V2RaySharp
{
    class Administrator
    {
        public static bool IsAdmin()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RunAsAdmin()
        {
            Process process = new Process();
            process.StartInfo.FileName = Application.ExecutablePath;
            process.StartInfo.Verb = "runas";
            process.Start();
        }
    }
}
