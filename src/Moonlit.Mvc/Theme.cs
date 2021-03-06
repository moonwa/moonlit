﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
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

        public static Theme Current
        {
            get
            {
                var theme = HttpContext.Current.GetObject<Theme>();
                if (theme != null)
                {
                    return theme;
                }
                var themeLoader = DependencyResolver.Current.GetService<IThemeLoader>(false);
                if (themeLoader == null)
                {
                    return null;
                }
                theme = themeLoader.Load();
                HttpContext.Current.SetObject(theme);
                return theme;
            }
        }
        protected internal virtual void PreRequest(RequestContext requestContext)
        {
            var scripts = Scripts.Current;
            if (scripts != null)
            {
                scripts.RegisterScript("submit_with_action", new Script
                   {
                       Content = @"submit_with_action = function(sender, formAction, name, value) {
                              var button, form;
                              form = $(sender).closest('form');
                              if (formAction) {
                                form.append($(""<input type='hidden' name='form_action' value='"" + formAction + ""' />""));
                              }
                            
                              if (name != null) {
                                if (name) {
                                  if ($(""[name='"" + name + ""']"", form).length === 0) {
                                    form.append($(""<input type='hidden' name='"" + name + ""' value='"" + value + ""' />""));
                                  } else {
                                    $(""[name='"" + name + ""']"", form).val(value);
                                  }
                                }
                              }
                              return form.submit();
                            };"
                   });
            }
        }

        public abstract string Name { get; }
    }
}