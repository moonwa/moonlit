using System;
using System.Collections.Generic;
using Moonlit.Collections;

namespace Moonlit
{
    [Serializable]
    public class PropertyError
    {
        private string _propertyName;
        private readonly IList<string> _errors;

        public PropertyError(string propertyName, params string[] errors)
        {
            _propertyName = propertyName;
            _errors = new List<string>(errors ?? new string[0]);
        }

        public string PropertyName
        {
            get { return _propertyName; }
            internal set { _propertyName = value; }
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        public void Add(string errorText)
        {
            _errors.Add(errorText);
        }
    }
}