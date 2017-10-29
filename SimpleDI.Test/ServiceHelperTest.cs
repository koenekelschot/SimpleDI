using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleDI.Test
{
    [TestClass]
    public class ServiceHelperTest
    {
        [TestMethod]
        public void ServiceHelper_ThrowIfNoInterfaceTest()
        {
            Assert.That.ThrowsNoException<NotSupportedException>(() => ServiceHelper.ThrowIfNoInterface<ISimpleTestInterface>());
            Assert.ThrowsException<NotSupportedException>(() => ServiceHelper.ThrowIfNoInterface<SimpleTestClass1>());
        }

        [TestMethod]
        public void ServiceHelper_ThrowIfInterfaceTest()
        {
            Assert.That.ThrowsNoException<NotSupportedException>(() => ServiceHelper.ThrowIfInterface<SimpleTestClass1>());
            Assert.ThrowsException<NotSupportedException>(() => ServiceHelper.ThrowIfInterface<ISimpleTestInterface>());
        }

        [TestMethod]
        public void ServiceHelper_IsInterfaceTest()
        {
            Assert.IsFalse(ServiceHelper.IsInterface<SimpleTestClass1>());
            Assert.IsTrue(ServiceHelper.IsInterface<ISimpleTestInterface>());
        }

        [TestMethod]
        public void ServiceHelper_ThrowIfCanBeTreatedAsTypeTest()
        {
            Assert.ThrowsException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SimpleTestClass1), typeof(ISimpleTestInterface))
            );
            Assert.ThrowsException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(ISimpleTestInterface), typeof(SimpleTestClass1))
            );

            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SimpleTestClass1), typeof(SimpleTestClass2))
            );
            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SimpleTestClass2), typeof(SimpleTestClass1))
            );

            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SimpleTestClass1), typeof(SomeOtherTestClass))
            );
            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SomeOtherTestClass), typeof(SimpleTestClass1))
            );

            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(ISimpleTestInterface), typeof(SomeOtherTestClass))
            );
            Assert.That.ThrowsNoException<NotSupportedException>(
                () => ServiceHelper.ThrowIfCanBeTreatedAsType(typeof(SomeOtherTestClass), typeof(ISimpleTestInterface))
            );
        }

        [TestMethod]
        public void ServiceHelper_CanBeTreatedAsTypeTest()
        {
            Assert.IsTrue(ServiceHelper.CanBeTreatedAsType(typeof(SimpleTestClass1), typeof(ISimpleTestInterface)));
            Assert.IsTrue(ServiceHelper.CanBeTreatedAsType(typeof(ISimpleTestInterface), typeof(SimpleTestClass1)));

            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(SimpleTestClass1), typeof(SimpleTestClass2)));
            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(SimpleTestClass2), typeof(SimpleTestClass1)));

            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(SimpleTestClass1), typeof(SomeOtherTestClass)));
            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(SomeOtherTestClass), typeof(SimpleTestClass1)));

            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(ISimpleTestInterface), typeof(SomeOtherTestClass)));
            Assert.IsFalse(ServiceHelper.CanBeTreatedAsType(typeof(SomeOtherTestClass), typeof(ISimpleTestInterface)));
        }
    }
}
