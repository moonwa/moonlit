using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Collections;

namespace Moonlit.TestFixtures.Collections
{
    [TestClass()]
    public class DimensionIteratorTests
    {
        [TestMethod()]
        public void NoIteratorTest()
        {
            var iterator = new MatrixBuilder<int, int>(new[] { Enumerable.Range(0, 3) }, (int target, int[] locals, ref bool continuation) =>
            {
                continuation = false;
                return new[] { target };
            });
            var actual = iterator.SelectMany(x => x).ToArray();
            CollectionAssert.AreEquivalent(new[] { 0, 1, 2 }, actual);
        }
        [TestMethod()]
        public void NoIteratorExtensionsTest()
        {
            var actual = new[] {Enumerable.Range(0, 3)}.Matrix((int target, int[] locals, ref bool continuation) =>
            {
                continuation = false;
                return new[] {target};
            }).SelectMany(x => x).ToArray();
            CollectionAssert.AreEquivalent(new[] { 0, 1, 2 }, actual);
        }


        [TestMethod()]
        public void NoDimension2Test()
        {
            Console.WriteLine();
            char[] arr1 = new[] { 'a', 'b' };
            char[] arr2 = new[] { '1', '2', '3' };
            var iterator = new MatrixBuilder<char, char>(new[] { arr1, arr2 },
                (char target, char[] locals, ref bool continuation) =>
                {
                    continuation = false;
                    return arr2;
                });

            var actual = iterator.Select(x => string.Join("", x)).ToArray();
            Assert.AreEqual(6, actual.Length);
        }
        [TestMethod()]
        public void NoDimensionCalcSumTest()
        {
            var array = new[] { 1, 2 };
            var iterator = new MatrixBuilder<int, int>(array.Repeat(),
                (int target, int[] locals, ref bool continuation) =>
                {
                    var localSum = locals.Sum();
                    if (localSum + target == 3)
                    {
                        continuation = false;
                        return new[] { target };
                    }
                    if (localSum + target > 3)
                    {
                        continuation = false;
                        return new int[0];
                    }

                    return array;
                });

            var actual = iterator.Select(x => string.Join("+", x)).Distinct().ToArray();
            Assert.AreEqual(3, actual.Length);
            CollectionAssert.AreEquivalent(new[] { "1+1+1", "1+2", "2+1" }, actual);
        }
        [TestMethod()]
        public void AutoFinish()
        {
            int[][] array = new[]
            {
                new []{ 1, 2 },
                new []{ 3, 4,5 },
            };
            var iterator = new MatrixBuilder<int, int>(array,
                (int target, int[] locals, ref bool continuation) =>
                {
                    return new int[] { target };
                });

            var actual = iterator.Select(x => string.Join("+", x)).Distinct().ToArray();
            Assert.AreEqual(6, actual.Length);
        }
    }
}
