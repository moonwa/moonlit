using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{
    internal class OptionArgumentEntityCollection : List<IParameterEntity>
    {
        internal IParseEntity Find(string key)
        {
            foreach (var item in this)
            {
                if (item.Name == key)
                {
                    return item;
                }
            }
            return null;
        }

        internal void Parse(ParseEnumerator enumer)
        {
            while (!enumer.End)
            {
                foreach (IParseEntity parser in this)
                {
                    if (parser.Parse(enumer))
                        break;
                }
            }
        }
    }
}
