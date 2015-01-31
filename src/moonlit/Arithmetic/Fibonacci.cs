using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Arithmetic
{
    /// <summary>
    /// Fibonacci 计算
    /// </summary>
    public class Fibonacci
    {
        /// <summary>
        /// 计算第N位 Fibonacci 数列
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public long Fib(long n)
        {
            if (n == 0) return 0;
            if (n <= 2) return 1;
            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
