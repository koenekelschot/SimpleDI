namespace SimpleDI.Configuration
{
    public class ConfigurationSection<ConfigType> : Options<ConfigType>, IConfigurationRoot,
        IConfigurationSection<ConfigType> where ConfigType : class, new()
    {
        private readonly IConfigurationRoot _root;
        private readonly string _path;

        protected internal ConfigurationSection(ConfigType value, IConfigurationRoot root, string path) : base(value) {
            _root = root;
            _path = path;
        }

        public ConfigSubType Get<ConfigSubType>(string propertyPath)
        {
            return Get<ConfigSubType>(propertyPath, default(ConfigSubType));
        }

        public ConfigSubType Get<ConfigSubType>(string propertyPath, ConfigSubType defaultValue)
        {
            string fullPath = string.Join(":", _path, propertyPath);
            return _root.Get<ConfigSubType>(fullPath, defaultValue);
        }

        public IConfigurationSection<ConfigSubType> GetSection<ConfigSubType>(string sectionName) where ConfigSubType : class, new()
        {
            string fullPath = string.Join(":", _path, sectionName);
            return _root.GetSection<ConfigSubType>(fullPath);
        }
    }
}
