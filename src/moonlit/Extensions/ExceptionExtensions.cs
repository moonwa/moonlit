using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public static class ExceptionExtensions
    {
        public static Exception Trim(this Exception exception)
        {
            AggregateException aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                exception = aggregateException.InnerExceptions[0];
            }
            return exception;
        }
    }
}
