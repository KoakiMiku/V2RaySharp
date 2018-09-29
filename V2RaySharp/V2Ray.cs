using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace V2RaySharp
{
    class V2Ray
    {
        public static void Start()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "wv2ray.exe");
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.Start();
                SystemProxy.Enable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Restart()
        {
            try
            {
                Exit();
                Thread.Sleep(1000);
                Start();
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

        public static void Exit()
        {
            try
            {
                Process[] process = Process.GetProcessesByName("wv2ray");
                foreach (var item in process)
                {
                    item.Kill();
                }
                SystemProxy.Disable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Switch()
        {
            try
            {
                Process[] process = Process.GetProcessesByName("wv2ray");
                if (process.Length == 0)
                {
                    Start();
                }
                else
                {
                    Exit();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
