using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Moonlit.Net
{
    public class Reactor<THandler>
        where THandler : ReactorHandler, new()
    {
        private readonly List<THandler> _handlers = new List<THandler>();
        private TcpListener _listener;

        private void RemoveHandler(THandler handler)
        {
            lock (_handlers)
            {
                if (_handlers.Contains(handler))
                {
                    _handlers.Remove(handler);
                }
            }
        }

        private void AddHandler(THandler handler)
        {
            lock (_handlers)
            {
                if (!_handlers.Contains(handler))
                {
                    _handlers.Add(handler);
                }
            }
        }

        public void Close()
        {
            lock (_handlers)
            {
                foreach (THandler client in _handlers)
                {
                    client.Close();
                }
                _handlers.Clear();
                if (_listener != null)
                {
                    _listener.Stop();
                    _listener = null;
                }
            }
        }


        public void Accept(int listenPort)
        {
            var ipendpoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), listenPort);
            Accept(ipendpoint);
        }

        public void Accept(IPEndPoint endpoint)
        {
            _listener = new TcpListener(endpoint);
            _listener.Start();
            _listener.BeginAcceptSocket(OnAccepted, _listener);
        }

        protected virtual void OnAccepted(IAsyncResult result)
        {
            var listener = (TcpListener)result.AsyncState;
            Socket sock = null;
            try
            {
                if (!listener.Server.IsBound)
                {
                    return;
                }
                sock = listener.EndAcceptSocket(result);
            }
            catch (SocketException)
            {
                return;
            }

            var client = new THandler();
            client.Handler = sock;
            client.OnConnected();
            client.Closed += new EventHandler(client_Closed);
            AddHandler(client); 
            listener.BeginAcceptSocket(OnAccepted, listener);
            client.BeginReceive();
        }

        void client_Closed(object sender, EventArgs e)
        {
            RemoveHandler((THandler) sender);
        }

    }
}