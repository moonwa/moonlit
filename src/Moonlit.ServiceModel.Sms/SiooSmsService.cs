using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using log4net;

namespace Moonlit.ServiceModel.Sms
{
    public class SiooSmsService : SmsServiceBase
    {
        protected override void BeginSend()
        {
             
        }

        protected override void EndSend()
        {
             
        }

        protected override void OnSend(string number, string message)
        {
            var config = Config;
            WebClient client = new WebClient();
            
                var computeHash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(config.Password));
                //var content = BitConverter.ToString(Encoding.GetEncoding("GBK").GetBytes(msg.Message)).ToLower().Replace("-", "");
                var content = HttpUtility.UrlEncode(Encoding.GetEncoding("GBK").GetBytes(message));
                var url = string.Format("{0}:{1}?user={2}&pass={3}&mobile={4}&content={5}", config.Host, config.Port,
                                        HttpUtility.UrlEncode(config.UserName),
                                        BitConverter.ToString(computeHash).Replace("-", "").ToLower(),
                                        number, content
                                        );
                try
                {
                    var s = client.DownloadString(url);
                    if (s != "200")
                    {
                        throw new Exception(string.Format("send message {0} failed", GetError(s)));
                    } 
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("send message {0} success", ex.Message));
                } 
        }

        static Dictionary<string, string> _errors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      
        static string GetError(string key)
        {
            if (_errors.ContainsKey(key))
                return _errors[key];
            return key;
        }
        static SiooSmsService()
        {
            _errors.Add("200", "操作成功");
            _errors.Add("101", "用户或密码错误");
            _errors.Add("102", "号码为空");
            _errors.Add("103", "保留");
            _errors.Add("104", "非法字符");
            _errors.Add("105", "内容过长");
            _errors.Add("106", "剩余短信不足");
            _errors.Add("107", "发送速度过快");
            _errors.Add("108", "号码过多");
            _errors.Add("109", "暂停发送");
            _errors.Add("110", "手机号码有误");
            _errors.Add("111", "保留");
            _errors.Add("112", "短信内容为空");
            _errors.Add("113", "账户无效");
            _errors.Add("114", "操作失败");
            _errors.Add("118", "修改密码,新密码为空");
        } 
    }
}