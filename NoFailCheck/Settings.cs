using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFailCheck
{
    public class Settings
    {
        public bool Enabled { get; set; } = true;
        public bool DoublePress { get; set; } = true;
        public bool ChangeText { get; set; } = true;

        [JsonIgnore]
        private static readonly string Filename = Path.Combine(Plugin.DataPath, "NoFailCheck.json");

        [JsonIgnore]
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), Formatting = Formatting.Indented };

        public void Set(string name, object value)
        {
            object obj = this;
            var props = obj.GetType().GetProperties();

            // check if the property exists
            if (!props.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))) return;

            // get the property to update
            var prop = props.First(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            // set the property
            prop.SetValue(this, value);

            // save
            Save();
        }

        public static Settings Load()
        {
            var _settings = new Settings();
            if (File.Exists(Filename))
            {
                var data = File.ReadAllText(Filename);
                _settings = JsonConvert.DeserializeObject<Settings>(data);
            }
            else
            {
                _settings.Save();
            }
            return _settings;
        }

        public void Save()
        {
            var data = JsonConvert.SerializeObject(this, serializerSettings);
            File.WriteAllText(Filename, data);
        }
    }
}
