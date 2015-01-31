using System;
using System.ServiceModel.Configuration;
using Moonlit.Wcf.CultureExtensions;

namespace Moonlit.Wcf.FaultExtensions
{
    public class FaultBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ExceptionHandlingBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(ExceptionHandlingBehavior); }
        }
    }
}