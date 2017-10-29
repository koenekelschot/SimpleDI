namespace SimpleDI.Configuration
{
    public interface IServiceCollection : SimpleDI.IServiceCollection
    {
        SimpleDI.IServiceCollection Configure<ConfigType>(IConfigurationSection<ConfigType> configuration) where ConfigType : class, new();
    }
}
