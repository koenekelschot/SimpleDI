using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ConfigurationRootTest
    {
        [TestMethod]
        public void ConfigurationRoot_GetSectionTest()
        {
            var root = new TestableConfigurationRoot<SimpleTestOptions>(new SimpleTestOptions("default"));

            var section = root.GetSection<SimpleTestOptions>("");
            Assert.IsNotNull(section);
            Assert.IsInstanceOfType(section.Value, typeof(SimpleTestOptions));
            Assert.AreEqual("default", section.Value.TestProperty);
        }

        [TestMethod]
        public void ConfigurationRoot_GetSection_UnknownTest()
        {
            var root = new TestableConfigurationRoot<SimpleTestOptions>(new SimpleTestOptions("default"));

            var section = root.GetSection<SimpleTestOptions>("unknown_path");
            Assert.IsNotNull(section);
            Assert.IsInstanceOfType(section.Value, typeof(SimpleTestOptions));
            Assert.IsNull(section.Value.TestProperty);
        }

        [TestMethod]
        public void ConfigurationRoot_GetTest()
        {
            var root = new TestableConfigurationRoot<ComplexTestOptions>(new ComplexTestOptions());

            Assert.IsInstanceOfType(root.Get<string>("TestProperty"), typeof(string));
            Assert.IsInstanceOfType(root.Get<SimpleTestOptions>("TestOptions"), typeof(SimpleTestOptions));
        }

        [TestMethod]
        public void ConfigurationRoot_Get_WithDefaultsTest()
        {
            var root = new TestableConfigurationRoot<ComplexTestOptions>(new ComplexTestOptions());
            var resultString1 = root.Get<string>("TestProperty");
            var resultString2 = root.Get<string>("TestProperty2", "applied default value");

            Assert.IsInstanceOfType(resultString1, typeof(string));
            Assert.AreEqual("default value", resultString1);
            Assert.IsInstanceOfType(resultString2, typeof(string));
            Assert.AreEqual("applied default value", resultString2);
        }

        [TestMethod]
        public void ConfigurationRoot_Get_UnknownPropertyTest()
        {
            var root = new TestableConfigurationRoot<ComplexTestOptions>(new ComplexTestOptions());

            Assert.IsNull(root.Get<string>("TestProperty2"));
        }
    }
}
