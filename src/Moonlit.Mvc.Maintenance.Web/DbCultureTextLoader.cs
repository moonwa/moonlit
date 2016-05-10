using System;
using System.Linq;
using System.Threading;
using System.Web;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Services;
using System.Web.Mvc;

namespace Moonlit.Mvc.Maintenance
{
    public class DbCultureTextLoader : ILanguageLoader
    {
        public string Get(string key)
        {
            var culture = GetCulture(Thread.CurrentThread.CurrentUICulture.Name);

            return Get(key, culture) ?? key;
        }


        private Culture GetCulture(string cultureName)
        {
            const string httpCultureNameKey = "__languageLoader::culture";

            var culture = HttpContext.Current.GetObject<Culture>();
            if (culture == null)
            {
                var ngmaDomainService = DependencyResolver.Current.GetService<IMaintDomainService>();
                culture =
                    ngmaDomainService.GetCultures()
                        .FirstOrDefault(
                            x => string.Equals(x.Name, cultureName, StringComparison.OrdinalIgnoreCase));
                //
                //                var cultures = ngmaDomainService.GetCultures();
                //                var systemSettings = ngmaDomainService.GetSystemSettings();
                //                var siteModel = new SiteModel(systemSettings);
                //
                //                var c = cultures.FirstOrDefault(x => string.Equals(x.Name, cultureName, StringComparison.OrdinalIgnoreCase));
                //                if (c != null)
                //                {
                //                    culture = c;
                //                    HttpContext.Current.Items[httpCultureNameKey] = c;
                //                    return culture;
                //                }
                //
                //                c = cultures.FirstOrDefault(x => x.CultureId == siteModel.DefaultCulture);
                //
                //                if (c != null)
                //                {
                //                    culture = c;
//                                    HttpContext.Current.Items[httpCultureNameKey] = c;
                                    HttpContext.Current.Items[httpCultureNameKey] = culture;
//                                    return culture;
                //                }


            }
            return culture;
        }

        private string Get(string key, Culture culture)
        {
            if (culture == null)
            {
                return null;
            }
            var ngmaDomainService = DependencyResolver.Current.GetService<IMaintDomainService>();
            var db = DependencyResolver.Current.GetService<IMaintDbRepository>();

            var cultureTexts = ngmaDomainService.GetCultureTexts();
            var cacheKey = "__languageLoader::cultureTexts";

            var item = cultureTexts.FirstOrDefault(x => string.Equals(x.Name, key, StringComparison.OrdinalIgnoreCase) && x.CultureId == culture.CultureId);
            if (item != null)
            {
                return item.Text;
            }
            item = new CultureText
            {
                Name = key,
                CultureId = culture.CultureId,
                Text = null
            };
            db.CultureTexts.Add(item);
            db.SaveChanges();
            ngmaDomainService.ClearCultureTextsCache();
            HttpContext.Current.Items[cacheKey] = null;
            return null;
        }
    }

}