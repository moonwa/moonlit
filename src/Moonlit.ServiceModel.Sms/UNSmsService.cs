using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Moonlit.ServiceModel.Sms
{
    /// <summary>
    /// UNCom 短信平台
    /// </summary>
    public class UNSmsService : SmsServiceBase
    {
        [DllImport("winsms")]
        public static extern string Sendsms(string uid, string pwd, string mobno, string content, string mycode, string time);

        ////查余额 函数
        //function Remoney(uid,pwd:pchar):pchar;stdcall;  

        ////接收短信  函数
        //function Getsms(uid,pwd:pchar):pchar;stdcall;

        ////修改密码 函数
        //function Editpwd(uid,pwd,newpwd:pchar):pchar;stdcall; 


        static UNSmsService()
        {
            _errors.Add(0, "成送成功");
            _errors.Add(-1, "当前账号余额不足");
            _errors.Add(-2, "当前用户ID错误");
            _errors.Add(-3, "当前密码错误");
            _errors.Add(-4, "参数不够或参数内容的类型错误");
            _errors.Add(-5, "手机号码格式不对");
            _errors.Add(-6, "短信内容编码不对");
            _errors.Add(-7, "短信内容含有敏感字符");
            _errors.Add(-8, "无接收数据");
            _errors.Add(-9, "系统维护中..");
            _errors.Add(-10, "手机号码数量超长");
            _errors.Add(-11, "短信内容超长");
            _errors.Add(-12, "其它错误");
        }

        protected override void BeginSend()
        {
        }
        protected override void EndSend()
        {
        }

        static Dictionary<int, string> _errors = new Dictionary<int, string>();
        protected override void OnSend(string number, string message)
        {
            var config = base.Config;
            var s = Sendsms(config.UserName, config.Password, number, message, "", "&now&");
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Trim();

                var pos = s.IndexOf("/");
                if (pos > 0)
                {
                    s = s.Substring(0, pos);
                    int v = 0;
                    if (int.TryParse(s, out v))
                    {
                        if (v == 0) return;
                        if (_errors.ContainsKey(v))
                            throw new Exception(_errors[v]);
                    }
                }
            }
            throw new Exception(s ?? "null");
        }
    }
}