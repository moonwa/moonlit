using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class SignInSession
    {
        public string UserName { get; set; }
        public string[] Privileges { get; set; }
    }
    public class Authenticate
    {
        private const string _prefix = "CacheAuthenticateService:";
        private readonly ICacheManager _cacheManager;
        private TimeSpan _expiredTime = TimeSpan.FromHours(100);
        public Authenticate(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string SetSession(SignInSession session)
        {

            var tokenId = Guid.NewGuid().ToString();

            _cacheManager.Set(_prefix + tokenId, session, _expiredTime);
            var cookie = FormsAuthentication.GetAuthCookie(session.UserName, false);

            cookie.Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, session.UserName, DateTime.Now,
                    DateTime.Now.AddHours(1000), false, tokenId));
            HttpContext.Current.Response.Cookies.Add(cookie);
            return tokenId;
        }
        public SignInSession GetSession()
        {
            var tokenId = GetSessionId();
            if (tokenId == null)
            {
                return null;
            }
            return _cacheManager.Get<SignInSession>(_prefix + tokenId);
        }

        string GetSessionId()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    return ticket.UserData.ToString();
                }
            }

            var sid = HttpContext.Current.Request.QueryString["sid"];
            if (!string.IsNullOrWhiteSpace(sid))
            {
                return sid;
            }
            return null;
        }
    }

    public interface IAuthenticateProvider
    {
        IUserPrincipal GetUserPrincipal(string name);
    }

    public interface IUserPrincipal : IPrincipal
    {
        string[] Privileges { get; set; }
    }
}
