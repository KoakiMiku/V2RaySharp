using System;
using System.IO;

namespace V2RaySharp.Controller
{
    class Check
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string v2ctl = Path.Combine(path, "v2ctl.exe");
        private static readonly string v2ray = Path.Combine(path, "v2ray.exe");
        private static readonly string wv2ray = Path.Combine(path, "wv2ray.exe");

        public static bool IsTrueDirectory()
        {
            try
            {
                bool file1 = File.Exists(v2ctl);
                bool file2 = File.Exists(v2ray);
                bool file3 = File.Exists(wv2ray);
                if (file1 && file2 && file3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
