using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SimpleDI.Configuration.Test
{
    [TestClass]
    public class ConfigurationBuilderTest
    {
        [TestMethod]
        public void ConfigurationBuilder_AddJsonFileTest()
        {
            var builder = new ConfigurationBuilder();
            var result = builder.AddJsonFile("config.json");

            Assert.IsInstanceOfType(result, typeof(IConfigurationBuilder));
        }

        [TestMethod]
        public void ConfigurationBuilder_AddJsonFile_OptionalTest()
        {
            var builer = new ConfigurationBuilder()
                .AddJsonFile("config.json", true);

            Assert.That.ThrowsNoException<FileNotFoundException>(() => builer.Build());
        }

        [TestMethod]
        public void ConfigurationBuilder_AddJsonFile_RequiredTest()
        {
            var builer = new ConfigurationBuilder()
                .AddJsonFile("config.json", false);

            Assert.ThrowsException<FileNotFoundException>(() => builer.Build());
        }

        [TestMethod]
        public void ConfigurationBuilder_AddJsonFile_InvalidFileNameTest()
        {
            var builder1 = new ConfigurationBuilder();
            var builder2 = new ConfigurationBuilder();
            var builder3 = new ConfigurationBuilder();

            Assert.That.ThrowsNoException<ArgumentException>(
                () => builder1.AddJsonFile("../config.json").Build()
            );
            Assert.ThrowsException<ArgumentException>(
                () => builder2.AddJsonFile("../<config.json").Build()
            );
            Assert.ThrowsException<ArgumentException>(
                () => builder3.AddJsonFile(".<./config.json").Build()
            );
        }

        [TestMethod]
        public void ConfigurationBuilder_AddJsonFile_DuplicateFileTest()
        {
            var builder = new ConfigurationBuilder();

            Assert.That.ThrowsNoException<ArgumentException>(() =>
                builder.AddJsonFile("config.json")
                    .AddJsonFile("config.json")
                    .AddJsonFile("./config.json")
                    .Build()
            );
        }

        [TestMethod]
        public void ConfigurationBuilder_SetBasePathTest()
        {
            var builder = new ConfigurationBuilder();
            Assert.That.ThrowsNoException<ArgumentException>(() => 
                builder.SetBasePath(".")
                    .AddJsonFile("config.json")
                    .Build()
            );
        }

        [TestMethod]
        public void ConfigurationBuilder_SetBasePath_RootedTest()
        {
            var builder = new ConfigurationBuilder();
            Assert.That.ThrowsNoException<ArgumentException>(() => 
                builder.SetBasePath("C:\\")
                    .AddJsonFile("config.json")
                    .Build()
            );
        }

        [TestMethod]
        public void ConfigurationBuilder_SetBasePath_InvalidPathTest()
        {
            var builder = new ConfigurationBuilder();
            Assert.ThrowsException<ArgumentException>(() => 
                builder.SetBasePath("$invalid*")
                    .AddJsonFile("config.json")
                    .Build()
            );
        }

        [TestMethod]
        public void ConfigurationBuilder_BuildTest()
        {
            var configRoot = new TestableConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            var test1 = configRoot.GetSection<JsonTestClass1>("Test1");
            Assert.IsNotNull(test1);
            Assert.IsInstanceOfType(test1.Value, typeof(JsonTestClass1));
            Assert.AreEqual("test1 - setting1", test1.Value.Setting1);
            Assert.AreEqual("test1 - setting2", test1.Value.Setting2);
        }

        [TestMethod]
        public void ConfigurationBuilder_Build_MissingPropertyTest()
        {
            var configRoot = new TestableConfigurationBuilder()
                .AddJsonFile("missing.json")
                .Build();

            var test1 = configRoot.GetSection<JsonTestClass1>("Test1");
            Assert.IsNotNull(test1);
            Assert.IsInstanceOfType(test1.Value, typeof(JsonTestClass1));
            Assert.AreEqual("test1 - setting1", test1.Value.Setting1);
            Assert.IsNull(test1.Value.Setting2);
        }

        [TestMethod]
        public void ConfigurationBuilder_Build_OverwrittenPropertyTest()
        {
            var configRoot = new TestableConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("overwrite.json")
                .Build();

            var test1 = configRoot.GetSection<JsonTestClass1>("Test1");
            Assert.IsNotNull(test1);
            Assert.IsInstanceOfType(test1.Value, typeof(JsonTestClass1));
            Assert.AreEqual("test1 - setting1 overwritten", test1.Value.Setting1);
            Assert.AreEqual("test1 - setting2", test1.Value.Setting2);
        }
    }
}
