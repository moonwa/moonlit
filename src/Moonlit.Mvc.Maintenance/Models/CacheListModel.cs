using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Caching;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CacheListModel : IPagedRequest
    {
        public CacheListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Key";
        }

        public string Name { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Template CreateTemplate(RequestContext requestContext, ICacheManager cacheManager, CacheKeyManager cacheKeyManager)
        {
            var urlHelper = new UrlHelper(requestContext);
            var query = cacheKeyManager.AllKeys.Select(x => new
            {
                Name = x,
                IsNull = !cacheManager.Exist(x)
            }).ToList().AsQueryable();
      
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim();

                query = query.Where(x => x.Name.Contains(name));
            }


            return new AdministrationSimpleListTemplate(query)
            {
                Title = CultureTextResources.CultureTextList,
                Description = CultureTextResources.CultureTextListDescription,
                QueryPanelTitle = CultureTextResources.PanelQuery,
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CacheName,
                        FieldName = "Name",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Name
                        }
                    },
                },
                DefaultSort = "Name",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Table = new Table
                {
                    Columns = new[]
                    {
                        new TableColumn
                        {
                            Sort = "Name",
                            Header = CultureTextResources.CacheName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((dynamic) x.Target).Name
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Exist",
                            Header = CultureTextResources.Null,
                            CellTemplate = x => new Literal
                            {
                                Text = ((dynamic) x.Target).IsNull == true ? CultureTextResources.Yes : CultureTextResources.No
                            }
                        }
                       
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = CultureTextResources.Search,
                        ActionName = ""
                    },
                    new Button
                    {
                        Text = CultureTextResources.Clear,
                        ActionName = "Clear"
                    },  
                },
                RecordButtons = new IClickable[]
                {
                }
            };
        }
    }
}