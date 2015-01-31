using System.ServiceModel;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class OperationContractAttributeIsRequiredForMethodOfServiceContractRule : BaseIntrospectionRule
    {
        public OperationContractAttributeIsRequiredForMethodOfServiceContractRule()
            : base("OperationContractAttributeIsRequiredForMethodOfServiceContractRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;
            if (method == null)
            {
                return base.Check(member);
            }

            if ( method.IsPublic && method.DeclaringType.HasCustomAttribute(typeof(ServiceContractAttribute).FullName) && !method.HasCustomAttribute(typeof(OperationContractAttribute).FullName))
            {
                var resolution = this.GetResolution(new object[] { method });
                Problems.Add(new Problem(resolution, method.ReturnType.SourceContext.FileName, method.ReturnType.SourceContext.StartLine));
            }
            return Problems;
        }
    }
}