using System;
using System.Diagnostics;
using System.IO;

namespace V2RaySharp
{
    class V2Ray
    {
        public static void Switch()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("wv2ray");
                if (processes.Length == 0)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = Path.Combine(AppContext.BaseDirectory, "wv2ray.exe");
                    process.Start();
                    SystemProxy.Enable();
                }
                else
                {
                    SystemProxy.Disable();
                    foreach (var item in processes)
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Config()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "config.json");
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
