using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Moonlit.Mvc.Scripts;

namespace Moonlit.Mvc.Themes
{
    public abstract class Theme
    {
        public override string ToString()
        {
            return Name;
        }
        private Dictionary<Type, string> _control2Templates = new Dictionary<Type, string>();

        public void RegisterControl(Type controlType, string template)
        {
            _control2Templates[controlType] = template;
        }

        public string ResolveControl(Type controlType)
        {
            string template;
            if (_control2Templates.TryGetValue(controlType, out template))
            {
                return template;
            }
            return null;
        }

        protected internal virtual void PreRequest(RequestContext requestContext)
        {
            var scripts = requestContext.HttpContext.GetObject<Scripts.Scripts>();
            if (scripts != null)
            {
                scripts.RegisterScript("submit_with_action", new Script
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
            }
        }

        public abstract string Name { get; }
    }
}