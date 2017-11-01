namespace SimpleDI.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection Configure<ConfigType>(this IServiceCollection services, IConfigurationSection<ConfigType> configuration) 
            where ConfigType : class, new()
        {
            return services.Configure<ConfigType>(configuration);
        }
    }
}
