using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Url;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class AdminUserListModel : IPagedRequest
    {
        public AdminUserListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.Action("Create", "User"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            var tableBuilder = new TableBuilder<User>();
            template.Table = tableBuilder
                .Add(tableBuilder.CheckBox(x => x.UserId.Format(), controllerContext, name: "ids"), "", "UserId")
                .Add(tableBuilder.Literal(x => x.UserName.Format(), controllerContext), MaintCultureTextResources.AdminUserUserName, "UserName")
                .Add(tableBuilder.Literal(x => x.LoginName.Format(), controllerContext), MaintCultureTextResources.AdminUserLoginName, "LoginName")
                .Add(tableBuilder.Literal(x => x.Gender.Format(), controllerContext), MaintCultureTextResources.AdminUserGender, "Gender")
                .Add(tableBuilder.Literal(x => x.DateOfBirth.Format(), controllerContext), MaintCultureTextResources.AdminUserDateOfBirth, "DateOfBirth")
                .Add(tableBuilder.Literal(x => x.IsBuildIn.Format(), controllerContext), MaintCultureTextResources.AdminUserIsBuildIn, "IsBuildIn")
                .Add(tableBuilder.Literal(x => x.IsEnabled.Format(), controllerContext), MaintCultureTextResources.AdminUserIsEnabled, "IsEnabled")
                .Add(tableBuilder.Literal(x => x.IsSuper.Format(), controllerContext), MaintCultureTextResources.AdminUserIsSuper, "IsSuper")
                .Add(x => new ControlCollection(
                    new Link(MaintCultureTextResources.Edit, urlHelper.Action("Edit", "User", new { id = x.Target.UserId }), LinkStyle.Normal)
                    ), MaintCultureTextResources.Operation).Build();
        }

        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var irepository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = irepository.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.LoginName.StartsWith(keyword) || x.UserName.StartsWith(keyword));
            }
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var userName = UserName.Trim();

                query = query.Where(x => x.UserName.StartsWith(userName));
            }
            if (IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == IsEnabled);
            }
            return query;
        }
    }
}