using Newtonsoft.Json;
using System;
using System.IO;
using V2RaySharp.Model;

namespace V2RaySharp.Controller
{
    class Configuration
    {
        public static Config Config { get; set; }

        public static void Load()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "V2RaySharp.json");
                if (!File.Exists(path))
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
                    string json = File.ReadAllText(path);
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
                string path = Path.Combine(AppContext.BaseDirectory, "V2RaySharp.json");
                string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
