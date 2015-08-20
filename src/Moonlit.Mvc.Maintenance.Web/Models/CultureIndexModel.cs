using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Controllers;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class CultureIndexModel : IPagedRequest
    {
        public CultureIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }
 
        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        { 
            var tableBuilder = new TableBuilder<Culture>();
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.Action("Create", "Culture"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            template.Table = tableBuilder.Add(tableBuilder.CheckBox(x => x.CultureId.ToString(), controllerContext, "ids"), "", "CultureId")
                .Add(tableBuilder.Literal(x => x.Name.Format(), controllerContext), MaintCultureTextResources.RoleName, "Name")
                .Add(tableBuilder.Literal(x => x.DisplayName.Format(), controllerContext), MaintCultureTextResources.RoleIsBuildIn, "DisplayName")
                .Add(tableBuilder.Literal(x => x.IsEnabled.Format(), controllerContext), MaintCultureTextResources.RoleIsEnabled, "IsEnabled")
                .Add(x =>
                {
                    return new ControlCollection(
                        new Link(MaintCultureTextResources.Edit, urlHelper.Action("Edit", "Culture", new { id = x.Target.CultureId }), LinkStyle.Normal)
                        );
                }, MaintCultureTextResources.Operation).Build();
        
        }

        public IMaintDbRepository MaintDbContext { get; set; }
        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var query = MaintDbContext.Cultures.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();
                query = query.Where(x => x.Name.Contains(keyword) || x.DisplayName.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim();

                query = query.Where(x => x.Name.StartsWith(name));
            }
            return query;
        }
    }
}