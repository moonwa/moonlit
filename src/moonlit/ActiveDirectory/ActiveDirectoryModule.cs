using System.Web;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveDirectoryModule : System.Web.IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
        }

        void context_AuthenticateRequest(object sender, System.EventArgs e)
        {
            
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}