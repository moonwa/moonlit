using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class Authenticate
    {
        private readonly ICacheManager _cacheManager;
        private readonly TimeSpan _expiredTime = TimeSpan.FromDays(300);
        public Authenticate(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager.GetPrefixCacheManager("Authenticate");
        }

        class SessionToken
        {
            public string UserName { get; set; }
            public string SessionId { get; set; }
            public override string ToString()
            {
                return UserName + ":" + SessionId;
            }

            public static SessionToken Pack(string s)
            {
                var arr = s.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length < 2)
                {
                    throw new Exception("Fail to Parse SessionId: " + s);
                }
                return new SessionToken
                {
                    SessionId = arr[1],
                    UserName = arr[0],
                };
            }
        }
        public string SetSession(string userName, Session session)
        {
            session.UserName = userName;

            var sessions = _cacheManager.Get<List<Session>>(userName) ?? new List<Session>();
            sessions.Add(session);

            var sessionId = Guid.NewGuid().ToString();
            session.SessionId = sessionId;

            _cacheManager.Set(userName, sessions, _expiredTime);

            SessionToken sid = new SessionToken
            {
                SessionId = sessionId,
                UserName = userName,
            };
            var cookie = FormsAuthentication.GetAuthCookie(userName, false);
            cookie.Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(1000), false, sid.ToString()));
            HttpContext.Current.Response.Cookies.Add(cookie);
            return sessionId;
        }

        static Session FindSessionById(List<Session> sessions, string sessionId)
        {
            return sessions.FirstOrDefault(x => string.Equals(x.SessionId, sessionId, StringComparison.OrdinalIgnoreCase));
        }
        public void SignOut()
        {
            var sessionToken = GetSessionToken();
            if (sessionToken != null)
            {
                var sessions = _cacheManager.Get<List<Session>>(sessionToken.UserName);
                if (sessions != null)
                {
                    var session = FindSessionById(sessions, sessionToken.SessionId);
                    if (session != null)
                    {
                        sessions.Remove(session);
                    }
                    _cacheManager.Set(sessionToken.UserName, sessions, _expiredTime);
                }
            }
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
        }
        public Session GetSession()
        {
            var sessionToken = GetSessionToken();
            if (sessionToken == null)
            {
                return null;
            }
            var sessions = _cacheManager.Get<List<Session>>(sessionToken.UserName);
            if (sessions == null)
            {
                return null;
            }
            var session = FindSessionById(sessions, sessionToken.SessionId);
            if (session != null && session.ExpiredTime > DateTime.Now)
            {
                return session;
            }
            return null;
        }

        SessionToken GetSessionToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    return SessionToken.Pack(ticket.UserData.ToString());
                }
            }

            var sid = HttpContext.Current.Request.QueryString["sid"];
            if (!string.IsNullOrWhiteSpace(sid))
            {
                return SessionToken.Pack(sid);
            }
            return null;
        }
    }
}
