using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Moonlit.Net
{
    public class MessageNetworkItem
    {
        public byte[] Data { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        public MessageNetworkItem(byte[] data, IPEndPoint remoteEndPoint)
        {
            Data = data;
            RemoteEndPoint = remoteEndPoint;
        }
    }
}
