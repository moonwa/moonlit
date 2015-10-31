using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Arithmetic
{
    public abstract class SortTest<T> where T : Moonlit.Arithmetic.ISort
    {
        protected abstract T CreateT();
        [TestMethod]
        public void TestSort()
        {
            T sort = this.CreateT();
            Dictionary<int[], int[]> sortMap = new Dictionary<int[], int[]>()
            {
                { new int[]{ 2, 4, 1 }, new int[] { 1, 2, 4 } },
                { new int[]{ 4, 2, 1 }, new int[] { 1, 2, 4 } },
                { new int[]{ 1, 2, 4 }, new int[] { 1, 2, 4 } },
                { new int[]{ 1, -2, 4 }, new int[] { -2, 1, 4 } },
                { new int[]{ 1, 1, 1 }, new int[] { 1, 1, 1 } }
            };
            foreach (var kp in sortMap)
            {
                List<int> sortData = new List<int>(kp.Key);
                var sorted = sort.Sort<int>(sortData, 0, kp.Key.Length - 1);
                string message = string.Format("排序 {0} 失败:, 预期: {1}, 实际 {2}", CombineArray(kp.Key), CombineArray(kp.Value), CombineArray(sorted.ToArray()));
                Assert.AreEqual(sorted.Count, kp.Key.Length);
                for (int i = 0; i < sorted.Count; i++)
                {
                    Assert.AreEqual(kp.Value[i], sorted[i], message);
                }
            }
        }
        private string CombineArray(int[] arr)
        {
            List<string> ss = new List<string>();
            foreach (var i in arr)
            {
                ss.Add(i.ToString());
            }
            return string.Join(",", ss.ToArray());
        }
    }
}
