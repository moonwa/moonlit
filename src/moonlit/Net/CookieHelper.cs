using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Moonlit.Text;

namespace Moonlit.Net
{
    public static class CookieHelper
    {
        public static IEnumerable<Cookie> GetCookies(string text)
        {
            var cookies = new List<Cookie>();

            if (string.IsNullOrWhiteSpace(text))
            {
                return cookies;
            }

            Cookie cookie = new Cookie();
            foreach (var phase in text.Split(';'))
            {
                WordEnumerator wordEnumerator = new WordEnumerator(phase, new[] { '=', ' ' });
                string name = null;
                string value = null;
                foreach (var word in wordEnumerator)
                {
                    if (name == null)
                    {
                        name = word;
                        wordEnumerator.RemoveSpliter('=');
                    }
                    else
                    {
                        value = word;
                    }
                }
                switch (name.ToLower())
                {
                    case "path":
                        cookie.Path = value;
                        break;
                    case "domain":
                        cookie.Domain = value;
                        break;
                    default:
                        if (value != null && value.Contains(","))
                            value = value.Split(',')[0];
                        cookie = new Cookie(name, value);
                        cookies.Add(cookie);
                        break;

                }
            }
             
            return cookies;
        }
    }
}
