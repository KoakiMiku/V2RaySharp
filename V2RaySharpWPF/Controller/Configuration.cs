using Newtonsoft.Json;
using System;
using System.IO;
using V2RaySharpWPF.Model;

namespace V2RaySharpWPF.Controller
{
    class Configuration
    {
        private static readonly string path = AppContext.BaseDirectory;
        private static readonly string config = Path.Combine(path, "V2RaySharp.json");

        public static Config Config { get; set; }

        public static void Load()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public static void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(config, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
