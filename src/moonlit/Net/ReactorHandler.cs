using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using Moonlit.Collections;

namespace Moonlit.Net
{
    public class ReactorHandler
    {
        public ReactorHandler()
        {
            MessagesToSend = new Queue<MessageBlock>();
            ReceiveBuffer = new byte[MaxReceiveSize];
        }

        public event EventHandler Closed = delegate { };
        public void Close()
        {
            try
            {
                OnClose();
                this.Handler.Close();
            }
            catch (Exception )
            {
            }
        }

        public byte[] ReceiveBuffer { get; set; }
        protected Queue<MessageBlock> MessagesToSend { get; private set; }
        public Socket Handler { get; internal set; }
        public void Send(byte[] bytes)
        {

            lock (this.MessagesToSend)
            {
                this.MessagesToSend.Enqueue(new MessageBlock(bytes));
            }
            this.Send();
        }
        void Send()
        {
            lock (this.MessagesToSend)
            {
                while (this.MessagesToSend.Count != 0)
                {
                    MessageBlock messageToSend = this.MessagesToSend.Dequeue();
                    if (messageToSend.IsEmpty)
                        continue;

                    try
                    {
                        this.Handler.BeginSend(messageToSend.Data, messageToSend.Index, messageToSend.RemainLength, SocketFlags.None, this.OnSended, messageToSend);
                        return;
                    }
                    catch (SocketException sex)
                    {
                        OnException(sex.SocketErrorCode);
                        return;
                    }
                    catch (ObjectDisposedException)
                    { 
                        return;
                    }
                }
            }
        }
        private const int MaxReceiveSize = 1024 * 10;
        protected virtual void OnReceived(IAsyncResult result)
        {
            try
            {
                int recvCnt = Handler.EndReceive(result);
                if (recvCnt == 0)
                {
                    OnException(SocketError.ConnectionReset);
                    return;
                }
                var recvData = new byte[recvCnt];
                Buffer.BlockCopy(ReceiveBuffer, 0, recvData, 0, recvCnt);
                OnReceived(recvData);
                this.BeginReceive();
            }
            catch (SocketException sex)
            {
                OnException(sex.SocketErrorCode);
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        public void BeginReceive()
        {
            try
            {
                Handler.BeginReceive(ReceiveBuffer, 0, MaxReceiveSize, SocketFlags.None, OnReceived, this);
            }
            catch (SocketException sex)
            {
                OnException(sex.SocketErrorCode);
                return;
            }
            catch (ObjectDisposedException)
            { 
                return;
            }
        }
        void OnSended(IAsyncResult result)
        {
            lock (this.MessagesToSend)
            {
                try
                {
                    int sendCount = this.Handler.EndSend(result);

                    MessageBlock msg = (MessageBlock)result.AsyncState;
                    msg.Read(sendCount);
                    this.MessagesToSend.Enqueue(msg);
                    this.Send();
                }
                catch (SocketException sex)
                {
                    OnException(sex.SocketErrorCode);
                    return;
                }
                catch (ObjectDisposedException)
                { 
                    return;
                }
            }
        }
        public virtual void OnException(SocketError error)
        {
            if (error != SocketError.Success)
            {
                OnDisconnected();
                this.Close();
                return;
            }
        }
        public virtual void OnReceived(byte[] data)
        {
        }
        public virtual void OnConnected()
        {
        }
        public virtual void OnDisconnected()
        {
        }
        public virtual void OnClose()
        {
        }
    }
}