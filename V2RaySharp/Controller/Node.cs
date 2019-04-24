using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using V2RaySharp.Model;
using V2RaySharp.Utiliy;

namespace V2RaySharp.Controller
{
    internal class Node
    {
        internal static string userInfo = string.Empty;
        internal static List<ShadowSocks> sses = new List<ShadowSocks>();
        internal static List<Vmess> vmesses = new List<Vmess>();

        internal static void Upgrade()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Configuration.Config.Subscription))
                {
                    string subscription = Configuration.Config.Subscription;
                    if (!Configuration.Config.Subscription.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !Configuration.Config.Subscription.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        subscription = $"http://{Configuration.Config.Subscription}";
                    }
                    var http = new WebClient();
                    http.DownloadStringCompleted += DownloadComplete;
                    http.DownloadStringAsync(new Uri(subscription));
                }
                else
                {
                    Complete(-2);
                }
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        private static void DownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Load(e.Result, false);
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        internal static void Load(string content, bool isFile)
        {
            sses.Clear();
            vmesses.Clear();

            string result = Base64.Decode(content);
            string[] nodeList = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in nodeList)
            {
                if (item.StartsWith("ssr://"))
                {
                    ShadowSocksNode(item);
                }
                else if (item.StartsWith("vmess://"))
                {
                    VmessNode(item);
                }
            }

            if (!isFile)
            {
                Configuration.Config.Raw = content;
                Configuration.Config.Upgrade = DateTime.Now.Ticks;
                Configuration.Save();
                Complete(DateTime.Now.Ticks);
            }
            else
            {
                Complete(0);
            }
        }

        private static void ShadowSocksNode(string url)
        {
            string nodeBase = url.Replace("ssr://", "");
            string node = Base64.Decode(nodeBase);
            string server = node.Split(':')[0];
            int port = Convert.ToInt32(node.Split(':')[1]);
            string passwordBase = node.Split(':')[5].Split('?')[0].Replace("/", "");
            string password = Base64.Decode(passwordBase);
            string method = node.Split(':')[3];
            string remarkBase = node.Split(':')[5].Split('&')[2].Replace("remarks=", "");
            string remark = Base64.Decode(remarkBase);

            if (server.Contains("账户状态"))
            {
                userInfo = $"{server}：{remark}";
            }
            else
            {
                var ss = new ShadowSocks
                {
                    Name = remark,
                    Address = server,
                    Port = port,
                    Password = password,
                    Method = method
                };
                sses.Add(ss);
            }
        }

        private static void VmessNode(string url)
        {
            // TODO: vmess
        }

        internal static event CompleteDelegate CompleteEvent;
        internal delegate void CompleteDelegate(long tick);
        internal static void Complete(long tick) => CompleteEvent?.Invoke(tick);

        internal static string GetName(string address)
        {
            string name = string.Empty;
            if (!string.IsNullOrWhiteSpace(address))
            {
                var temp1 = sses.Where(x => x.Address.Equals(address, StringComparison.OrdinalIgnoreCase)).Select(y => y.Name);
                var temp2 = vmesses.Where(x => x.Address.Equals(address, StringComparison.OrdinalIgnoreCase)).Select(y => y.Name);
                if (temp1.Count() != 0)
                {
                    name = temp1.First();
                }
                else if (temp2.Count() != 0)
                {
                    name = temp2.First();
                }
            }
            return name;
        }

        internal static object GetNode(string name)
        {
            object address = null;
            var temp1 = sses.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            var temp2 = vmesses.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (temp1.Count() != 0)
            {
                address = temp1.First();
            }
            else if (temp2.Count() != 0)
            {
                address = temp2.First();
            }
            return address;
        }
    }
}
