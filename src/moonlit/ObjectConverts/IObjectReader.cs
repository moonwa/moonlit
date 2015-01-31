using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.ObjectConverts
{ 
    public interface IObjectReader
    {
        object GetValue(string propertyName );
        object Value { get; }
        IEnumerable<string> Properties { get; }
        bool TryGetValue(string propertyName, out object obj);
    }
}