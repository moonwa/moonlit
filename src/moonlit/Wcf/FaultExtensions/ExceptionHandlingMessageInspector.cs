using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Moonlit.Wcf.FaultExtensions
{
    public class ExceptionHandlingMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref¡¡Message reply, object correlationState)
        {
            if (!reply.IsFault)
            {
                return;
            }

            if (reply.Headers.Action == ServiceExceptionDetail.FaultAction)
            {
                MessageFault fault = MessageFault.CreateFault(reply, int.MaxValue);
                if (fault.Code.SubCode.Name == ServiceExceptionDetail.FaultSubCodeName &&
                    fault.Code.SubCode.Namespace == ServiceExceptionDetail.FaultSubCodeNamespace)
                {
                    FaultException<ServiceExceptionDetail> exception = (FaultException<ServiceExceptionDetail>)FaultException.CreateFault(fault, typeof(ServiceExceptionDetail));
                    throw GetException(exception.Detail);
                }
            }
        }

        private Exception GetException(ServiceExceptionDetail exceptionDetail)
        {
            return (Exception)exceptionDetail.DeserializeException();
        }

        public object BeforeSendRequest(ref¡¡Message request, IClientChannel channel)
        {
            return null;
        }
    }
}