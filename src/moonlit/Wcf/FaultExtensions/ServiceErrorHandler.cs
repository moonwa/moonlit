using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Moonlit.Wcf.FaultExtensions
{
    public class ServiceErrorHandler : IErrorHandler
    {
        public ServiceErrorHandler()
        { 
        }

        #region IErrorHandler¡¡Members

        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (typeof (FaultException).IsInstanceOfType(error))
            {
                return;
            }

            try
            {
                fault = Message.CreateMessage(version, BuildFault(error), ServiceExceptionDetail.FaultAction);
            }
            catch (Exception ex)
            {
                fault = Message.CreateMessage(version, BuildFault(ex), ServiceExceptionDetail.FaultAction);
            }
        }

        private MessageFault BuildFault(Exception error)
        {
            var exceptionDetail = new ServiceExceptionDetail(error);
            return
                MessageFault.CreateFault(
                    FaultCode.CreateReceiverFaultCode(ServiceExceptionDetail.FaultSubCodeName,
                                                      ServiceExceptionDetail.FaultSubCodeNamespace),
                    new FaultReason(error.Message), exceptionDetail);
        }

        #endregion

    }
}