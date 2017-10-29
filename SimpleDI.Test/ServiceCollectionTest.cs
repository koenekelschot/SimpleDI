using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleDI.Test
{
    [TestClass]
    public class ServiceCollectionTest
    {
        [TestMethod]
        public void ServiceCollection_AddSingletonTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();

            Assert.IsInstanceOfType(instance1, typeof(ISimpleTestInterface));
            Assert.IsInstanceOfType(instance1, typeof(SimpleTestClass1));
        }

        [TestMethod]
        public void ServiceCollection_AddSingleton_WithInterfaceTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<ISimpleTestInterface, SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();

            Assert.IsInstanceOfType(instance1, typeof(ISimpleTestInterface));
            Assert.IsInstanceOfType(instance1, typeof(SimpleTestClass1));
        }

        [TestMethod]
        public void ServiceCollection_AddSingleton_WithInstanceTest()
        {
            var singleton = new Singleton()
            {
                FavoriteDrink = "Beer"
            };

            var provider = new ServiceCollection()
                .AddSingleton<Singleton>(singleton)
                .BuildServiceProvider();

            var instance = provider.GetService<Singleton>();

            Assert.AreNotEqual("Coffee", instance.FavoriteDrink);
            Assert.AreEqual("Beer", instance.FavoriteDrink);
        }

        [TestMethod]
        public void ServiceCollection_AddTransientTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();

            Assert.IsInstanceOfType(instance1, typeof(ISimpleTestInterface));
            Assert.IsInstanceOfType(instance1, typeof(SimpleTestClass1));
        }

        [TestMethod]
        public void ServiceCollection_AddTransient_WithInterfaceTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<ISimpleTestInterface, SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();

            Assert.IsInstanceOfType(instance1, typeof(ISimpleTestInterface));
            Assert.IsInstanceOfType(instance1, typeof(SimpleTestClass1));
        }

        [TestMethod]
        public void ServiceCollection_CheckInterfaceAlreadyRegisteredTest()
        {
            var services = new ServiceCollection();
            Assert.That.ThrowsNoException<NotSupportedException>(
                () => services.AddSingleton<ISimpleTestInterface, SimpleTestClass1>()
            );
            Assert.ThrowsException<NotSupportedException>(
                () => services.AddSingleton<ISimpleTestInterface, SimpleTestClass2>()
            );
        }

        [TestMethod]
        public void ServiceCollection_BuildServiceProviderTest()
        {
            var provider = new ServiceCollection().BuildServiceProvider();

            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(IServiceProvider));
        }

        private class Singleton
        {
            public string FavoriteDrink { get; set; } = "Coffee";
        }
    }
}
