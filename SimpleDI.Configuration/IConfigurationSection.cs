namespace SimpleDI.Configuration
{
    public interface IConfigurationSection<ConfigType> : IConfigurationRoot, IOptions<ConfigType> where ConfigType : class, new()
    {

    }
}
