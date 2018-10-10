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
        public static void Start()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = Path.Combine(AppContext.BaseDirectory, "wv2ray.exe");
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

        public static void Change(string name)
        {
            try
            {
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

        public static string Select()
        {
            try
            {
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
                string path = Path.Combine(AppContext.BaseDirectory, "config.json");
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.Start();
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
                string path = Path.Combine(AppContext.BaseDirectory, "config.json");
                string json = string.Empty;
                if (File.Exists(path))
                {
                    json = File.ReadAllText(path);
                }
                else
                {
                    json = Encoding.UTF8.GetString(Resources.config);
                }
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
                string path = Path.Combine(AppContext.BaseDirectory, "config.json");
                string json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
