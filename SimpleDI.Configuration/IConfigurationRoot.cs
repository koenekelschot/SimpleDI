using System.IO;

namespace SimpleDI.Configuration
{
    public interface IConfigurationRoot
    {
        IConfigurationSection<ConfigType> GetSection<ConfigType>(string sectionName) where ConfigType : class, new();
        ConfigType Get<ConfigType>(string propertyPath);
        ConfigType Get<ConfigType>(string propertyPath, ConfigType defaultValue);
    }

    internal interface IInternalConfigurationRoot
    {
        void AddJsonFromStream(MemoryStream memoryStream);
    }
}
