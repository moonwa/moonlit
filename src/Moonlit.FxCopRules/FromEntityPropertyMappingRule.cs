using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FxCop.Sdk;

namespace Moonlit.FxCopRules
{
    public class FromEntityPropertyMappingRule : BaseIntrospectionRule
    {
        public FromEntityPropertyMappingRule()
            : base("FromEntityPropertyMappingRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }

        #region Overrides of BaseIntrospectionRule

        public override ProblemCollection Check(Member member)
        {
            if (!member.HasCustomAttribute("Moonlit.Mvc.MappingAttribute"))
            {
                return Problems;
            }
            var mappingAttr = NodeExtensions.GetCustomAttribute(member, "Moonlit.Mvc.MappingAttribute");
            var fromEntityType = member.DeclaringType.Interfaces.FirstOrDefault(x => x.IsGeneric && x.Template.FullName == "Moonlit.Mvc.IFromEntity`1");
            if (fromEntityType == null)
            {
                return Problems;
            }
            var entityType = fromEntityType.TemplateArguments[0];
            string toProperty = member.Name.Name;
            var to = mappingAttr.Expressions.OfType<NamedArgument>().FirstOrDefault(x => x.Name.Name == "To");
            if (to != null)
            {
                toProperty = to.Value.ToString();
            }
            var property = entityType.GetProperty(Identifier.For(toProperty));
            if (property == null)
            {
                var resolution = this.GetResolution(new object[] {member.DeclaringType.FullName,  member.Name.Name, entityType.FullName, toProperty});
                Problems.Add(new Problem(resolution, member.SourceContext));
            }
            return Problems;
        }

        #endregion
    }
    public class ToEntityPropertyMappingRule : BaseIntrospectionRule
    {
        public ToEntityPropertyMappingRule()
            : base("ToEntityPropertyMappingRule", GlobalConstant.ResourceName, typeof(GlobalConstant).Assembly)
        {
        }

        #region Overrides of BaseIntrospectionRule

        public override ProblemCollection Check(Member member)
        {
            if (!member.HasCustomAttribute("Moonlit.Mvc.MappingAttribute"))
            {
                return Problems;
            }
            var mappingAttr = NodeExtensions.GetCustomAttribute(member, "Moonlit.Mvc.MappingAttribute");
            var fromEntityType = member.DeclaringType.Interfaces.FirstOrDefault(x => x.IsGeneric && x.Template.FullName == "Moonlit.Mvc.IToEntity`1");
            if (fromEntityType == null)
            {
                return Problems;
            }
            var entityType = fromEntityType.TemplateArguments[0];
            string toProperty = member.Name.Name;
            var to = mappingAttr.Expressions.OfType<NamedArgument>().FirstOrDefault(x => x.Name.Name == "To");
            if (to != null)
            {
                toProperty = to.Value.ToString();
            }
            var property = entityType.GetProperty(Identifier.For(toProperty));
            if (property == null)
            {
                var resolution = this.GetResolution(new object[] {member.DeclaringType.FullName,  member.Name.Name, entityType.FullName, toProperty});
                Problems.Add(new Problem(resolution, member.SourceContext));
            }
            return Problems;
        }

        #endregion
    }
}
