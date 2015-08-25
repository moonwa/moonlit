using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    /// <summary>
    /// 如果 DataContract 必须包含
    /// </summary>
    public class DataContractAttributeIsRequiredForParameterOfOperationContractRule : BaseIntrospectionRule
    {
        public DataContractAttributeIsRequiredForParameterOfOperationContractRule()
            : base("DataContractAttributeIsRequiredForParameterOfOperationContractRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }
        public override ProblemCollection Check(Parameter parameter)
        {
            var method = parameter.DeclaringMethod;
            if (method.HasCustomAttribute(typeof(OperationContractAttribute).FullName))
            {
                if (Helper.IsDataContractNesscery(parameter.Type) && !parameter.Type.HasCustomAttribute(typeof(DataContractAttribute).FullName))
                {
                    var resolution = this.GetResolution(new object[] { parameter });
                    Problems.Add(new Problem(resolution, parameter.SourceContext));
                }
            }
            return Problems;
        }
    }
}