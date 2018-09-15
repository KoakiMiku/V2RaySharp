using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace V2RaySharp
{
    class SystemProxy
    {
        static readonly string proxyPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
        static readonly string localIPEndpoint = "127.0.0.1:1080";
        static readonly string proxyOverride = "<local>";
        static readonly List<string> privateIPAddress = new List<string>() {
            "localhost", "127.*", "10.*", "192.168.*",
            "172.16.*", "172.17.*", "172.18.*", "172.19.*", "172.20.*", "172.21.*",
            "172.22.*", "172.23.*", "172.24.*", "172.25.*", "172.26.*", "172.27.*",
            "172.28.*", "172.29.*", "172.30.*", "172.31.*",
            proxyOverride
        };

        public static void Enable()
        {
            RegistryKey autorun = Registry.CurrentUser.OpenSubKey(proxyPath, true);
            autorun.SetValue("ProxyEnable", 1);
            autorun.SetValue("ProxyServer", localIPEndpoint);
            autorun.SetValue("ProxyOverride", string.Join(";", privateIPAddress));
            autorun.Close();
            Update();
        }

        public static void Disable()
        {
            RegistryKey autorun = Registry.CurrentUser.OpenSubKey(proxyPath, true);
            autorun.SetValue("ProxyEnable", 0);
            autorun.SetValue("ProxyServer", localIPEndpoint);
            autorun.SetValue("ProxyOverride", proxyOverride);
            autorun.Close();
            Update();
        }

        private static void Update()
        {
            InternetSetOption(IntPtr.Zero, internetOptionProxySettingsChanged, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, internetOptionRefresh, IntPtr.Zero, 0);
        }

        static readonly int internetOptionRefresh = 37;
        static readonly int internetOptionProxySettingsChanged = 95;

        [DllImport("wininet.dll")]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
    }
}
