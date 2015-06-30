using System;
using System.Security.Cryptography;
using System.Web;

namespace Moonlit
{
    public static class UriBuilderHelper
    {
        public static UriBuilder AddQuery(this UriBuilder uriBuilder, string name, string value)
        {
            var currentQueryString = string.Format("{0}={1}", HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(value));
            var queryString = uriBuilder.Query;
            if (!string.IsNullOrEmpty(queryString))
                queryString = string.Format("{0}&{1}", queryString.Substring(1), currentQueryString);
            else
                queryString = currentQueryString;
            var newUriBuilder = new UriBuilder(uriBuilder.ToString());
            newUriBuilder.Query = queryString; // remove '?'
            return newUriBuilder;
        }
    } 

}
