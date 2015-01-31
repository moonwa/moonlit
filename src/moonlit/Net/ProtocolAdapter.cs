using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Moonlit.Net
{
    public class ProtocolAdapter<TProtocol>
        where TProtocol : class
    {
        protected IAsyNetwork Network { get; set; }
        protected IProtocolPacker<TProtocol> ProtocolPacker { get; set; }

        #region Event
        public event EventHandler<EventArgs<TProtocol>> Received;
        #endregion

        public ProtocolAdapter(IAsyNetwork asyNetwork, IProtocolPacker<TProtocol> protocolPacker)
        {
            this.Network = asyNetwork;
            ProtocolPacker = protocolPacker;

            BindingEvents(Network, ProtocolPacker);
        }
        public void Send(MessageNetworkItem item)
        {
            this.Network.Send(item);
        }
        private void BindingEvents(IAsyNetwork network, IProtocolPacker<TProtocol> protocolPacker)
        {
            network.Received += new EventHandler<EventArgs<MessageNetworkItem>>(Network_Received);
        }
        MemoryStream _buffer = new MemoryStream();
        void Network_Received(object sender, EventArgs<MessageNetworkItem> e)
        {
            MessageNetworkItem item = e.Value;
            _buffer.Write(item.Data, 0, item.Data.Length);
            _buffer.Position = 0;

            TProtocol protocol = null;
            do
            {
                protocol = ProtocolPacker.Pack(_buffer, item.RemoteEndPoint);

                if (protocol != null)
                {
                    if (_buffer.Position != 0)
                    {
                        byte[] buffer = new byte[_buffer.Length - _buffer.Position];
                        _buffer.Read(buffer, 0, buffer.Length);
                        _buffer = new MemoryStream();
                        _buffer.Write(buffer, 0, buffer.Length);
                    }
                    if (protocol != default(TProtocol))
                    {
                        var received = this.Received;
                        if (received != null)
                        {
                            received(this, new EventArgs<TProtocol>(protocol));
                        }
                    }
                }
            } while (protocol != null);
        }

    }
    /// <summary>
    /// 南网络调用接口
    /// </summary>
    public interface IAsyNetwork
    {
        event EventHandler<EventArgs<MessageNetworkItem>> Received;
        void Send(MessageNetworkItem item);
    }
    public interface IProtocolPacker<TProtocol>
        where TProtocol : class
    {
        TProtocol Pack(MemoryStream data, IPEndPoint remoteEndPoint);
        byte[] Unpack(TProtocol pack);
    }
}
