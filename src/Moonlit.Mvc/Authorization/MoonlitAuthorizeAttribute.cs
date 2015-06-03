using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Authorization
{
    public class UserAttribute : ActionFilterAttribute
    {
        private readonly IActionResultModelResolver _resultModelResolver;

        public UserAttribute(IActionResultModelResolver resultModelResolver)
        {
            _resultModelResolver = resultModelResolver;
        }

        public UserAttribute()
            : this(ActionResultModelResolver.Current)
        {

        }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = _resultModelResolver.ResolveModel(filterContext) as IUserModel;
            if (model != null)
            {
                model.User = filterContext.HttpContext.User;
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public interface IUserModel
    {
        IPrincipal User { set; }
    }

    public class MoonlitAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly Authenticate _authenticate;
        private readonly IAuthenticateProvider _authenticateProvider;

        public MoonlitAuthorizeAttribute(Authenticate authenticate, IAuthenticateProvider authenticateProvider)
        {
            _authenticate = authenticate;
            _authenticateProvider = authenticateProvider;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User != null)
            {
                var session = _authenticate.GetSession();
                if (session != null)
                {
                    IUserPrincipal user = _authenticateProvider.GetUserPrincipal(session.UserName);

                    if (user != null)
                    {
                        user.Privileges = (user.Privileges ?? new string[0]).Intersect(session.Privileges ?? new string[0]).ToArray();
                        filterContext.HttpContext.User = user;
                    }
                }
            }
        }
    }
}