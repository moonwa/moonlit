using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Controllers;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class CultureTextIndexModel : IPagedRequest
    {
        public CultureTextIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } 
        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var repository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = repository.CultureTexts.Where(x => x.CultureId == Culture);
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.Name.Contains(keyword) || x.Text.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim();

                query = query.Where(x => x.Name.StartsWith(name));
            }
            return query;
        }

        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new List<IClickable>
            {
                new Button(MaintCultureTextResources.Search ),
                new Link(MaintCultureTextResources.New, urlHelper.Action("Create", "CultureText", new {cultureId = this.Culture}), LinkStyle.Button),
                new Button(MaintCultureTextResources.Delete, "Delete"),
                new Link(MaintCultureTextResources.Import, urlHelper.Action("Import", "CultureText", new {cultureId=this.Culture}), LinkStyle.Button), 
            };
            var tableBuilder = new TableBuilder<CultureText>();
            template.Table = tableBuilder
                .Add(tableBuilder.CheckBox(x => x.CultureTextId.Format(), controllerContext, name: "ids"), "", "CultureTextId")
                .Add(tableBuilder.Literal(x => x.Name.Format(), controllerContext), MaintCultureTextResources.AdminUserUserName, "CultureTextName")
                .Add(tableBuilder.Literal(x => x.Text.Format(), controllerContext), MaintCultureTextResources.AdminUserLoginName, "CultureTextText")
                .Add(tableBuilder.Literal(x => x.IsEdited.Format(), controllerContext), MaintCultureTextResources.AdminUserGender, "CultureTextIsEdited")
                .Add(x => new ControlCollection(
                    new Link(MaintCultureTextResources.Edit, urlHelper.Action("Edit", "CultureText", new { id = x.Target.CultureTextId }), LinkStyle.Normal)
                    ), MaintCultureTextResources.Operation).Build();
        }
    }
}