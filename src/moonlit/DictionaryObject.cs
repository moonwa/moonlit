using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Moonlit.Objects
{
    public sealed class DictionaryObject : System.Dynamic.DynamicObject
    {
        private readonly IDictionary<string, object> _dictionary;

        public DictionaryObject(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            _dictionary = dictionary;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{");
            ToString(sb);
            return sb.ToString();
        }

        private void ToString(StringBuilder sb)
        {
            var firstInDictionary = true;
            foreach (var pair in _dictionary)
            {
                if (!firstInDictionary)
                    sb.Append(",");
                firstInDictionary = false;
                var value = pair.Value;
                var name = pair.Key;
                if (value is string)
                {
                    sb.AppendFormat("{0}:\"{1}\"", name, value);
                }
                else if (value is IDictionary<string, object>)
                {
                    new DictionaryObject((IDictionary<string, object>)value).ToString(sb);
                }
                else if (value is ArrayList)
                {
                    sb.Append(name + ":[");
                    var firstInArray = true;
                    foreach (var arrayValue in (ArrayList)value)
                    {
                        if (!firstInArray)
                            sb.Append(",");
                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                            new DictionaryObject((IDictionary<string, object>)arrayValue).ToString(sb);
                        else if (arrayValue is string)
                            sb.AppendFormat("\"{0}\"", arrayValue);
                        else
                            sb.AppendFormat("{0}", arrayValue);

                    }
                    sb.Append("]");
                }
                else
                {
                    sb.AppendFormat("{0}:{1}", name, value);
                }
            }
            sb.Append("}");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
            {
                result = new DictionaryObject(dictionary);
                return true;
            }

            var arrayList = result as ArrayList;
            if (arrayList != null && arrayList.Count > 0)
            {
                if (arrayList[0] is IDictionary<string, object>)
                    result = new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DictionaryObject(x)));
                else
                    result = new List<object>(arrayList.Cast<object>());
            }

            return true;
        }
    }
}