using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitMvcAttribute : ActionFilterAttribute
    {
        private readonly RequestMappings _requestMappings;
        private readonly Themes _themes;

        public MoonlitMvcAttribute(RequestMappings requestMappings, Themes themes)
        {
            _requestMappings = requestMappings;
            _themes = themes;
        }

        internal IDependencyResolver DependencyResolver { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var moonlitContext = MoonlitContext.Current;

            moonlitContext.RegisterScript("submit_with_action", new Script
            {
                Content = @"submit_with_action = function(sender, formAction, name, value) {
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
                            };"
            });
            foreach (var parameterDescriptor in filterContext.ActionDescriptor.GetParameters())
            {
                if (parameterDescriptor.ParameterType == typeof(RequestMappings))
                {
                    filterContext.ActionParameters[parameterDescriptor.ParameterName] = _requestMappings;
                }
            }

            moonlitContext.Theme = _themes.GetTheme(null);
            moonlitContext.Theme.PreRequest(moonlitContext, filterContext.RequestContext);
            moonlitContext.DependencyResolver = DependencyResolver;
            base.OnActionExecuting(filterContext);
        }
    }
}