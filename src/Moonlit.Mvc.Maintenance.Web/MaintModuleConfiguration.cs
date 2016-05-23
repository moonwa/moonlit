using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Autofac;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moonlit.Mvc.Maintenance
{
    internal class MaintModuleConfiguration : IModuleConfiguration
    {
        public void Configure(IContainer container)
        {
            AuthorizeManager.Setup();
            ModelBinders.Binders.DefaultBinder = new MaintInjectBinder(ModelBinders.Binders.DefaultBinder, container);

            Formatter.Register((x, v) => x == typeof(bool) || x == typeof(bool?), new BooleanFormatter(() => MaintCultureTextResources.Yes, () => MaintCultureTextResources.No));
            Formatter.Register((x, v) => (x == typeof(DateTime) || x == typeof(DateTime?)) && v != null && ((DateTime)v).Date == ((DateTime)v), new DateFormatter());
            Formatter.Register((x, v) => (x == typeof(DateTime) || x == typeof(DateTime?)), new DateTimeFormatter());
            Formatter.Register((x, v) => x.ToWithoutNullableType().IsEnum, new EnumFormatter());
            LanguageUpdater languageUpdater = new LanguageUpdater("http://cdn.hizhanzhang.com/apps/sample/zh-cn.lang", "zh-cn");
            languageUpdater.Update();
        }
    }

    internal class LanguageUpdater
    {
        private readonly string _url;
        private readonly string _cultureName;

        public LanguageUpdater(string url, string cultureName)
        {
            _url = url;
            _cultureName = cultureName;
        }

        public void Update()
        { 
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var text = client.DownloadString(_url);
            var items = (JObject)JsonConvert.DeserializeObject(text);
            using (var db = new MaintDbContext())
            {
                var culture = GetCulture(db);

                UpdateCultureTexts(db, items, culture);
            }
        }

        private void UpdateCultureTexts(MaintDbContext db, JObject items, Culture culture)
        {
            var cultureTexts = db.CultureTexts.Where(x => x.Culture.Name == _cultureName).ToList();
            foreach (var prop in items.Properties())
            {
                var cultureText = cultureTexts.FirstOrDefault(x => x.Name.EqualsIgnoreCase(prop.Name));
                if (cultureText == null)
                {
                    cultureText = new CultureText
                    {
                        Name = prop.Name,
                        Culture = culture,
                    };
                    db.CultureTexts.Add(cultureText);
                }
                if (!cultureText.IsEdited)
                {
                    cultureText.Text = prop.Value.ToString();
                }
            }
            db.SaveChanges();
        }

        private Culture GetCulture(MaintDbContext db)
        {
            var culture = db.Cultures.FirstOrDefault(x => x.Name == _cultureName);
            if (culture == null)
            {
                culture = new Culture
                {
                    Name = _cultureName,
                    DisplayName = _cultureName,
                    IsEnabled = true
                };
                db.Cultures.Add(culture);
                db.SaveChanges();
            }
            return culture;
        }
    }
}