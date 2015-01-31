using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit
{
    /// <summary>
    /// 包装无法分类的单元操作
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// 交换指定类型的数据
        /// </summary>
        /// <typeparam name="T">交换类型</typeparam>
        /// <param name="left">左值</param>
        /// <param name="right">大中型值</param>
        public static void Swap<T>(ref T left, ref T right)
        {
            T tmp = left;
            left = right;
            right = tmp;
        }
    }
}
