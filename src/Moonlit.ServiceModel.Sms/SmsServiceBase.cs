using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using Moonlit.Data;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public abstract class SmsServiceBase : IStartable
    {
        protected SmsConfiguration Config
        {
            get { return (SmsConfiguration)ConfigurationManager.GetSection("sms"); }
        }
        protected abstract void OnSend(string number, string message);
        protected abstract void BeginSend();
        protected abstract void EndSend();
        private Timer _timer;
        public void Start()
        {
            _timer = new Timer(TimerElapsed, null, 5000, 5000);
            Logger.Debug("_timer started");
            Logger.Debug("okok");
        }

        class MessageEntry
        {
            public string Mobile { get; set; }
            public string Message { get; set; }

            public int SmsId { get; set; }
        }
        public void Send(string number, string message)
        {
            try
            {
                BeginSend();
                OnSend(number, message);
            }
            finally
            {
                EndSend();
            }
        }
        private void TimerElapsed(object state)
        {
            try
            {
                Console.WriteLine("okbbb");
                Logger.Debug("time Elapsed");
                _timer.Change(999999, 999999);
                var db = new Database("sms");
                using (var conn = db.OpenConnection())
                {
                    var sql = "select * from sms where RetryCount > 0 and (ReservateTime is null or ReservateTime < getdate()) and ExpiredTime > getdate() and state = 1";
                    var cmd = db.CreateCommand(sql, conn);
                    List<MessageEntry> msgs = new List<MessageEntry>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MessageEntry msg = new MessageEntry();
                            msg.Mobile = reader.GetValue("Mobile", msg.Mobile);
                            msg.Message = reader.GetValue("Message", msg.Message);
                            msg.SmsId = reader.GetValue("SmsId", msg.SmsId);
                            if (string.IsNullOrWhiteSpace(msg.Message) || string.IsNullOrWhiteSpace(msg.Mobile))
                                continue;
                            msgs.Add(msg);
                        }
                    }
                    if (msgs.Count > 0)
                        try
                        {
                            BeginSend();
                            foreach (var msg in msgs)
                            {
                                try
                                {
                                    OnSend(msg.Mobile, msg.Message);
                                    var cmd2 = conn.CreateCommand();
                                    cmd2.CommandText = "update sms set RetryCount =RetryCount -1, state = 10 where smsid = " + msg.SmsId;
                                    cmd2.ExecuteNonQuery();
                                }
                                catch
                                {
                                    var cmd2 = conn.CreateCommand();
                                    cmd2.CommandText = "update sms set RetryCount =RetryCount -1 where smsid = " + msg.SmsId;
                                    cmd2.ExecuteNonQuery();
                                    throw;
                                }
                            }
                        }
                        finally
                        {
                            EndSend();
                        }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                Logger.Debug("time started");
                _timer.Change(5000, 5000);
            }
        }

        private ILog Logger = LogManager.GetLogger(typeof(SmsServiceBase));
    }
}