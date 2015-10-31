using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Arithmetic;

#region TestFrameworkUsing
 

#endregion

namespace Moonlit.TestFixtures.Arithmetic
{
    [TestClass]
    public class FibonacciTests
    {
        [TestMethod]
        public void Fib()
        {
            var fib = new Fibonacci();
            int[] cases = {0, 1, 1, 2, 3, 5, 8, 13, 21, 34};
            for (var i = 0; i < cases.Length; i++)
            {
                Assert.AreEqual(cases[i], fib.Fib(i),
                    string.Format("Fail : exp = {0}, act = {1}, value = {2}", i, fib.Fib(i), cases[i]));
            }
        }
    }
}