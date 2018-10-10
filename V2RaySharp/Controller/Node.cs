using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using V2RaySharp.Model;

namespace V2RaySharp.Controller
{
    class Node
    {
        public static string userInfo = string.Empty;
        public static List<ShadowSocks> sses = new List<ShadowSocks>();
        public static List<Vmess> vmesses = new List<Vmess>();

        public static void Upgrade()
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
                    WebClient http = new WebClient();
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

        public static void Load(string content, bool isFile)
        {
            try
            {
                sses.Clear();
                vmesses.Clear();

                string result = Base64.Decode(content);
                string[] nodeList = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in nodeList)
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
            catch (Exception)
            {
                throw;
            }
        }

        private static void ShadowSocksNode(string url)
        {
            try
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
                    ShadowSocks ss = new ShadowSocks
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
            catch (Exception)
            {
                throw;
            }
        }

        private static void VmessNode(string url)
        {
            try
            {
                // TODO: vmess
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static event CompleteDelegate CompleteEvent;
        public delegate void CompleteDelegate(long tick);
        public static void Complete(long tick)
        {
            try
            {
                CompleteEvent?.Invoke(tick);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetName(string address)
        {
            try
            {
                string name = string.Empty;
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
                return name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object GetNode(string name)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
