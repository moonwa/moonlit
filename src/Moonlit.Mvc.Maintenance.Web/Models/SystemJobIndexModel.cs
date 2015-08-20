using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class SystemJobIndexModel : IPagedRequest
    {
        public SystemJobIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "StartTime desc";
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
                new Button(MaintCultureTextResources.Abort, "Abort"),
            };
            var tableBuilder = new TableBuilder<SystemJob>();
            template.Table = tableBuilder
                .Add(tableBuilder.CheckBox(x => x.SystemJobId.Format(), controllerContext, name: "ids"), "")
                .Add(tableBuilder.Literal(x => x.Title.Format(), controllerContext), MaintCultureTextResources.SystemJobTitle, "Title")
                .Add(tableBuilder.Literal(x => x.StartTime.Format(), controllerContext), MaintCultureTextResources.SystemJobStartTime, "StartTime")
                .Add(tableBuilder.Literal(x => x.ExecuteTime.Format(), controllerContext), MaintCultureTextResources.SystemJobExecuteTime, "ExecuteTime")
                .Add(tableBuilder.Literal(x => x.Status.Format(), controllerContext), MaintCultureTextResources.SystemJobStatus, "Status")
                .Add(tableBuilder.Literal(x => x.CreationTime.Format(), controllerContext), MaintCultureTextResources.SystemJobCreationTime, "CreationTime")
                .Build();
        }

        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var irepository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = irepository.SystemJobs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.Name.Contains(keyword) || x.Title.Contains(keyword));
            }
            
            if (Status != null)
            {
                query = query.Where(x => x.Status == Status);
            }
            return query;
        }
    }
}