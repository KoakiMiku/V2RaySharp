using System;
using System.Diagnostics;
using System.IO;

namespace V2RaySharp.Utiliy
{
    class Loopback
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string execute = Path.Combine(path, "EnableLoopback.exe");

        public static void Start()
        {
            Check();
            var process = new Process();
            process.StartInfo.FileName = execute;
            process.Start();
        }

        private static void Check()
        {
            if (!File.Exists(execute))
            {
                File.WriteAllBytes(execute, Properties.Resources.EnableLoopback);
            }
        }
    }
}
