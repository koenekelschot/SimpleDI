using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ServiceCollectionTest
    {
        [TestMethod]
        public void ServiceCollection_Configure_ExtensionTest()
        {
            var provider = new ServiceCollection()
                .Configure(new TestableConfigurationSection<BasicConfigTestClass>(new BasicConfigTestClass()));

            Assert.IsInstanceOfType(provider, typeof(IServiceCollection));
            Assert.IsInstanceOfType(provider, typeof(SimpleDI.IServiceCollection));
        }

        [TestMethod]
        public void ServiceCollection_ConfigureTest()
        {
            var provider = new ServiceCollection()
                .Configure(new TestableConfigurationSection<BasicConfigTestClass>(new BasicConfigTestClass()))
                .BuildServiceProvider();

            var config = provider.GetService<IOptions<BasicConfigTestClass>>();

            Assert.IsNotNull(config);
            Assert.IsInstanceOfType(config, typeof(IOptions<BasicConfigTestClass>));
        }

        private class BasicConfigTestClass { }
    }
}
