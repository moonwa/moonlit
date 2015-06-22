using System;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Newtonsoft.Json;

namespace Moonlit.Mvc.Maintenance
{
    public class ExceptionLogAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {

            try
            {
                var db = DependencyResolver.Current.GetService<IMaintDbRepository>();
                db.Add(new ExceptionLog()
                {
                    RouteData = JsonConvert.SerializeObject(filterContext.RouteData.Values),
                    Exception = filterContext.Exception.ToString(),
                    CreationTime = DateTime.Now,
                });
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}