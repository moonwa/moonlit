using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit
{
    [Serializable]
    public class PropertyErrors : IEnumerable<PropertyError>
    {
        private readonly IList<PropertyError> _errors = new List<PropertyError>();
        public IEnumerator<PropertyError> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }
        public void ChangeProperty(string oldName, string newName, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            foreach (var error in this)
            {
                if (string.Equals(error.PropertyName, oldName, stringComparison))
                {
                    error.PropertyName = newName;
                    break;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PropertyError propertyError)
        {
            if (propertyError == null) return;
            var error = _errors.FirstOrDefault(x => string.Equals(x.PropertyName, propertyError.PropertyName, StringComparison.OrdinalIgnoreCase));
            if (error == null)
                _errors.Add(propertyError);
            else
            {
                foreach (var errorText in propertyError.Errors)
                {
                    error.Add(errorText);
                }
            }
        }
    }
}