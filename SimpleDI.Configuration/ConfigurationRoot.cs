using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace SimpleDI.Configuration
{
    public class ConfigurationRoot : IConfigurationRoot, IInternalConfigurationRoot
    {
        private readonly JObject _jsonRoot;
        private readonly JsonMergeSettings _mergeSettings;

        protected internal ConfigurationRoot() {
            _jsonRoot = new JObject();
            _mergeSettings = new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                MergeNullValueHandling = MergeNullValueHandling.Ignore
            };
        }

        public IConfigurationSection<ConfigType> GetSection<ConfigType>(string sectionName) where ConfigType : class, new()
        {
            try
            {
                sectionName = sectionName.Replace(':', '.').Trim('.');
                var result = GetObject<ConfigType>(sectionName);
                return new ConfigurationSection<ConfigType>(result, this, sectionName);
            }
            catch(JsonReaderException)
            {
                return new ConfigurationSection<ConfigType>(new ConfigType(), this, sectionName);
            }
        }

        public ConfigType Get<ConfigType>(string propertyPath) where ConfigType : class
        {
            return Get<ConfigType>(propertyPath, default(ConfigType));
        }

        public ConfigType Get<ConfigType>(string propertyPath, ConfigType defaultValue) where ConfigType : class
        {
            try
            {
                propertyPath = propertyPath.Replace(':', '.').Trim('.');
                var value = GetValue<ConfigType>(propertyPath);
                if (value == null || value == default(ConfigType))
                {
                    return defaultValue;
                }
                return value;
            }
            catch (JsonReaderException)
            {
                return defaultValue;
            }
        }

        void IInternalConfigurationRoot.AddJsonFromStream(MemoryStream memoryStream)
        {
            using (var streamReader = new StreamReader(memoryStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                try
                {
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonReader);
                    _jsonRoot.Merge(jsonObject, _mergeSettings);
                }
                catch (JsonReaderException jre)
                {
                    throw new FormatException("Error reading json from config.", jre);
                }
            }
        }

        protected virtual ConfigType GetValue<ConfigType>(string path) where ConfigType : class
        {
            return _jsonRoot.SelectToken(path)?.Value<ConfigType>();
        }

        protected virtual ConfigType GetObject<ConfigType>(string path) where ConfigType : class
        {
            return _jsonRoot.SelectToken(path).ToObject<ConfigType>();
        }
    }
}
