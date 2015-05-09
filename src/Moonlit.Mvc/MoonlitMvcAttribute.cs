using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitMvcAttribute : ActionFilterAttribute
    {
        private readonly RequestMappings _requestMappings;

        public MoonlitMvcAttribute(RequestMappings requestMappings)
        {
            _requestMappings = requestMappings;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MoonlitContext.Current.RegisterScript("submit_with_action", @"submit_with_action = function(sender, formAction, name, value) {
      var button, form;
      if (!formAction) {
        button = $(""<button style='display:none' />"");
      } else {
        button = $(""<button style='display:none' name='form_action' value='"" + formAction + ""' />"");
      }
      form = $('body form');
      form.append(button);
      if (name != null) {
        if (name) {
          if ($(""[name='"" + name + ""']"", form).length === 0) {
            form.append($(""<input type='hidden' name='"" + name + ""' value='"" + value + ""' />""));
          } else {
            $(""[name='"" + name + ""']"", form).val(value);
          }
        }
      }
      return button.click();
    };");
            foreach (var parameterDescriptor in filterContext.ActionDescriptor.GetParameters())
            {
                if (parameterDescriptor.ParameterType == typeof(RequestMappings))
                {
                    filterContext.ActionParameters[parameterDescriptor.ParameterName] = _requestMappings;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}