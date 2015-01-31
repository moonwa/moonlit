using Microsoft.VisualStudio.TestTools.UnitTesting;

    #region TestFrameworkUsing
#if !NUNIT

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif
#endregion
namespace Moonlit.TestFixtures.Arithmetic
{
    [TestClass]
    public class Fibonacci_Test
    {
        [TestMethod]
        public void Fib()
        {
            Moonlit.Arithmetic.Fibonacci fib = new Moonlit.Arithmetic.Fibonacci();
            int[] cases = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 };
            for (int i = 0; i < cases.Length; i++)
            {
                Assert.AreEqual(cases[i], fib.Fib(i), string.Format("Fail : exp = {0}, act = {1}, value = {2}", i, fib.Fib(i), cases[i]));
            }
        }
    }
}
