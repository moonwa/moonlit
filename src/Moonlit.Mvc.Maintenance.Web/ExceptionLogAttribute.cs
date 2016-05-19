using System;
using Moonlit.Mvc.Maintenance.Domains;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Moonlit.Mvc.Maintenance
{
    public class ExceptionLogAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {

            try
            {
                var db = new MaintDbContext();
                db.ExceptionLogs.Add(new ExceptionLog()
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