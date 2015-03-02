using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Mvc
{
    public class RequestMappingAttribute : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public RequestMappingAttribute()
        {

        }
        public RequestMappingAttribute(string name)
        {
            Name = name;
        }

        public RequestMappingAttribute(string name, string url)
        {
            Name = name;
            Url = url;
        }

    }
}
