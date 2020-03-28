using System.Diagnostics;
using System.Security.Principal;

namespace V2RaySharp.Utiliy
{
    class Administrator
    {
        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;

        public static bool IsAdmin()
        {
            var current = WindowsIdentity.GetCurrent();
            var windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RunAsAdmin()
        {
            var process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Verb = "runas";
            process.Start();
        }
    }
}
