using System;
using System.Linq;
using System.Threading;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Domains;

namespace Moonlit.Mvc.Maintenance.Loaders
{
    public class MaintainLanguageLoader : ILanguageLoader
    {
        private readonly ICacheManager _cacheManager;

        public MaintainLanguageLoader(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string Get(string key)
        {
            var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
            var cacheKey = $"languages-{cultureName.ToLower()}";
            var languages = _cacheManager.Get<LanguageItem[]>(cacheKey);
            if (languages == null)
            {
                var db = new MaintDbContext();
                languages = db.CultureTexts.Where(x => x.Culture.Name == cultureName)
                    .Select(x => new LanguageItem()
                    {
                        CultureName = cultureName,
                        Key = x.Name,
                        Text = x.Text,
                    }).ToArray();
                _cacheManager.Set(cacheKey, languages, TimeSpan.MaxValue);
            }
            return languages.FirstOrDefault(x => key.EqualsIgnoreCase(x.Key) )?.Text ?? key;
        }

        public class LanguageItem
        {
            public string Key { get; set; }
            public string Text { get; set; }
            public string CultureName { get; set; }
        }
    }
}