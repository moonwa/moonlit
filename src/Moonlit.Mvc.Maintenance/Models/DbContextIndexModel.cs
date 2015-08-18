using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Controllers;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class DbContextIndexModel : IPagedRequest
    {
        public DbContextIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var urlHelper = new UrlHelper(requestContext);
            var query = BuildManager.GetReferencedAssemblies()
                    .Cast<Assembly>()
                    .Select(x => x).Where(x => x.GetCustomAttribute<MvcAttribute>() != null)
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(DbContext).IsAssignableFrom(x))
                    .AsQueryable();

            return new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.DevToolsDbContextIndex,
                Description = MaintCultureTextResources.DevToolsDbContextListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                Criteria = new Field[0],
                DefaultSort = "Name",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Table = new Table
                {
                    Columns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            CellTemplate = x => new CheckBox()
                            {
                                Name = "ids",
                                Value = ((Type) x.Target).AssemblyQualifiedName
                            },
                        },
                        new TableColumn
                        {
                            CellTemplate = x => new Literal()
                            {
                                Text = ((Type) x.Target).FullName
                            },
                        },
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Search,
                        ActionName = ""
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.ExportAsNode,
                        ActionName = DbContextController.FormActionNameExportAsNodeJs
                    }, 
                },
                RecordButtons = new IClickable[]
                {
                 
                }
            };
        }
    }
}