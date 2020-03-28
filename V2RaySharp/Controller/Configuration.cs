using System;
using System.IO;
using Newtonsoft.Json;
using V2RaySharp.Model;

namespace V2RaySharp.Controller
{
    class Configuration
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string config = Path.Combine(path, "V2RaySharp.json");

        public static Config Config { get; set; }

        public static void Load()
        {
            if (!File.Exists(config))
            {
                Config = new Config
                {
                    Subscription = string.Empty,
                    RawData = string.Empty,
                    UpgradeTime = 0
                };
                Save();
            }
            else
            {
                var json = File.ReadAllText(config);
                Config = JsonConvert.DeserializeObject<Config>(json);
                Node.LoadVmess(Config.RawData, true);
            }
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(config, json);
        }
    }
}
