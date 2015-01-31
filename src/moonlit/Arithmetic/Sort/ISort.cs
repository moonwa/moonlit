using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Arithmetic
{
    /// <summary>
    /// 排序算法
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 排序算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szArray"></param>
        /// <param name="nLower"></param>
        /// <param name="nUpper"></param>
        /// <returns></returns>
        List<T> Sort<T>(List<T> szArray, int nLower, int nUpper) where T : IComparable<T>;
    }
}
