using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class RoleListModel : IPagedRequest
    {
        public RoleListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IMaintDbRepository MaintDbContext { get; set; }
        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var query = MaintDbContext.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.Name.StartsWith(keyword) || x.Name.StartsWith(keyword));
            }
            if (IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == IsEnabled);
            }

            return query;
        }

        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.Action("Create", "Role"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            var tableBuilder = new TableBuilder<Role>();
            template.Table = tableBuilder.Add(tableBuilder.CheckBox(x => x.RoleId.ToString(), controllerContext, "ids"), "", "RoleId")
                .Add(tableBuilder.Literal(x => x.Name.Format(), controllerContext), MaintCultureTextResources.RoleName, "Name")
                .Add(tableBuilder.Literal(x => x.IsBuildIn.Format(), controllerContext), MaintCultureTextResources.RoleIsBuildIn, "IsBuildIn")
                .Add(tableBuilder.Literal(x => x.IsEnabled.Format(), controllerContext), MaintCultureTextResources.RoleIsEnabled, "IsEnabled")
                .Add(x =>
                {
                    return new ControlCollection()
                    {
                        Controls = new List<Control>
                        {
                            new Link(MaintCultureTextResources.Edit, urlHelper.Action("Edit", "Role", new {id = x.Target.RoleId}), LinkStyle.Normal)
                        }
                    };
                }, MaintCultureTextResources.Operation).Build();
        }
    }
}