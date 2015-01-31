using System;
using System.CodeDom.Compiler;
using System.Text;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace Moonlit.VisualStudio
{
    public static class ServiceStringHelper
    {
        public static string ToCamel(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            StringBuilder builder = new StringBuilder();
            bool isWordFirst = true;
            foreach (var c in text)
            {
                if (c == '.' || c == '_')
                {
                    isWordFirst = true;
                    builder.Append(c);
                    continue;
                }
                if (isWordFirst)
                {
                    builder.Append(Char.ToUpper(c));
                    isWordFirst = false;
                    continue;
                }
                builder.Append(c);
            }
            return builder.ToString();
        }
        public static string GetPropertyName(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            text = text.Replace("_", "__").Replace(".", "_");
            return ToCamel(text);
        }
        public static string GetType(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return char.ToUpper(text[0]).ToString() + text.Substring(1);
        }
    }
    public static class ServiceProviderHelper
    {
        public static CodeDomProvider GetCodeDomProvider(this IServiceProvider serviceProvider)
        {
            IVSMDCodeDomProvider provider = serviceProvider.GetService(typeof(SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;
            if (provider != null)
            {
                return provider.CodeDomProvider as CodeDomProvider;
            }
            else
            {
                //In the case where no language specific CodeDom is available, fall back to C#
                return CodeDomProvider.CreateProvider("C#");
            }
        }
    }
}