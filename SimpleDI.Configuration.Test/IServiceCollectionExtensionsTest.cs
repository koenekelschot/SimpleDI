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

        private class BasicConfigTestClass {
            public string Test { get; set; }
        }
    }
}
