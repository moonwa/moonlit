using System;
using System.Collections.Generic;
using System.Linq;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Services
{
    public class MaintDomainService : IMaintDomainService
    {
        private readonly CacheKeyManager _cacheKeyManager;
        private readonly IMaintDbRepository _maintDbRepository;
        private readonly ICacheManager _cacheManager;
        private const string CacheKeySystemSettings = "MaintDomainService::SystemSettings";
        private const string CacheKeyCultures = "MaintDomainService::Cultures";
        private const string CacheKeyCultureTexts = "MaintDomainService::CultureTexts";
        public MaintDomainService(CacheKeyManager cacheKeyManager, ICacheManager cacheManager, IMaintDbRepository maintDbRepository)
        {
            _cacheKeyManager = cacheKeyManager;
            _maintDbRepository = maintDbRepository;
            _cacheManager = cacheManager;
             

            _cacheKeyManager.RegisterCacheKey(CacheKeySystemSettings);
            _cacheKeyManager.RegisterCacheKey(CacheKeyCultures);
            _cacheKeyManager.RegisterCacheKey(CacheKeyCultureTexts);
        }

        private TimeSpan _timeout = TimeSpan.FromHours(1);
        public List<SystemSetting> GetSystemSettings()
        {

            var systemSettings = _cacheManager.Get<List<SystemSetting>>(CacheKeySystemSettings);

            if (systemSettings == null)
            {
                var db = _maintDbRepository;
                systemSettings = db.SystemSettings.ToList();
                _cacheManager.Set(CacheKeySystemSettings, systemSettings, _timeout);
            }
            return systemSettings;
        }
        public List<Culture> GetCultures()
        {
            var cultures = _cacheManager.Get<List<Culture>>(CacheKeyCultures);

            if (cultures == null)
            {
                var db = _maintDbRepository;
                cultures = db.Cultures.Where(x => x.IsEnabled).ToList();
                _cacheManager.Set(CacheKeyCultures, cultures, _timeout);
            }
            return cultures;
        }
        public List<CultureText> GetCultureTexts()
        {
            var cultureTexts = _cacheManager.Get<List<CultureText>>(CacheKeyCultureTexts);

            if (cultureTexts == null)
            {
                var db = _maintDbRepository;

                cultureTexts = db.CultureTexts.ToList();
                _cacheManager.Set(CacheKeyCultureTexts, cultureTexts, _timeout);

            }
            return cultureTexts;
        }

        public void ClearCultureTextsCache()
        {
            _cacheManager.Remove(CacheKeyCultureTexts);
        }

        public void ClearSystemSettingsCache()
        {
            _cacheManager.Remove(CacheKeySystemSettings);
        }
    }

    public interface IMaintDomainService
    {
        List<SystemSetting> GetSystemSettings();
        List<Culture> GetCultures();
        List<CultureText> GetCultureTexts();
        void ClearCultureTextsCache();
        void ClearSystemSettingsCache();
    }
}
