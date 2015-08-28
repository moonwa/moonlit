using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class MethodHasToBeVirtualForOperationContractSvcRule : BaseIntrospectionRule
    {
        public MethodHasToBeVirtualForOperationContractSvcRule()
            : base("MethodHasToBeVirtualForOperationContractSvcRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        { 
            Microsoft.FxCop.Sdk.Method method = member as Microsoft.FxCop.Sdk.Method;
            if (method == null)
            {
                return base.Check(member);
            }

            if (method.DeclaringType is ClassNode && method.IsPublic && !(method is InstanceInitializer) && method.DeclaringType.Name.Name.EndsWith("Svc") && (!method.IsVirtual || ((long)method.Flags & (long)Microsoft.FxCop.Sdk.MethodFlags.Final) != 0))
            {
                var resolution = this.GetResolution(new object[] { method });
                Problems.Add(new Problem(resolution, method.SourceContext));
            }
            return Problems;
        }
    }
}