using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Controllers;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class UserLoginFailedLogIndexModel : IPagedRequest
    {

        public UserLoginFailedLogIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "CreationTime desc";
        }
        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new List<IClickable>
            {
                new Button(MaintCultureTextResources.Search),
                new Button(MaintCultureTextResources.Delete, "Delete"),
            };
            var tableBuilder = new TableBuilder<UserLoginFailedLog>();
            template.Table = tableBuilder
                .Add(tableBuilder.CheckBox(x => x.UserLoginFailedLogId.Format(), controllerContext, name: "ids"), "", "UserLoginFailedLogId")
                .Add(x => new Link(x.Target.User.UserName, urlHelper.Action("Edit", "User", new { id = x.Target.UserId })), MaintCultureTextResources.UserLoginFailedLogUser, "CreateTime")
                .Add(tableBuilder.Literal(x => x.IpAddress.Format(), controllerContext), MaintCultureTextResources.UserLoginFailedLogIpAddress, "IpAddress")
                .Add(tableBuilder.Literal(x => x.CreationTime.Format(), controllerContext), MaintCultureTextResources.UserLoginFailedLogCreateTime, "CreateTime")
                .Build();
        }

        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var database = ((MaintControllerBase)controllerContext.Controller).Database;
            var query = database.UserLoginFailedLogs.Include(x => x.User);
            if (UserId != null)
            {
                query = query.Where(x => x.UserId == this.UserId);
            }
            return query;
        }
    }
}