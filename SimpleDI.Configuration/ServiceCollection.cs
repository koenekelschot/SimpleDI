namespace SimpleDI.Configuration
{
    public class ServiceCollection : SimpleDI.ServiceCollection, IServiceCollection
    {
        public SimpleDI.IServiceCollection Configure<ConfigType>(IConfigurationSection<ConfigType> configuration) where ConfigType : class, new()
        {
            base.Configure<ConfigType>(configuration);
            return this;
        }
    }
}
