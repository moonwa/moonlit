using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class DataContractAttributeIsRequiredForReturnTypeOfOperationContractRule : BaseIntrospectionRule
    {
        public DataContractAttributeIsRequiredForReturnTypeOfOperationContractRule()
            : base("DataContractAttributeIsRequiredForReturnTypeOfOperationContractRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;
            if (method == null)
            {
                return base.Check(member);
            }

            if (method.HasCustomAttribute(typeof(OperationContractAttribute).FullName))
            {
                if (Helper.IsDataContractNesscery(method.ReturnType)
                    && !(method.ReturnType.Template != null && method.ReturnType.Template.FullName == typeof(Task<>).FullName)
                    && !(method.ReturnType.FullName == typeof(Task).FullName)
                    && !method.ReturnType.HasCustomAttribute(typeof(DataContractAttribute).FullName))
                {
                    Console.WriteLine(method.ReturnType.FullName);
                    var resolution = this.GetResolution(new object[] { method.ReturnType });
                    Problems.Add(new Problem(resolution, method.ReturnType.SourceContext.FileName, method.ReturnType.SourceContext.StartLine));
                }
            }
            return Problems;
        }
    }
}