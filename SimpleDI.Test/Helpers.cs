using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleDI.Test
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

    public class TestableOptions<TOptions> : Options<TOptions> where TOptions : class, new()
    {
        public TestableOptions(TOptions value) : base(value) { }
    }

    public interface ISimpleTestInterface
    {
        long SixByNine();
    }

    public class SimpleTestClass1 : ISimpleTestInterface
    {
        public long SixByNine()
        {
            return 42;
        }
    }

    public class SimpleTestClass2 : ISimpleTestInterface
    {
        public long SixByNine()
        {
            return 54;
        }
    }

    public class SomeOtherTestClass { }
}
