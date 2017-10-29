using System.IO;

namespace SimpleDI.Configuration
{
    public interface IConfigurationRoot
    {
        IConfigurationSection<ConfigType> GetSection<ConfigType>(string sectionName) where ConfigType : class, new();
        ConfigType Get<ConfigType>(string propertyPath) where ConfigType : class;
        ConfigType Get<ConfigType>(string propertyPath, ConfigType defaultValue) where ConfigType : class;
    }

    internal interface IInternalConfigurationRoot
    {
        void AddJsonFromStream(MemoryStream memoryStream);
    }
}
