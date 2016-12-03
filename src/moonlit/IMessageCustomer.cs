using System;

namespace Moonlit
{
    public interface IMessageCustomer<TMessage>
    {
        event EventHandler<EventArgs<TMessage>> Received;
    }
}