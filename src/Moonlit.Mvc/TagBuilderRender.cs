using System;
using System.IO;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
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