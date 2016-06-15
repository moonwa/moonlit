using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Moonlit.Timers
{
    public class DefaultKeyStore : IKeyStore
    {
        private readonly IKeyStore _innerStore;
        private readonly string _defaultValue;

        public DefaultKeyStore(IKeyStore innerStore, string defaultValue)
        {
            _innerStore = innerStore;
            _defaultValue = defaultValue;
        }

        public string Get(string key)
        {
            return _innerStore.Get(key) ?? _defaultValue;
        }

        public void Set(string key, string value)
        {
            _innerStore.Set(key, value);
        }
    }

    public static class KeyStoreExtensions
    {
        public static IKeyStore AsDefault(this IKeyStore keyStore, string defaultValue)
        {
            return new DefaultKeyStore(keyStore, defaultValue);
        }
    }
    public interface IKeyStore
    {
        string Get(string key);
        void Set(string key, string value);
    }
    public class ContinuousTimer
    {
        public TimeSpan Interval { get; set; }
        private readonly string _lastTimeKey;
        public IKeyStore KeyStore { get; set; }
        private Timer _timer;
        private bool _stoped = true;

        public ContinuousTimer(string lastTimeKey, TimeSpan interval)
        {
            Interval = interval;
            _lastTimeKey = lastTimeKey;
            KeyStore = new NoneKeyStore();
        }

        public void Start()
        {
            if (!_stoped)
            {
                return;
            }
            _stoped = false;
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimer;
            _timer.Enabled = true;
        }

        public event EventHandler<EventArgs<Exception>> Error = delegate { };
        public event EventHandler Elapsed = delegate { };
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_stoped)
                {
                    return;
                }
                _timer.Enabled = false;
                var lastTimeValue = KeyStore.Get(_lastTimeKey);
                if (string.IsNullOrEmpty(lastTimeValue))
                {
                    return;
                }
                var lastTime = Convert.ToDateTime(lastTimeValue);
                int intervalCount = -1;
                do
                {
                    lastTime = lastTime.Add(Interval);
                    intervalCount++;
                } while (lastTime < DateTime.Now);

                if (intervalCount == 0)
                {
                    return;
                }

                    lastTime = lastTime.Add(-Interval);
                Elapsed(this, EventArgs.Empty);
                KeyStore.Set(_lastTimeKey, lastTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<Exception>(ex));
            }
            finally
            {
                if (!_stoped)
                {
                    _timer.Enabled = true;
                }
            }
        }
    }

    public class AppSettingsKeyStore : IKeyStore
    {
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public void Set(string key, string value)
        {
            System.Configuration.Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (oConfig.AppSettings.Settings[key] == null)
            {
                oConfig.AppSettings.Settings.Add(key, "");
            }
            oConfig.AppSettings.Settings[key].Value = value;
            oConfig.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
    public class NoneKeyStore : IKeyStore
    {
        public string Get(string key)
        {
            return null;
        }

        public void Set(string key, string value)
        {
        }
    }
}
