using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ConfigurationSectionTest
    {
        private string testString = "Some boring test string";

        [TestMethod]
        public void ConfigurationSection_TypeTest()
        {
            var testOptions = new TestableConfigurationSection<TypeTestOptions>(new TypeTestOptions());

            Assert.IsNotNull(testOptions);
            Assert.IsInstanceOfType(testOptions, typeof(IOptions<TypeTestOptions>));
            Assert.IsNotNull(testOptions.Value);
            Assert.IsInstanceOfType(testOptions.Value, typeof(TypeTestOptions));
        }

        [TestMethod]
        public void ConfigurationSection_NullValueTest()
        {
            var testOptions = new TestableConfigurationSection<NullValueTestOptions>(new NullValueTestOptions());

            Assert.IsNull(testOptions.Value.Test);
            Assert.IsNull(testOptions["test"]);
        }

        [TestMethod]
        public void ConfigurationSection_SimpleTest()
        {
            var testOptions = new TestableConfigurationSection<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNotNull(testOptions.Value.TestProperty);
            Assert.AreEqual(testString, testOptions.Value.TestProperty);
        }

        [TestMethod]
        public void ConfigurationSection_UnknownPropertyTest()
        {
            var testOptions = new TestableConfigurationSection<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNull(testOptions["UnknownProperty"]);
        }

        [TestMethod]
        public void ConfigurationSection_GetPropertyWithCasingTest()
        {
            var testOptions = new TestableConfigurationSection<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.AreEqual(testString, testOptions["TestProperty"]);
            Assert.AreEqual(testString, testOptions["testproperty"]);
            Assert.AreEqual(testString, testOptions["TeStPrOpErTy"]);
        }

        [TestMethod]
        public void ConfigurationSection_FieldTest()
        {
            var testOptions = new TestableConfigurationSection<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNotNull(testOptions.Value.TestField);
            Assert.IsNull(testOptions["TestField"]);
        }

        [TestMethod]
        public void ConfigurationSection_GetTest()
        {
            var testOptions = new TestableConfigurationSection<ComplexTestOptions>(new ComplexTestOptions());

            Assert.IsInstanceOfType(testOptions.Get<string>("TestProperty"), typeof(string));
            Assert.IsInstanceOfType(testOptions.Get<SimpleTestOptions>("TestOptions"), typeof(SimpleTestOptions));
        }

        [TestMethod]
        public void ConfigurationSection_Get_UnknownPropertyTest()
        {
            var testOptions = new TestableConfigurationSection<ComplexTestOptions>(new ComplexTestOptions());

            Assert.IsNull(testOptions.Get<string>("TestProperty2"));
        }

        [TestMethod]
        public void ConfigurationSection_Get_WithDefaultsTest()
        {
            var testOptions = new TestableConfigurationSection<ComplexTestOptions>(new ComplexTestOptions());
            var resultString1 = testOptions.Get<string>("TestProperty");
            var resultString2 = testOptions.Get<string>("TestProperty2", "applied default value");

            Assert.IsInstanceOfType(resultString1, typeof(string));
            Assert.AreEqual("default value", resultString1);
            Assert.IsInstanceOfType(resultString2, typeof(string));
            Assert.AreEqual("applied default value", resultString2);
        }

        private class TypeTestOptions { }

        private class NullValueTestOptions
        {
            public string Test { get; set; } = null;
        }
    }
}
