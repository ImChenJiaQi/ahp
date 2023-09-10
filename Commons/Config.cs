using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AHPTest.Commons
{
    internal class Config
    {
        public static string ConfigFile { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "config.AHP.json");

        public static T GetValue<T>(string key)
        {
            try
            {
                string file = ConfigFile;
                if (!File.Exists(file)) return default;
                string content = File.ReadAllText(file);
                if (string.IsNullOrEmpty(content)) return default;
                Dictionary<string, JsonElement> dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content);
                if (dict == null || !dict.ContainsKey(key)) return default;
                JsonElement element = dict[key];
                return JsonSerializer.Deserialize<T>(element);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }
        public static void SetValue<T>(string key, T values)
        {
            try
            {
                string file = ConfigFile;
                Dictionary<string, object> dict = null;
                if (File.Exists(file))
                {
                    dict = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(file));
                }
                if (dict == null) dict = new Dictionary<string, object>();
                if (dict.ContainsKey(key)) dict[key] = values;
                else dict.Add(key, values);
                File.WriteAllText(file, JsonSerializer.Serialize(dict));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                File.Delete(ConfigFile);
            }
        }
    }
}
