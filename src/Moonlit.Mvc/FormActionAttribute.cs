using System;
using System.Reflection;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class FormActionAttribute : ActionNameSelectorAttribute
    {
        private readonly string _actionName;

        public FormActionAttribute(string actionName)
        {
            _actionName = actionName;
        }

        public FormActionAttribute()
        {
            
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var formActionName = _actionName;
            if (string.IsNullOrWhiteSpace(formActionName))
            {
                formActionName = methodInfo.Name;
            }
            var action = controllerContext.Controller.ValueProvider.GetValue("form_action");
            if (action == null)
            {
                return false;
            }
            return string.Equals(action.AttemptedValue, formActionName, StringComparison.OrdinalIgnoreCase);
        }
    }
}