using System.Collections.Generic;
using System.DirectoryServices;

namespace Moonlit.DirectoryServices.IIS
{
    public class ServerBindings
    {
        List<ServerBinding> _serverBindings = new List<ServerBinding>();

        public int Count
        {
            get { return _serverBindings.Count; }
        }

        public void Add(ServerBinding serverBinding)
        {
            _serverBindings.Add(serverBinding);
            _propertieValues.Add(serverBinding.ToString());
        }
        public void Remove(ServerBinding serverBinding)
        {
            if (_serverBindings.Remove(serverBinding))
            {
                _propertieValues.Remove(serverBinding.ToString());
            }
        }
        private readonly PropertyValueCollection _propertieValues;

        internal ServerBindings(PropertyValueCollection propertieValues)
        {
            _propertieValues = propertieValues;
            for (int i = 0; i < propertieValues.Count; i++)
            {
                _serverBindings.Add(new ServerBinding(propertieValues[i] as string));
            }
        }


        public ServerBinding this[int index]
        {
            get { return _serverBindings[index]; }
            set
            {
                _serverBindings[index] = value;
                _propertieValues[index] = value.ToString();
            }
        }
    }
}