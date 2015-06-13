using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class MvcAttribute : Attribute
    {

        public string Module { get; set; }

    }

}