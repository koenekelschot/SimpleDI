using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleDI.Test
{
    [TestClass]
    public class OptionsTest
    {
        private string testString = "Some boring test string";

        [TestMethod]
        public void Options_TypeTest()
        {
            var testOptions = new TestableOptions<TypeTestOptions>(new TypeTestOptions());

            Assert.IsNotNull(testOptions);
            Assert.IsInstanceOfType(testOptions, typeof(IOptions<TypeTestOptions>));
            Assert.IsNotNull(testOptions.Value);
            Assert.IsInstanceOfType(testOptions.Value, typeof(TypeTestOptions));
        }

        [TestMethod]
        public void Options_NullValueTest()
        {
            var testOptions = new TestableOptions<NullValueTestOptions>(new NullValueTestOptions());

            Assert.IsNull(testOptions.Value.Test);
            Assert.IsNull(testOptions["test"]);
        }

        [TestMethod]
        public void Options_SimpleTest()
        {
            var testOptions = new TestableOptions<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNotNull(testOptions.Value.TestProperty);
            Assert.AreEqual(testString, testOptions.Value.TestProperty);
        }

        [TestMethod]
        public void Options_UnknownPropertyTest()
        {
            var testOptions = new TestableOptions<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNull(testOptions["UnknownProperty"]);
        }

        [TestMethod]
        public void Options_GetPropertyWithCasingTest()
        {
            var testOptions = new TestableOptions<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.AreEqual(testString, testOptions["TestProperty"]);
            Assert.AreEqual(testString, testOptions["testproperty"]);
            Assert.AreEqual(testString, testOptions["TeStPrOpErTy"]);
        }
        
        [TestMethod]
        public void Options_FieldTest()
        {
            var testOptions = new TestableOptions<SimpleTestOptions>(new SimpleTestOptions(testString));

            Assert.IsNotNull(testOptions.Value.TestField);
            Assert.IsNull(testOptions["TestField"]);
        }

        private class TypeTestOptions { }

        private class NullValueTestOptions
        {
            public string Test { get; set; } = null;
        }

        private class SimpleTestOptions
        {
            public SimpleTestOptions() { }

            public SimpleTestOptions(string input)
            {
                TestProperty = input;
                TestField = input;
            }

            public string TestProperty { get; set; }

            public string TestField;
        }
    }
}
