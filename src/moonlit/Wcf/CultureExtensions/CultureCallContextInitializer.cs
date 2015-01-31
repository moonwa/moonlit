using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Moonlit.Wcf.CultureExtensions
{
    public class CultureCallContextInitializer : ICallContextInitializer
    {
        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {
            try
            {
                var currentCulture = Thread.CurrentThread.CurrentCulture;
                var currentUICulture = Thread.CurrentThread.CurrentUICulture;

                if (message.Headers.FindHeader(CultureName.LocalName, CultureName.Ns) > 0)
                {
                    var cultureName = message.Headers.GetHeader<CultureName>(CultureName.LocalName, CultureName.Ns);
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName.Name);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName.Name);
                }
                return new[] { currentCulture, currentUICulture };
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public void AfterInvoke(object correlationState)
        {
            if (correlationState != null)
            {
                CultureInfo[] cultures = correlationState as CultureInfo[];
                Thread.CurrentThread.CurrentCulture = cultures[0];
                Thread.CurrentThread.CurrentUICulture = cultures[1];
            }
        }
    }
}