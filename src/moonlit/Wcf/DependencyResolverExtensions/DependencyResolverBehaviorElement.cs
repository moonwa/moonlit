using System;
using System.ServiceModel.Configuration;

namespace Moonlit.Wcf.DependencyResolverExtensions
{
    public class DependencyResolverBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            var unityContainer = DependencyResolverContainerFactory.Container; 

            return new DependencyResolverBehavior(unityContainer);
        }

        public override Type BehaviorType
        {
            get { return typeof(DependencyResolverBehavior); }
        }
    }
}