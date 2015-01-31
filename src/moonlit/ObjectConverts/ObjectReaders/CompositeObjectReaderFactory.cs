using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectReaders
{
    internal class CompositeObjectReaderFactory : IObjectReaderFactory
    {
        public CompositeObjectReaderFactory()
        {
            _factories = new SortedList<int, IObjectReaderFactory>();
        }
        private SortedList<int, IObjectReaderFactory> _factories;

        public void Register(int index, IObjectReaderFactory factory)
        {
            _factories.Add(index, factory);
        }
        public IObjectReader CreateReader(object obj)
        {
            foreach (var objectReaderFactory in _factories)
            {
                var reader = objectReaderFactory.Value.CreateReader(obj);
                if (reader != null)
                {
                    return reader;
                }
            }
            return null;
        }
    }
}