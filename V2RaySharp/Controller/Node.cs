using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using V2RaySharp.Model;
using V2RaySharp.Utiliy;

namespace V2RaySharp.Controller
{
    public class Node
    {
        public static string userInfo = string.Empty;
        public static List<Vmess> vmesses = new List<Vmess>();

        public static void Upgrade()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Configuration.Config.Subscription))
                {
                    var subscription = Configuration.Config.Subscription;
                    if (!Configuration.Config.Subscription.StartsWith("http://",
                            StringComparison.OrdinalIgnoreCase)
                        && !Configuration.Config.Subscription.StartsWith("https://",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        subscription = $"http://{Configuration.Config.Subscription}";
                    }
                    var http = new WebClient();
                    http.DownloadStringCompleted += DownloadVmessComplete;
                    http.DownloadStringAsync(new Uri(subscription));
                }

                if (string.IsNullOrWhiteSpace(Configuration.Config.Subscription))
                {
                    Complete(-2);
                }
            }
            catch (Exception)
            {
                Complete(-1);
            }
        }

        private static void DownloadVmessComplete(
            object sender, DownloadStringCompletedEventArgs e)
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

        public static void LoadVmess(string content, bool isFile)
        {
            vmesses.Clear();

            var result = Base64.Decode(content);
            var nodeList = result.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in nodeList)
            {
                if (item.StartsWith("vmess://"))
                {
                    VmessNode(item);
                }
            }

            if (!isFile)
            {
                Configuration.Config.RawData = content;
                Configuration.Config.UpgradeTime = DateTime.Now.Ticks;
                Configuration.Save();
                Complete(DateTime.Now.Ticks);
            }
            else
            {
                Complete(0);
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

        public static event CompleteDelegate CompleteEvent;
        public delegate void CompleteDelegate(long tick);
        public static void Complete(long tick) => CompleteEvent?.Invoke(tick);

        public static string GetName(string address)
        {
            return vmesses
                .Where(x => x.Address == address)
                .Select(y => y.Name)
                .FirstOrDefault();
        }

        public static Vmess GetNode(string name)
        {
            return vmesses
                .Where(x => x.Name == name)
                .FirstOrDefault();
        }
    }
}
