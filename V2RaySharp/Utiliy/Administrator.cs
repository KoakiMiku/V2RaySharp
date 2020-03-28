using System.Security.Principal;

namespace V2RaySharp.Utiliy
{
    class Administrator
    {
        public static bool IsAdmin()
        {
            var current = WindowsIdentity.GetCurrent();
            var windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
