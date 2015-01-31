using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;

namespace Moonlit.Wcf.CultureExtensions
{
    public class CultureMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            MessageHeader<CultureName> header = new MessageHeader<CultureName>(new CultureName(Thread.CurrentThread.CurrentUICulture.Name));
            request.Headers.Add(header.GetUntypedHeader(CultureName.LocalName, CultureName.Ns));
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}
