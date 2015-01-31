using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class DataContractAttributeIsRequiredForPropertyOfDataContractRule : BaseIntrospectionRule
    {
        public DataContractAttributeIsRequiredForPropertyOfDataContractRule()
            : base("DataContractAttributeIsRequiredForPropertyOfDataContractRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }
        public override ProblemCollection Check(Member member)
        {
            var property = member as PropertyNode;
            if (property == null)
            {
                return base.Check(member);
            }

            if (property.HasCustomAttribute(typeof(OperationContractAttribute).FullName))
            {
                if (Helper.IsDataContractNesscery(property.Type) &&
                    !property.Type.HasCustomAttribute(typeof(DataContractAttribute).FullName))
                {
                    var resolution = this.GetResolution(new object[] { property.Type });
                    Problems.Add(new Problem(resolution, property.Type.SourceContext ));
                }
            }
            return Problems;
        }
    }
}