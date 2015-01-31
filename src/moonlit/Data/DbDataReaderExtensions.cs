using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Moonlit.Data
{
    public static class DbDataReaderExtensions
    {
        public static string GetString(this DbDataReader reader, string key, string defaultValue = "")
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var value = reader[key];
            if (value == null || value == DBNull.Value)
                return defaultValue;
            return value.ToString();
        }
        public static int GetInt32(this DbDataReader reader, string key, int defaultValue = 0)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var value = reader[key];
            if (value == null || value == DBNull.Value)
                return defaultValue;
            return Convert.ToInt32(value);
        }
        public static decimal GetDecimal(this DbDataReader reader, string key, decimal defaultValue = 0)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var value = reader[key];
            if (value == null || value == DBNull.Value)
                return defaultValue;
            return Convert.ToDecimal(value);
        }
        public static DateTime GetDateTime(this DbDataReader reader, string key, DateTime defaultValue = default(DateTime))
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var value = reader[key];
            if (value == null || value == DBNull.Value)
                return defaultValue;
            return Convert.ToDateTime(value);
        }


        public static object NullIfEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
                return DBNull.Value;
            return s;
        }

        public static int GetValue(this DbDataReader reader, string key, int defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToInt32(o);
        }

        public static bool GetValue(this DbDataReader reader, string key, bool defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToBoolean(o);
        }
        public static int? GetValue(this DbDataReader reader, string key, int? defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToInt32(o);
        }

        public static string GetValue(this DbDataReader reader, string key, string defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToString(o);
        }

        public static DateTime? GetValue(this DbDataReader reader, string key, DateTime? defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToDateTime(o);
        }
        public static DateTime GetValue(this DbDataReader reader, string key, DateTime defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToDateTime(o);
        }

        public static float GetValue(this DbDataReader reader, string key, float defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToSingle(o);
        }

        public static double GetValue(this DbDataReader reader, string key, double defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToDouble(o);
        }
        public static float? GetValue(this DbDataReader reader, string key, float? defaultValue)
        {
            var o = reader[key];
            if (o == DBNull.Value)
                return defaultValue;
            return Convert.ToSingle(o);
        }
    }
}
