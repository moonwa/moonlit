using System;
using System.Collections.Generic;
using System.Linq;
using Moonlit.Mvc.Maintenance.Domains;

namespace Moonlit.Mvc.Maintenance.Models
{
    public abstract class SystemSettingModel
    {
        private readonly List<SystemSetting> _settings;
        private readonly string _category;

        protected SystemSettingModel(IEnumerable<SystemSetting> settings, string category)
        {
            _settings = settings.Where(x => x.Category == category).ToList();
            _category = category;
        }
        protected DateTime? GetValue(string name, DateTime? defaultValue)
        {
            var setting = _settings.FirstOrDefault(x => x.Category == _category && x.Name == name);
            if (setting == null || string.IsNullOrEmpty(setting.Value))
            {
                return defaultValue;
            }
            return Convert.ToDateTime(setting.Value);
        }
        protected string GetValue(string name, string defaultValue)
        {
            var setting = _settings.FirstOrDefault(x => x.Category == _category && x.Name == name);
            if (setting == null)
            {
                return defaultValue;
            }
            return setting.Value;
        }
        public IEnumerable<SystemSetting> Settings { get { return this._settings; } }
        protected decimal GetValue(string name, decimal defaultValue)
        {
            var setting = _settings.FirstOrDefault(x => x.Category == _category && x.Name == name);
            if (setting == null)
            {
                return defaultValue;
            }
            return Convert.ToDecimal(setting.Value);
        }
        protected int GetValue(string name, int defaultValue)
        {
            var setting = _settings.FirstOrDefault(x => x.Category == _category && x.Name == name);
            if (setting == null)
            {
                return defaultValue;
            }
            return Convert.ToInt32(setting.Value);
        }
        protected void SetValue(string name, string value)
        {
            var setting = _settings.FirstOrDefault(x => x.Category == _category && x.Name == name);
            if (setting == null)
            {
                _settings.Add(new SystemSetting()
                {
                    Name = name,
                    Value = value,
                    Category = _category
                });
            }
            else
            {
                setting.Value = value;
            }
        }
        public void Save(IMaintDbRepository db)
        {
            foreach (var settings in Settings)
            {
                if (settings.SystemSettingId == 0)
                {
                    db.Add(settings);
                }
            }
        }
    }
}