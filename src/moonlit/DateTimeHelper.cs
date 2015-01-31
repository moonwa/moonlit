using System;

namespace Moonlit
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class DateTimeHelper
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="birthDate"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        public static bool IsBirthday(this DateTime birthDate, DateTime today)
        {
            return (birthDate.Month == today.Month && birthDate.Day == today.Day);
        }
    }
}