using System;
using System.ServiceModel.Configuration;
using Moonlit.Wcf.CultureExtensions;

namespace Moonlit.Wcf.CultureExtensions
{
    public class CultureBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new CultureEndpointBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(CultureEndpointBehavior); }
        }
    }
}