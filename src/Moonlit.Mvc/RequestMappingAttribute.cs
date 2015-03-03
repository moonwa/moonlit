using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

    public class TagBuilderRender : IDisposable
    {
        private readonly TagBuilder _tagBuilder;
        private readonly TextWriter _textWriter;

        public TagBuilderRender(TagBuilder tagBuilder, TextWriter textWriter)
        {
            _tagBuilder = tagBuilder;
            _textWriter = textWriter;
            textWriter.Write(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            _textWriter.Write(_tagBuilder.ToString(TagRenderMode.EndTag));
        }
    }
     
}
