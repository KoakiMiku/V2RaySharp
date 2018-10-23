using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using V2RaySharp.Model;
using V2RaySharp.Properties;

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
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = program;
                process.Start();
                SystemProxy.Enable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Stop()
        {
            try
            {
                SystemProxy.Disable();
                Process[] processes = Process.GetProcessesByName("wv2ray");
                foreach (var item in processes)
                {
                    item.Kill();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Restart()
        {
            try
            {
                Stop();
                Thread.Sleep(1000);
                Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsRunning()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName("wv2ray");
                if (processes.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ChangeNode(string name)
        {
            try
            {
                CheckConfig();
                object node = Node.GetNode(name);
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
            catch (Exception)
            {
                throw;
            }
        }

        private static void ChangeSS(ShadowSocks ss)
        {
            try
            {
                JObject jObject = ReadConfig();
                jObject["outbound"]["protocol"] = "shadowsocks";
                jObject["outbound"]["settings"]["servers"][0]["address"] = ss.Address;
                jObject["outbound"]["settings"]["servers"][0]["port"] = ss.Port;
                jObject["outbound"]["settings"]["servers"][0]["password"] = ss.Password;
                jObject["outbound"]["settings"]["servers"][0]["method"] = ss.Method;
                jObject["outbound"]["settings"]["vnext"][0]["address"] = null;
                jObject["outbound"]["settings"]["vnext"][0]["port"] = null;
                jObject["outbound"]["settings"]["vnext"][0]["users"][0]["id"] = null;
                jObject["outbound"]["settings"]["vnext"][0]["users"][0]["alterId"] = null;
                WriteConfig(jObject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ChangeVmess(Vmess vmess)
        {
            try
            {
                JObject jObject = ReadConfig();
                jObject["outbound"]["protocol"] = "vmess";
                jObject["outbound"]["settings"]["vnext"][0]["address"] = vmess.Address;
                jObject["outbound"]["settings"]["vnext"][0]["port"] = vmess.Port;
                jObject["outbound"]["settings"]["vnext"][0]["users"][0]["id"] = vmess.ID;
                jObject["outbound"]["settings"]["vnext"][0]["users"][0]["alterId"] = vmess.AlterID;
                jObject["outbound"]["settings"]["servers"][0]["address"] = null;
                jObject["outbound"]["settings"]["servers"][0]["port"] = null;
                jObject["outbound"]["settings"]["servers"][0]["password"] = null;
                jObject["outbound"]["settings"]["servers"][0]["method"] = null;
                WriteConfig(jObject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ChangeRoute(string name)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsUsingRoute()
        {
            try
            {
                JObject jObject = ReadConfig();
                JToken jToken = jObject["routing"];
                if (jToken != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SelectNode()
        {
            try
            {
                CheckConfig();
                JObject jObject = ReadConfig();
                string protocol = jObject["outbound"]["protocol"].ToString();
                string address = string.Empty;
                if (protocol == "shadowsocks")
                {
                    address = jObject["outbound"]["settings"]["servers"][0]["address"].ToString();
                }
                else if (protocol == "vmess")
                {
                    address = jObject["outbound"]["settings"]["vnext"][0]["address"].ToString();
                }
                string name = Node.GetName(address);
                return name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void EditConfig()
        {
            try
            {
                CheckConfig();
                Process process = new Process();
                process.StartInfo.FileName = config;
                process.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CheckConfig()
        {
            try
            {
                if (!File.Exists(config))
                {
                    File.WriteAllText(config, jsonGlobal);
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
            catch (Exception)
            {
                throw;
            }
        }

        private static JObject ReadConfig()
        {
            try
            {
                string json = File.ReadAllText(config);
                JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                return jObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void WriteConfig(JObject jObject)
        {
            try
            {
                string json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                File.WriteAllText(config, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
