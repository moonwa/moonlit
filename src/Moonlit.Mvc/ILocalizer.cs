using System;
using System.Data.Entity;
using System.Linq;

namespace Moonlit.Mvc
{
    public interface ILocalizer
    {
        string GetString(string key, string defaultValue, string lang);
    }
}