using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
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
                if (!string.IsNullOrWhiteSpace(Configuration.Config.SsSub))
                {
                    var subscription = Configuration.Config.SsSub;
                    if (!Configuration.Config.SsSub.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        && !Configuration.Config.SsSub.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        subscription = $"http://{Configuration.Config.SsSub}";
                    }
                    var http = new WebClient();
                    http.DownloadStringCompleted += DownloadShadowSocksComplete;
                    http.DownloadStringAsync(new Uri(subscription));
                }

                if (!string.IsNullOrWhiteSpace(Configuration.Config.VmessSub))
                {
                    var subscription = Configuration.Config.VmessSub;
                    if (!Configuration.Config.SsSub.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        && !Configuration.Config.SsSub.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        subscription = $"http://{Configuration.Config.VmessSub}";
                    }
                    var http = new WebClient();
                    http.DownloadStringCompleted += DownloadVmessComplete;
                    http.DownloadStringAsync(new Uri(subscription));
                }

                if (string.IsNullOrWhiteSpace(Configuration.Config.SsSub)
                    && string.IsNullOrWhiteSpace(Configuration.Config.VmessSub))
                {
                    Complete(-2);
                }
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        private static void DownloadShadowSocksComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                LoadShadowSocks(e.Result, false);
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        private static void DownloadVmessComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                LoadVmess(e.Result, false);
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        internal static void LoadShadowSocks(string content, bool isFile)
        {
            sses.Clear();

            var result = Base64.Decode(content);
            var nodeList = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in nodeList)
            {
                if (item.StartsWith("ssr://"))
                {
                    ShadowSocksNode(item);
                }
            }

            if (!isFile)
            {
                Configuration.Config.SsRaw = content;
                Configuration.Config.Upgrade = DateTime.Now.Ticks;
                Configuration.Save();
                Complete(DateTime.Now.Ticks);
            }
            else
            {
                Complete(0);
            }
        }

        internal static void LoadVmess(string content, bool isFile)
        {
            vmesses.Clear();

            var result = Base64.Decode(content);
            var nodeList = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in nodeList)
            {
                if (item.StartsWith("vmess://"))
                {
                    VmessNode(item);
                }
            }

            if (!isFile)
            {
                Configuration.Config.VmessRaw = content;
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
            var nodeBase = url.Replace("ssr://", "");
            var node = Base64.Decode(nodeBase);
            var server = node.Split(':')[0];
            var port = Convert.ToInt32(node.Split(':')[1]);
            var passwordBase = node.Split(':')[5].Split('?')[0].Replace("/", "");
            var password = Base64.Decode(passwordBase);
            var method = node.Split(':')[3];
            var remarkBase = node.Split(':')[5].Split('&')[2].Replace("remarks=", "");
            var remark = Base64.Decode(remarkBase);

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
            var nodeBase = url.Replace("vmess://", "");
            var node = Base64.Decode(nodeBase);
            var json = JObject.Parse(node);
            var version = json["v"].ToString();

            if (version != "2")
            {
                return;
            }

            var server = json["host"].ToString();
            var port = Convert.ToInt32(json["port"]);
            var alterId = Convert.ToInt32(json["aid"]);
            var id = json["id"].ToString();
            var network = json["net"].ToString();
            var security = json["tls"].ToString();
            var path = json["path"].ToString();
            var remark = json["ps"].ToString();

            if (server.Contains("账户状态"))
            {
                userInfo = $"{server}：{remark}";
            }
            else
            {
                var v = new Vmess
                {
                    Name = remark,
                    Address = server,
                    Port = port,
                    AlterID = alterId,
                    ID = id,
                    Network = network,
                    Security = security,
                    Path = path
                };
                vmesses.Add(v);
            }
        }

        internal static event CompleteDelegate CompleteEvent;
        internal delegate void CompleteDelegate(long tick);
        internal static void Complete(long tick) => CompleteEvent?.Invoke(tick);

        internal static string GetName(string address)
        {
            var name = string.Empty;
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
