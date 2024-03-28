using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Paragoniarz.Domain.Settings
{
    public class ConfigurationService : IConfigurationService
    {
        public Configuration LoadConfiguration()
        {
            Configuration? configuration = null;
            if (File.Exists(ConfigPath))
            {
                string config = File.ReadAllText(ConfigPath);
                if (!string.IsNullOrEmpty(config))
                    configuration = JsonConvert.DeserializeObject<Configuration>(config, GetSerializerSettings())!;
            }

            if (configuration == null)
                configuration = Configuration.CreateDefault();

            return configuration;
        }

        public void SaveConfiguration(Configuration configuration)
        {
            string json = JsonConvert.SerializeObject(configuration, GetSerializerSettings());
            File.WriteAllText(ConfigPath, json);
        }

        private JsonSerializerSettings GetSerializerSettings() => new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private string ConfigPath => $"{Configuration.GetProgramDirectory()}/config.json";
    }
}
