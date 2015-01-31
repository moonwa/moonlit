using System;

namespace Moonlit.Arithmetic
{
    /// <summary>
    /// 数字帮助类 
    /// </summary>
    public static class DigitExtensions
    {
        /// <summary>
        /// Determines whether the specified value is zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is zero; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZero(this decimal value)
        {
            return Math.Abs(value) < 0.0001M;
        }
    }
}