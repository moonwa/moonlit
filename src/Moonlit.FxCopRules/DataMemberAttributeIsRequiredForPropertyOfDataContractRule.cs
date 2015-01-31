using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class DataMemberAttributeIsRequiredForPropertyOfDataContractRule : BaseIntrospectionRule
    {
        public DataMemberAttributeIsRequiredForPropertyOfDataContractRule()
            : base("DataMemberAttributeIsRequiredForPropertyOfDataContractRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            var property = member as PropertyNode;
            if (property == null)
            {
                return base.Check(member);
            }
            if (member.DeclaringType.HasCustomAttribute(typeof(DataContractAttribute).FullName))
            {
                if (property.IsPublic && !property.IsStatic && property.Getter != null && property.Setter != null)
                {
                    Debug.Write("Check Property " + property, "Code Analysis");

                    if (!property.HasCustomAttribute(typeof(DataMemberAttribute).FullName) && !property.HasCustomAttribute(typeof(IgnoreDataMemberAttribute).FullName))
                    {
                        var resolution = this.GetResolution(new object[] { member.DeclaringType, property });
                        Problems.Add(new Problem(resolution, member.SourceContext));
                    }
                }
            }
            return Problems;
        }
    }
}
