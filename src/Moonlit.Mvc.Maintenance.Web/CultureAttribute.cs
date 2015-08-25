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
    public class CultureLoader : ICultureLoader
    {

        #region Implementation of ICultureLoader

        public ICulture Load(IUser user)
        {
            var ngmaDomainService = DependencyResolver.Current.GetService<IMaintDomainService>();
            var cultures = ngmaDomainService.GetCultures();
            var systemSettings = ngmaDomainService.GetSystemSettings();
            SiteModel siteModel = new SiteModel(systemSettings);


            Culture culture = cultures.FirstOrDefault(x => x.IsEnabled && x.CultureId == siteModel.DefaultCulture) ?? cultures.FirstOrDefault(x => x.IsEnabled);

            if (user != null)
            {
                User tuser = user as User;
                if (tuser != null && tuser.CultureId != 0)
                {
                    return cultures.FirstOrDefault(x => x.CultureId == tuser.CultureId) ?? culture;
                }
            }
            return culture;
        }

        #endregion
    }
}
