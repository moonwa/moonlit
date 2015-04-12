using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moonlit.Text;

namespace Moonlit
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Subs the string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static string Substring(this string source, string left, string right)
        {
            int posLeft = source.IndexOf(left);
            if (posLeft == -1)
            {
                posLeft = 0;
            }
            int posRight = source.IndexOf(right, posLeft + 1);
            if (posRight == -1)
            {
                posRight = source.Length;
            }
            return source.Substring(posLeft, posRight - posLeft);
        }

        /// <summary>
        /// hex string to bytes
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(this string source)
        {
            var key = new List<byte>();
            for (int i = 0; i < source.Length; i += 2)
            {
                key.Add(byte.Parse(source.Substring(i, 2), NumberStyles.HexNumber));
            }
            return key.ToArray();
        }

        /// <summary>
        /// split <paramref name="text"/> with <paramref name="splitWorld"/>
        /// split "HelloWorld" to "Hello World".
        /// split "RemovedRAF" to "Removed RAF".
        /// split "RAFParent" to "RAF Parent".
        /// split "WhatIsRAFParent" to "What Is RAF Parent".
        /// </summary>
        /// <returns></returns>
        public static string ToSentence(this string text, string splitWorld = " ")
        {
            if (string.IsNullOrEmpty(text)) return text;

            bool theBeforeIsUpper = false;
            List<char> chars = new List<char>();
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                var c = text[i];
                if (char.IsUpper(c))
                {
                    if (!theBeforeIsUpper)
                    {
                        if (i > 0)
                            chars.AddRange(splitWorld);
                    }
                    else
                    {
                        if (i < length - 1 && char.IsLower(text[i + 1]))
                            chars.AddRange(splitWorld);
                    }
                }
                theBeforeIsUpper = char.IsUpper(c);
                chars.Add(c);
            }
            return new string(chars.ToArray());
        }
        public static bool ContainsAnyCharIn(this string str, string array)
        {
            if (str == null) throw new NullReferenceException("This string to check");
            if (array == null) throw new ArgumentNullException("String containing the characters to validate");
            return str.ContainsAnyCharIn(array.ToCharArray());
        }
        public static string ToAbsoluteLocalPath(string url, string rootPath)
        {
            var uri = default(Uri);
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                return uri.LocalPath;

            if (Uri.TryCreate(Path.Combine(rootPath, url), UriKind.Absolute, out uri))
                return uri.LocalPath;
            return null;
        }
        public static bool ContainsAnyCharIn(this string str, params char[] array)
        {
            if (str == null) throw new NullReferenceException("This string to check");
            if (array == null) throw new ArgumentNullException("Array of characters to validate");
            if (array.Length == 0) throw new ArgumentNullException("Array of characters to validate cannot be empty.");

            foreach (char c in array) if (str.Contains(c.ToString())) return true;
            return false;
        }
        /// <summary>
        /// indicate is <paramref name="src"/> equals to <paramref name="dst"/>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="equalsEmptyWithNull"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string src, string dst, bool equalsEmptyWithNull = true)
        {
            if (equalsEmptyWithNull)
            {
                if (string.IsNullOrEmpty(src) && string.IsNullOrEmpty(dst))
                    return true;
            }
            return string.Equals(src, dst, StringComparison.OrdinalIgnoreCase);
        }
        public static bool StartsWithIgnoreCase(this string src, string dst, bool startsWithNull = false)
        {
            if (src == null)
            {
                return false;
            }
            if (dst == null)
                if (startsWithNull)
                    return true;
                else
                    return false;
            return src.StartsWith(dst, StringComparison.OrdinalIgnoreCase);
        }
        public static string DefaultIfNullOrEmpty(this string src, string defaultValue)
        {
            return string.IsNullOrEmpty(src) ? defaultValue : src;
        }
        public static string DefaultIfNullOrWhiteSpace(this string src, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(src) ? defaultValue : src;
        }
        public static byte[] ToByteArray(this string source, Encoding encoding)
        {
            return encoding.GetBytes(source);
        }
        public static string TrimSafty(this string source)
        {
            if (source == null)
                return string.Empty;
            return source.Trim();
        }
        public static string DefaultIsNullOrEmpty(this string source, string defaultValue)
        {
            if (string.IsNullOrEmpty(source))
                return defaultValue;
            return source;
        }

        public static string NullIfEmpty(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return s;
        }

        public static byte[] GetBytesAscii(this string s)
        {
            return GetBytes(s, Encoding.ASCII);
        }
        public static byte[] GetBytesUnicode(this string s)
        {
            return GetBytes(s, Encoding.Unicode);
        }
        public static byte[] GetBytesUtf8(this string s)
        {
            return GetBytes(s, Encoding.UTF8);
        }
        public static byte[] GetBytes(this string s, Encoding encoding)
        {
            return encoding.GetBytes(s);
        }

        public static string CleanHtmlTags(this string s)
        {
            Regex exp = new Regex(
                "<[^<>]*>",
                RegexOptions.Compiled
                );

            return exp.Replace(s, "");
        }


        public static string ForFormat(this string s)
        {
            return Regex.Replace(s.Replace("{", "{{").Replace("}", "}}"), @"{{(?<d>\d+)}}", "{${d}}");
        }
        public static string FromCamel(this string s, int minWordLength = 1, params string[] words)
        {
            return string.Join(" ", new Moonlit.Text.CamelEnumerator(s, minWordLength, words).ToArray());
        }
    }
}
