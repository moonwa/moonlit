using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class Culture
    {
        public int CultureId { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        [StringLength(128)]
        public string DisplayName { get; set; }
        public bool IsEnabled { get; set; }

        public void Import(MaintDbContextMaintDbRepository repository, List<KeyValuePair<string, string>> cultureTexts, LanguageItemConverType languageItemConverType)
        {
            var culture = this;
            var originalCultureTexts =
                repository.CultureTexts.Where(x => x.CultureId == culture.CultureId).ToList();
            foreach (var cultureText in cultureTexts)
            {
                var text = (cultureText.Value ?? "").ToString();
                var key = cultureText.Key;
                var originalLanguageItem = originalCultureTexts.FirstOrDefault(x => string.Equals(x.Name, key, StringComparison.OrdinalIgnoreCase));
                if (originalLanguageItem == null)
                {
                    originalLanguageItem = new CultureText()
                    {
                        Text = text,
                        CultureId = culture.CultureId,
                        Name= key,
                    };
                    repository.CultureTexts.Add(originalLanguageItem);
                }
                else
                {
                    if (originalLanguageItem.IsEdited)
                    {
                        switch (languageItemConverType)
                        {
                            case LanguageItemConverType.ExcludeIsEdited:
                                break;
                            case LanguageItemConverType.All:
                                originalLanguageItem.Text = text;
                                originalLanguageItem.IsEdited = false;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        switch (languageItemConverType)
                        {
                            case LanguageItemConverType.ExcludeIsEdited:
                            case LanguageItemConverType.All:
                                originalLanguageItem.Text = text;
                                originalLanguageItem.IsEdited = false;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        public void Import(MaintDbContextMaintDbRepository repository, LanguageItemConverType languageItemConverType)
        {
            var stream = typeof(Culture).Assembly.GetManifestResourceStream("Moonlit.Mvc.Maintenance.Properties.languages." + this.Name.ToLower() + ".lang");
            if (stream == null || stream == Stream.Null)
            {
                return;
            }
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            var s = reader.ReadToEnd();
            var languageItemsJson = (JObject)JsonConvert.DeserializeObject(s);

            List<KeyValuePair<string, string>> languageItems = new List<KeyValuePair<string, string>>();
            foreach (var kv in languageItemsJson)
            {
                var text = (kv.Value ?? "").ToString();
                var key = kv.Key;
                languageItems.Add(new KeyValuePair<string, string>(key, text));
            }
            Import(repository, languageItems, languageItemConverType);
        }
    }

    public enum LanguageItemConverType
    {
        ExcludeIsEdited, All
    }
}