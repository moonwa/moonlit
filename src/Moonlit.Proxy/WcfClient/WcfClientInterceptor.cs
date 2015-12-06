using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Moonlit.Proxy.WcfClient
{
    public class WcfClientProxy
    {
        public static TInterface CreateProxy<TInterface>() where TInterface : class
        {
            ProxyGenerator proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<TInterface>(new WcfClientInterceptor<TInterface>());
        }
    }
    internal class WcfClientInterceptor<T> : IInterceptor
    {
        static ChannelFactory<T> factory = new ChannelFactory<T>("*");
        public WcfClientInterceptor()
        {

        }
        public void Intercept(IInvocation invocation)
        {
            if (typeof(Task).IsAssignableFrom(invocation.Method.ReturnType))
            {
                ICommunicationObject channel = (ICommunicationObject)factory.CreateChannel();
                invocation.ReturnValue = invocation.Method.Invoke(channel, invocation.Arguments);
                var task = invocation.ReturnValue as Task;
                var taskAwaiter = task.GetAwaiter();
                taskAwaiter.OnCompleted(() =>
                {
                    if (channel.State != CommunicationState.Faulted)
                    {
                        channel.Close();
                    }
                });
            }
            else
            {
                ICommunicationObject channel = (ICommunicationObject)factory.CreateChannel();
                try
                {
                    invocation.ReturnValue = invocation.Method.Invoke(channel, invocation.Arguments);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
                finally
                {
                    if (channel.State != CommunicationState.Faulted)
                    {
                        channel.Close();
                    }
                }
            }
        }
    }

}
