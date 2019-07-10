using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace V2RaySharp.Utiliy
{
    internal class SystemProxy
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string sysproxyX64 = Path.Combine(path, "sysproxy_x64.exe");
        private static readonly string sysproxyX86 = Path.Combine(path, "sysproxy_x86.exe");
        private static readonly bool isX64 = Environment.Is64BitOperatingSystem;
        private static readonly string localIPEndpoint = "127.0.0.1:1080";
        private static readonly List<string> privateIPAddressList = new List<string>() {
            "localhost", "127.*",
            "10.*", "169.254.*", "192.168.*",
            "172.16.*", "172.17.*", "172.18.*", "172.19.*", "172.20.*", "172.21.*",
            "172.22.*", "172.23.*", "172.24.*", "172.25.*", "172.26.*", "172.27.*",
            "172.28.*", "172.29.*", "172.30.*", "172.31.*",
            "<local>"
        };
        private static readonly string privateIPAddress = string.Join(";", privateIPAddressList);

        internal static void Enable()
        {
            Check();
            var process = new Process();
            process.StartInfo.FileName = isX64 ? sysproxyX64 : sysproxyX86;
            process.StartInfo.Arguments = $"global {localIPEndpoint} {privateIPAddress}";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }

        internal static void Disable()
        {
            Check();
            var process = new Process();
            process.StartInfo.FileName = isX64 ? sysproxyX64 : sysproxyX86;
            process.StartInfo.Arguments = $"set 1 {localIPEndpoint} {privateIPAddress} {string.Empty}";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }

        private static void Check()
        {
            if (!File.Exists(sysproxyX64))
            {
                File.WriteAllBytes(sysproxyX64, Properties.Resources.sysproxy_x64);
            }
            if (!File.Exists(sysproxyX86))
            {
                File.WriteAllBytes(sysproxyX86, Properties.Resources.sysproxy_x86);
            }
        }
    }
}
