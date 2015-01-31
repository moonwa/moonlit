using System;
using System.IO;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public class LogSmsService : SmsServiceBase
    {
        private static string path;
        static LogSmsService()
        {
            path = Path.GetDirectoryName(typeof(LogSmsService).Assembly.Location);
            path = Path.Combine(path, "messages");
        }
        protected override void BeginSend()
        {
             
        }
        protected override void EndSend()
        {
             
        }
        protected override void OnSend(string number, string message)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, string.Format("{0}-{1}.txt", number, DateTime.Now.Ticks.ToString())), message);
        }
    }
}