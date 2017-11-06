using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleDI.Test
{
    [TestClass]
    public class ServiceProviderTest
    {
        [TestMethod]
        public void ServiceProvider_GetServiceTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(SimpleTestClass1));
        }

        [TestMethod]
        public void ServiceProvider_GetService_OfInterfaceTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<ISimpleTestInterface>();

            Assert.IsNotNull(instance1);
            Assert.IsInstanceOfType(instance1, typeof(ISimpleTestInterface));
        }

        [TestMethod]
        public void ServiceProvider_GetService_SingletonTest()
        {
            var provider = new ServiceCollection()
                .AddSingleton<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();
            var instance2 = provider.GetService<SimpleTestClass1>();

            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public void ServiceProvider_GetService_TransientTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<SimpleTestClass1>()
                .BuildServiceProvider();

            var instance1 = provider.GetService<SimpleTestClass1>();
            var instance2 = provider.GetService<SimpleTestClass1>();

            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void ServiceProvider_GetService_NotRegisteredTest()
        {
            var provider = new ServiceCollection()
                .BuildServiceProvider();

            Assert.ThrowsException<TypeLoadException>(
                () => provider.GetService<SimpleTestClass1>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_WithDependenciesTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<DependencyServiceTestClass>()
                .AddTransient<DefaultConstructorTestClass>()
                .BuildServiceProvider();

            DependencyServiceTestClass service = null;

            Assert.That.ThrowsNoException<TypeLoadException>(
                () => service = provider.GetService<DependencyServiceTestClass>()
            );
            Assert.IsNotNull(service);
            Assert.IsNotNull(service.Dependency);
            Assert.IsInstanceOfType(service.Dependency, typeof(DefaultConstructorTestClass));
        }

        [TestMethod]
        public void ServiceProvider_GetService_WithInterfaceParam()
        {
            var provider = new ServiceCollection()
                .AddTransient<IOptionsServiceTestClass>()
                .AddSingleton<Options<TestOptions>>(new Options<TestOptions>(new TestOptions()))
                .BuildServiceProvider();

            IOptionsServiceTestClass service = null;

            Assert.That.ThrowsNoException<TypeLoadException>(
                () => service = provider.GetService<IOptionsServiceTestClass>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_SimpleCircularReferenceTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<SimpleCircularRef1>()
                .AddTransient<SimpleCircularRef2>()
                .BuildServiceProvider();

            Assert.ThrowsException<TypeLoadException>(
                () => provider.GetService<SimpleCircularRef1>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_ComplexCircularReferenceTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<ComplexCircularRef1>()
                .AddTransient<ComplexCircularRef2>()
                .BuildServiceProvider();

            Assert.ThrowsException<TypeLoadException>(
                () => provider.GetService<ComplexCircularRef1>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_DefaultConstructorTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<DefaultConstructorTestClass>()
                .BuildServiceProvider();

            Assert.That.ThrowsNoException<TypeLoadException>(
                () => provider.GetService<DefaultConstructorTestClass>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_ConstructorWithOptionalParamsTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<ConstructorWithOptionalParamsTestClass>()
                .BuildServiceProvider();

            Assert.That.ThrowsNoException<TypeLoadException>(
                () => provider.GetService<ConstructorWithOptionalParamsTestClass>()
            );
        }

        [TestMethod]
        public void ServiceProvider_GetService_NonDefaultNonOptionalConstructorTest()
        {
            var provider = new ServiceCollection()
                .AddTransient<NonDefaultNonOptionalConstructorTestClass>()
                .BuildServiceProvider();

            Assert.ThrowsException<TypeLoadException>(
                () => provider.GetService<NonDefaultNonOptionalConstructorTestClass>()
            );
        }

        private class DependencyServiceTestClass
        {
            public readonly DefaultConstructorTestClass Dependency;
            public DependencyServiceTestClass(DefaultConstructorTestClass dependency)
            {
                Dependency = dependency;
            }
        }

        private class DefaultConstructorTestClass { }

        private class ConstructorWithOptionalParamsTestClass
        {
            public ConstructorWithOptionalParamsTestClass(bool arg1 = true, bool arg2 = false) { }
        }

        private class NonDefaultNonOptionalConstructorTestClass
        {
            public NonDefaultNonOptionalConstructorTestClass(bool arg1, bool arg2) { }
        }

        private class IOptionsServiceTestClass
        {
            private readonly TestOptions _settings;
            public IOptionsServiceTestClass(IOptions<TestOptions> settings)
            {
                _settings = settings.Value;
            }
        }

        private class TestOptions
        {
            public string Test { get; set; }
        }

        private class SimpleCircularRef1
        {
            private readonly SimpleCircularRef2 _circular;
            public SimpleCircularRef1(SimpleCircularRef2 circular)
            {
                _circular = circular;
            }
        }

        private class SimpleCircularRef2
        {
            private readonly SimpleCircularRef1 _circular;
            public SimpleCircularRef2(SimpleCircularRef1 circular)
            {
                _circular = circular;
            }
        }

        private class ComplexCircularRef1
        {
            private readonly ComplexCircularRef2 _circular;
            public ComplexCircularRef1(ComplexCircularRef2 circular)
            {
                _circular = circular;
            }
        }

        private class ComplexCircularRef2
        {
            private readonly ComplexCircularRef3 _circular;
            public ComplexCircularRef2(ComplexCircularRef3 circular)
            {
                _circular = circular;
            }
        }

        private class ComplexCircularRef3
        {
            private readonly ComplexCircularRef4 _circular;
            public ComplexCircularRef3(ComplexCircularRef4 circular)
            {
                _circular = circular;
            }
        }

        private class ComplexCircularRef4
        {
            private readonly ComplexCircularRef1 _circular;
            private readonly ComplexCircularRef2 _circular2;
            public ComplexCircularRef4(ComplexCircularRef1 circular, ComplexCircularRef2 circular2)
            {
                _circular = circular;
                _circular2 = circular2;
            }
        }
    }
}
