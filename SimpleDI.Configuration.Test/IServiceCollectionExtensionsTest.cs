using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ServiceCollectionTest
    {
        [TestMethod]
        public void IServiceCollectionExtensions_ConfigureTest()
        {
            var testConfig = new BasicConfigTestClass { Test = "test" };
            var services = new ServiceCollection()
                .Configure(new TestableConfigurationSection<BasicConfigTestClass>(testConfig));
            var provider = services.BuildServiceProvider();

            var config = provider.GetService<IOptions<BasicConfigTestClass>>();

            Assert.IsInstanceOfType(services, typeof(IServiceCollection));
            Assert.IsNotNull(config);
            Assert.IsInstanceOfType(config, typeof(IOptions<BasicConfigTestClass>));
            Assert.IsNotNull(config.Value.Test);
            Assert.AreEqual("test", config.Value.Test);
        }

        [TestMethod]
        public void IServiceCollectionExtensions_Configure_WithDependencyTest()
        {
            var testConfig = new BasicConfigTestClass { Test = "test" };
            var services = new ServiceCollection()
                .AddSingleton<IOptionsServiceTestClass>()
                .Configure(new TestableConfigurationSection<BasicConfigTestClass>(testConfig));
            var provider = services.BuildServiceProvider();

            var service = provider.GetService<IOptionsServiceTestClass>();

            Assert.IsInstanceOfType(service, typeof(IOptionsServiceTestClass));
            Assert.IsInstanceOfType(service.Settings, typeof(BasicConfigTestClass));
        }

        private class BasicConfigTestClass {
            public string Test { get; set; }
        }

        private class IOptionsServiceTestClass
        {
            public readonly BasicConfigTestClass Settings;
            public IOptionsServiceTestClass(IOptions<BasicConfigTestClass> settings)
            {
                Settings = settings.Value;
            }
        }
    }
}
