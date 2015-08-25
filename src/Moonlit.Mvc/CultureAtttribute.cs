using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moonlit.Mvc
{

    public class CultureAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var cultureLoader = DependencyResolver.Current.GetService<ICultureLoader>();

                ICulture culture = cultureLoader.Load(filterContext.HttpContext.User.Identity as IUser);

                if (culture != null)
                {
                    var cultureInfo = new CultureInfo(culture.Name);
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    Thread.CurrentThread.CurrentCulture = cultureInfo;
                }
            }
            catch (Exception ex)
            {
                //                                LogHelper.WriteError("设置语言失败", ex);
            }
        }
    }

    public interface ICulture
    {
        string Name { get; }
    }

    public interface ICultureLoader
    {
        ICulture Load(IUser user);
    }
}
