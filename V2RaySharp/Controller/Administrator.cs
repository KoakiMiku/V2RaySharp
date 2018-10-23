using System;
using System.Diagnostics;
using System.Security.Principal;

namespace V2RaySharp.Controller
{
    class Administrator
    {
        public static bool IsAdmin()
        {
            try
            {
                WindowsIdentity current = WindowsIdentity.GetCurrent();
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
                return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void RunAsAdmin()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                process.StartInfo.Verb = "runas";
                process.Start();
            }
            catch (Exception)
            {
                //throw;
            }
        }
    }
}
