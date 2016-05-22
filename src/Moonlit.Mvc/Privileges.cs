using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Privileges
    {
        public static Privileges Current
        {
            get
            {
                var privileges = HttpContext.Current.GetObject<Privileges>();
                if (privileges == null)
                {
                    var loader = MoonlitDependencyResolver.Current.Resolve<IPrivilegeLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }

                    privileges = loader.Load();

                    HttpContext.Current.SetObject(privileges);
                }
                return privileges;
            }
        }

        public List<Privilege> Items { get; set; }
    }
}