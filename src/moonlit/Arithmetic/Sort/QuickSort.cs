using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Arithmetic.Sort
{
    /// <summary>
    /// 快速排序
    /// </summary>
    public class QuickSort : ISort
    {
        // QuickSort implementation
        /// <summary>
        /// 排序算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szArray"></param>
        /// <param name="nLower"></param>
        /// <param name="nUpper"></param>
        /// <returns></returns>
        public List<T> Sort<T>(List<T> szArray, int nLower, int nUpper)
            where T : IComparable<T>
        {
            List<T> result = new List<T>(szArray);
            InnserSort(result, nLower, nUpper);
            return result;
        }

        /// <summary>
        /// Innsers the sort.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szArray">The sz array.</param>
        /// <param name="nLower">The n lower.</param>
        /// <param name="nUpper">The n upper.</param>
        private void InnserSort<T>(List<T> szArray, int nLower, int nUpper)
            where T : IComparable<T>
        {
            // Check for non-base case
            if (nLower < nUpper)
            {
                // Split and sort partitions
                int nSplit = Partition(szArray, nLower, nUpper);
                InnserSort(szArray, nLower, nSplit - 1);
                InnserSort(szArray, nSplit + 1, nUpper);
            }
        }
        /// <summary>
        /// Sorts the specified sz array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szArray">The sz array.</param>
        /// <returns></returns>
        public List<T> Sort<T>(List<T> szArray)
            where T : IComparable<T>
        {
            return this.Sort<T>(szArray, 0, szArray.Count - 1);
        }
        // QuickSort partition implementation
        private int Partition<T>(List<T> szArray, int nLower, int nUpper)
            where T : IComparable<T>
        {
            // Pivot with first element
            int nLeft = nLower + 1;
            T szPivot = szArray[nLower];
            int nRight = nUpper;

            // Partition array elements
            T szSwap;
            while (nLeft <= nRight)
            {
                // Find item out of place
                while (nLeft <= nRight && szArray[nLeft].CompareTo(szPivot) <= 0)
                    nLeft = nLeft + 1;
                while (nLeft <= nRight && szArray[nRight].CompareTo(szPivot) > 0)
                    nRight = nRight - 1;

                // Swap values if necessary
                if (nLeft < nRight)
                {
                    szSwap = szArray[nLeft];
                    szArray[nLeft] = szArray[nRight];
                    szArray[nRight] = szSwap;
                    nLeft = nLeft + 1;
                    nRight = nRight - 1;
                }
            }

            // Move pivot element
            szSwap = szArray[nLower];
            szArray[nLower] = szArray[nRight];
            szArray[nRight] = szSwap;
            return nRight;
        }
    }
}
