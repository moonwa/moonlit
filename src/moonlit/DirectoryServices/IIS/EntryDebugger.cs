using System.Collections.Generic;
using System.DirectoryServices;

namespace Moonlit.DirectoryServices.IIS
{
    public class EntryDebugger
    {
        private readonly DirectoryEntry _entry;

        public EntryDebugger(DirectoryEntry entry)
        {
            _entry = entry;
        }
        public string[] PropertyNames
        {
            get
            {
                List<string> arr = new List<string>();
                foreach (var propertyName in _entry.Properties.PropertyNames)
                {
                    arr.Add((string)propertyName);
                }
                return arr.ToArray();
            }
        }
        public string[] PropertyValues
        {
            get
            {
                List<string> arr = new List<string>();
                foreach (PropertyValueCollection value in _entry.Properties.Values)
                {
                    List<string> ss = new List<string>();
                    for (int i = 0; i < value.Count; i++)
                    {
                        ss.Add(value[i] as string);
                    }
                    arr.Add(string.Join(",", ss.ToArray()));
                }
                return arr.ToArray();
            }
        }
    }
}