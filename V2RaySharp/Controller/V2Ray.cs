using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using V2RaySharp.Model;
using V2RaySharp.Properties;
using V2RaySharp.Utiliy;

namespace V2RaySharp.Controller
{
    class V2Ray
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string program = Path.Combine(path, "wv2ray.exe");
        private static readonly string config = Path.Combine(path, "config.json");
        private static readonly string configGlobal = Path.Combine(path, "config.global.json");
        private static readonly string configRoute = Path.Combine(path, "config.route.json");
        private static readonly string jsonGlobal = Encoding.UTF8.GetString(Resources.Global);
        private static readonly string jsonRoute = Encoding.UTF8.GetString(Resources.Route);

        public static void Start()
        {
            var process = new Process();
            process.StartInfo.FileName = program;
            process.Start();
            SystemProxy.Enable();
        }

        public static void Stop()
        {
            SystemProxy.Disable();
            var processes = Process.GetProcessesByName("wv2ray");
            foreach (var item in processes)
            {
                item.Kill();
            }
        }

        private static void Restart()
        {
            Stop();
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            Start();
        }

        public static bool IsRunning()
        {
            var processes = Process.GetProcessesByName("wv2ray");
            return processes.Length != 0;
        }

        public static void ChangeNode(string name)
        {
            CheckConfig();
            var vmess = Node.GetNode(name);
            ChangeVmess(vmess);
            Restart();
        }

        private static void ChangeVmess(Vmess vmess)
        {
            var jObject = ReadConfig();
            var jArray = jObject["outbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                if (item["tag"].ToString() == "proxy")
                {
                    item["settings"]["vnext"][0]["address"] = vmess.Address;
                    item["settings"]["vnext"][0]["port"] = vmess.Port;
                    item["settings"]["vnext"][0]["users"][0]["id"] = vmess.ID;
                    item["settings"]["vnext"][0]["users"][0]["alterId"] = vmess.AlterID;
                    item["streamSettings"]["network"] = vmess.Network;
                    item["streamSettings"]["security"] = vmess.Security;
                    item["streamSettings"]["tlsSettings"]["serverName"] = vmess.Address;
                    item["streamSettings"]["wsSettings"]["path"] = vmess.Path;
                    item["streamSettings"]["wsSettings"]["headers"]["Host"] = vmess.Address;
                }
                break;
            }
            jObject["outbounds"] = jArray;
            WriteConfig(jObject);
        }

        public static void ChangeRoute(string name)
        {
            CheckConfig();
            if (IsUsingRoute())
            {
                File.Copy(config, configRoute, true);
                File.Copy(configGlobal, config, true);
            }
            else
            {
                File.Copy(config, configGlobal, true);
                File.Copy(configRoute, config, true);
            }
            ChangeNode(name);
        }

        public static void ChangeListen(string name)
        {
            CheckConfig();
            if (IsListenHostOnly())
            {
                ChangeAllowAny();
            }
            else
            {
                ChangeHostOnly();
            }
            ChangeNode(name);
        }

        private static void ChangeHostOnly()
        {
            var jObject = ReadConfig();
            var jArray = jObject["inbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                item["listen"] = "127.0.0.1";
            }
            jObject["inbounds"] = jArray;
            WriteConfig(jObject);
        }

        private static void ChangeAllowAny()
        {
            var jObject = ReadConfig();
            var jArray = jObject["inbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                item["listen"] = "0.0.0.0";
            }
            jObject["inbounds"] = jArray;
            WriteConfig(jObject);
        }

        public static bool IsUsingRoute()
        {
            CheckConfig();
            var jObject = ReadConfig();
            var jToken = jObject["routing"];
            return jToken != null;
        }

        public static bool IsListenHostOnly()
        {
            CheckConfig();
            var jObject = ReadConfig();
            var listen = jObject["inbounds"][0]["listen"].ToString();
            return listen == "127.0.0.1";
        }

        public static string SelectNode()
        {
            CheckConfig();
            var jObject = ReadConfig();
            var jArray = jObject["outbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                if (item["tag"].ToString() == "proxy")
                {
                    var address = item["settings"]["vnext"][0]["address"].ToString();
                    return Node.GetName(address);
                }
            }
            return null;
        }

        private static void CheckConfig()
        {
            if (!File.Exists(config))
            {
                File.WriteAllText(config, jsonRoute);
            }
            if (!File.Exists(configGlobal))
            {
                File.WriteAllText(configGlobal, jsonGlobal);
            }
            if (!File.Exists(configRoute))
            {
                File.WriteAllText(configRoute, jsonRoute);
            }
        }

        private static JObject ReadConfig()
        {
            var json = File.ReadAllText(config);
            return JsonConvert.DeserializeObject<JObject>(json);
        }

        private static void WriteConfig(JObject jObject)
        {
            var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            File.WriteAllText(config, json);
        }
    }
}
