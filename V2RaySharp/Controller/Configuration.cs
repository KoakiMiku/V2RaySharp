using Newtonsoft.Json;
using System;
using System.IO;
using V2RaySharp.Model;

namespace V2RaySharp.Controller
{
    internal class Configuration
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string config = Path.Combine(path, "V2RaySharp.json");

        internal static Config Config { get; set; }

        internal static void Load()
        {
            if (!File.Exists(config))
            {
                Config = new Config
                {
                    SsSub = string.Empty,
                    VmessSub = string.Empty,
                    SsRaw = string.Empty,
                    VmessRaw = string.Empty,
                    Upgrade = 0
                };
                Save();
            }
            else
            {
                var json = File.ReadAllText(config);
                Config = JsonConvert.DeserializeObject<Config>(json);
                Node.LoadShadowSocks(Config.SsRaw, true);
                Node.LoadVmess(Config.VmessRaw, true);
            }
        }

        internal static void Save()
        {
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(config, json);
        }
    }
}
