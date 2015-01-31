using System;

namespace Moonlit.ObjectConverts
{
    [System.Diagnostics.DebuggerStepThrough]
    public class ConvertArgs
    {
        public ConvertArgs(IObjectReader reader, ObjectConverter objectConverter, Type destinationType)
        {
            Reader = reader;
            this.Converter = objectConverter;
            DestinationType = destinationType;
        }

        public object ConvertedObject { get; set; }
        public Type DestinationType { get; set; }
        public IObjectReader Reader { get; set; }
        public ObjectConverter Converter { get; set; }
    }
}