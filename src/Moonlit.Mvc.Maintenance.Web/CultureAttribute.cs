using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Services;

namespace Moonlit.Mvc.Maintenance
{
    public class CultureAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var ngmaDomainService = DependencyResolver.Current.GetService<IMaintDomainService>();
            var cultures = ngmaDomainService.GetCultures();
            var systemSettings = ngmaDomainService.GetSystemSettings();
            SiteModel siteModel = new SiteModel(systemSettings);


            Culture culture = cultures.FirstOrDefault(x => x.IsEnabled && x.CultureId == siteModel.DefaultCulture) ?? cultures.FirstOrDefault(x => x.IsEnabled);

            if (filterContext.HttpContext.User != null)
            {
                User user = filterContext.HttpContext.User.Identity as User;
                if (user != null && user.CultureId != 0)
                {
                    culture = cultures.FirstOrDefault(x => x.CultureId == user.CultureId) ?? culture;
                }
            }


            if (culture != null)
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture.Name);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture.Name);
                }
                catch (Exception ex)
                {
                    //                                LogHelper.WriteError("设置语言失败", ex);
                }
            }

        }
    }
}
