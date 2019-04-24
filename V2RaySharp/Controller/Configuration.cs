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
                    Subscription = string.Empty,
                    Raw = string.Empty,
                    Upgrade = 0
                };
                Save();
            }
            else
            {
                string json = File.ReadAllText(config);
                Config = JsonConvert.DeserializeObject<Config>(json);
                Node.Load(Config.Raw, true);
            }
        }

        internal static void Save()
        {
            string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(config, json);
        }
    }
}
