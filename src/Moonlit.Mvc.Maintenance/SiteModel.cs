using System.Collections.Generic;
using System.Diagnostics;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Maintenance.Domains;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class SiteModel : SystemSettingModel, ISite
    {
        public SiteModel(IEnumerable<SystemSetting> settings)
            : base(settings, "Site")
        {
        }

        public string SiteName
        {
            get { return GetValue("SiteName", "Maint 示例站点"); }
            set { SetValue("SiteName", value); }
        }

        /// <summary>
        ///     主货币
        /// </summary>
        public int PrimaryCurrency
        {
            get { return GetValue("PrimaryCurrency", 0); }
            set { SetValue("PrimaryCurrency", value.ToString()); }
        }

        /// <summary>
        ///     默认语言
        /// </summary>
        public int DefaultCulture
        {
            get { return GetValue("DefaultCulture", 0); }
            set { SetValue("DefaultCulture", value.ToString()); }
        }
        public int MaxSignInFailTimes
        {
            get { return GetValue("MaxSignInFailTimes", 5); }
            set { SetValue("MaxSignInFailTimes", value.ToString()); }
        }

        public const string VersionFirst = "0.1";

        public string DBVersion
        {
            get
            {
                return GetValue("DBVersion", VersionFirst);
            }
            set
            {
                SetValue("DBVersion", value);
            }
        }

        public string FullVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(typeof(SiteModel).Assembly.Location).FileDescription + "-" + DBVersion;
            }
        }

        public string CopyRight
        {
            get { return "moon.wa 版权所有"; }
        }

        public string Support
        {
            get { return "QQ: 6914337"; }
        }

        string ISite.Title
        {
            get { return SiteName; }
        }

        string ISite.CopyRight
        {
            get { return CopyRight; }
        }
         
    }
}