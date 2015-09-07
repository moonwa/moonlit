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

            var action = controllerContext.Controller.ValueProvider.GetValue("form_action");
            if (action == null)
            {
                if (string.IsNullOrWhiteSpace(formActionName))
                {
                    return true;
                }
                return false;
            }
            if (string.IsNullOrWhiteSpace(formActionName) && string.IsNullOrWhiteSpace(action.AttemptedValue))
            {
                return true;
            }
            return string.Equals(action.AttemptedValue, formActionName, StringComparison.OrdinalIgnoreCase);
        }
    }
}