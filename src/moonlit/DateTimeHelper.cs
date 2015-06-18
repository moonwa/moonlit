using System;
using Moonlit.Properties;

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
        public static string GetFriendlyRelativeTime(this DateTime time)
        {
            var now = DateTime.Now;
            var timespan = now - time;
            if (timespan.TotalMinutes < 5)
            {
                return string.Format(CultureTextResources.FriendlyTimeInMunites, Math.Floor(timespan.TotalMinutes));
            }
            if (timespan.TotalHours < 3)
            {
                return string.Format(CultureTextResources.FriendlyTimeInHours, Math.Floor(timespan.TotalHours));
            }
            if (timespan.TotalDays < 4)
            {
                return string.Format(CultureTextResources.FriendlyTimeInDays, Math.Floor(timespan.TotalDays));
            }
            return string.Format(CultureTextResources.FriendlyTimeInMonths);
        }
    }
}