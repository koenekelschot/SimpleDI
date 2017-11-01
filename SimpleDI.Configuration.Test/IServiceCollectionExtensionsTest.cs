using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ServiceCollectionTest
    {
        [TestMethod]
        public void IServiceCollectionExtensions_ConfigureTest()
        {
            var provider = new ServiceCollection()
                .Configure(new TestableConfigurationSection<BasicConfigTestClass>(new BasicConfigTestClass()))
                .BuildServiceProvider();

            var config = provider.GetService<IOptions<BasicConfigTestClass>>();

            Assert.IsInstanceOfType(provider, typeof(IServiceCollection));
            Assert.IsNotNull(config);
            Assert.IsInstanceOfType(config, typeof(IOptions<BasicConfigTestClass>));
        }

        private class BasicConfigTestClass { }
    }
}
