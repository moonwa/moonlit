using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Diagnostics
{
    /// <summary>
    /// 验证功能，实现简单的失败判断
    /// </summary>
    public static class Validator
    { 
        public static void Validate<T>(bool condition, string throwIfFalse)
            where T : Exception
        {
            if (!condition)
                throw (Exception)System.Activator.CreateInstance(typeof(T), throwIfFalse);
        }
        public static void Validate(bool condition, string throwIfFalse)
        {
            Validate<Exception>(condition, throwIfFalse);
        }
        /// <summary>
        /// Validates the specified condition.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="exThrowIfFalse">The ex throw if false.</param>
        public static void Validate(bool condition, Exception exThrowIfFalse)
        {
            if (!condition)
            {
                throw exThrowIfFalse;
            }
        }
    }
}
