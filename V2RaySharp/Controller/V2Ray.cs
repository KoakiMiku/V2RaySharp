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
            var node = Node.GetNode(name);
            if (node is ShadowSocks ss)
            {
                ChangeSS(ss);
            }
            else if (node is Vmess vmess)
            {
                ChangeVmess(vmess);
            }
            Restart();
        }

        private static void ChangeSS(ShadowSocks ss)
        {
            var jObject = ReadConfig();
            var jArray = jObject["outbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                if (item["tag"].ToString() == "proxy")
                {
                    item["protocol"] = "shadowsocks";
                    var servers = new JArray() { new JObject()
                        {
                            { "address", ss.Address },
                            { "port", ss.Port },
                            { "password", ss.Password },
                            { "method", ss.Method }
                        }};
                    item["settings"]["servers"] = servers;
                    item["settings"]["vnext"] = null;
                    item["streamSettings"] = null;
                }
                break;
            }
            jObject["outbounds"] = jArray;
            WriteConfig(jObject);
        }

        private static void ChangeVmess(Vmess vmess)
        {
            var jObject = ReadConfig();
            var jArray = jObject["outbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                if (item["tag"].ToString() == "proxy")
                {
                    item["protocol"] = "vmess";
                    var vnext = new JArray()
                    {
                        new JObject()
                        {
                            { "address", vmess.Address },
                            { "port", vmess.Port },
                            { "users", new JArray()
                                {
                                    new JObject()
                                    {
                                        { "id", vmess.ID },
                                        { "alterId", vmess.AlterID }
                                    }
                                }
                            },
                        }
                    };
                    item["settings"]["vnext"] = vnext;
                    item["settings"]["servers"] = null;
                    var streamSettings = new JObject()
                    {
                        { "network", vmess.Network },
                        { "security", vmess.Security },
                        { "tlsSettings", new JObject()
                            {
                                { "allowInsecure", true },
                                { "serverName", vmess.Address }
                            }
                        },
                        { "wsSettings", new JObject()
                            {
                                { "path", vmess.Path },
                                { "headers", new JObject()
                                    {
                                        { "Host", vmess.Address }
                                    }
                                }
                            }
                        }
                    };
                    item["streamSettings"] = streamSettings;
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
            jObject["inbounds"][0]["listen"] = "127.0.0.1";
            WriteConfig(jObject);
        }

        private static void ChangeAllowAny()
        {
            var jObject = ReadConfig();
            jObject["inbounds"][0]["listen"] = "0.0.0.0";
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
            string name = null;
            CheckConfig();
            var jObject = ReadConfig();
            var jArray = jObject["outbounds"].ToObject<JArray>();
            foreach (var item in jArray)
            {
                if (item["tag"].ToString() == "proxy")
                {
                    var protocol = item["protocol"].ToString();
                    var address = string.Empty;
                    if (protocol == "shadowsocks")
                    {
                        address = item["settings"]["servers"][0]["address"].ToString();
                    }
                    else if (protocol == "vmess")
                    {
                        address = item["settings"]["vnext"][0]["address"].ToString();
                    }
                    name = Node.GetName(address);
                }
                break;
            }
            return name;
        }

        public static void EditConfig()
        {
            CheckConfig();
            var process = new Process();
            process.StartInfo.FileName = config;
            process.Start();
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
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            return jObject;
        }

        private static void WriteConfig(JObject jObject)
        {
            var json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            File.WriteAllText(config, json);
        }
    }
}
