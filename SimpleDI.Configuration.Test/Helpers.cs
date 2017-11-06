using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.IO;
using System.Text;

namespace SimpleDI.Configuration.Test
{
    public static class Helpers
    {
        public static void ThrowsNoException<T>(this Assert assert, Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                Assert.Fail();
            }
            catch (AssertFailedException) { }
            catch (Exception) { }
        }
    }

    public class TestableConfigurationSection<ConfigType> : ConfigurationSection<ConfigType> where ConfigType : class, new()
    {
        public TestableConfigurationSection(ConfigType value) : 
            base(value, new TestableConfigurationRoot<ConfigType>(value), string.Empty) { }
    }

    public class TestableConfigurationRoot<ConfigType> : ConfigurationRoot where ConfigType : class, new()
    {
        private readonly ConfigType _config;
        public TestableConfigurationRoot(ConfigType config) {
            _config = config;
        }

        protected override SubType GetObject<SubType>(string path)
        {
            if (!"".Equals(path))
            {
                throw new JsonReaderException("Mock failure from test");
            }
            return _config as SubType;
        }

        protected override SubType GetValue<SubType>(string path)
        {
            TypeInfo valueTypeInfo = typeof(ConfigType).GetTypeInfo();
            foreach (PropertyInfo property in valueTypeInfo.DeclaredProperties)
            {
                if (property.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    return (SubType)property.GetValue(_config);
                }
            }
            return default(SubType);
        }
    }

    public class TestableConfigurationBuilder : ConfigurationBuilder
    {
        protected override void ReadFile(string filePath, bool optional)
        {
            var contents = string.Empty;
            if (filePath.EndsWith("config.json"))
            {
                contents = "{\"Test1\":{\"setting1\":\"test1 - setting1\",\"setting2\":\"test1 - setting2\"},\"Test2\":{\"setting1\":\"test2 - setting1\"}}";
            }
            else if (filePath.EndsWith("overwrite.json"))
            {
                contents = "{\"Test1\":{\"setting1\":\"test1 - setting1 overwritten\"}}";
            }
            else if (filePath.EndsWith("missing.json"))
            {
                contents = "{\"Test1\":{\"setting1\":\"test1 - setting1\"},\"Test2\":{\"setting1\":\"test2 - setting1\"}}";
            }

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(contents)))
            {
                AddFileContents(memoryStream);
            }
        }
    }

    public class SimpleTestOptions
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

    public class ComplexTestOptions
    {
        public string TestProperty { get; set; } = "default value";
        public int TestValueType { get; set; } = 42;
        public SimpleTestOptions TestOptions { get; set; } = new SimpleTestOptions("test");
    }

    public class JsonTestClass1
    {
        public string Setting1 { get; set; }
        public string Setting2 { get; set; }
    }

    public class JsonTestClass2
    {
        public string Setting1 { get; set; }
    }
}
