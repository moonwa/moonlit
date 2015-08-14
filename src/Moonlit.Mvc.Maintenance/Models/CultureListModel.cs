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
    public class CultureListModel : IPagedRequest
    {
        public CultureListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Keyword")]
        public string Keyword { get; set; }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureName")]
        public string Name { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext, IMaintDbRepository db)
        {
            var query = db.Cultures.AsQueryable();

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
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.CultureList,
                Description = MaintCultureTextResources.CultureListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = "Name",
                DefaultPageSize = 10,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Search,
                        ActionName = ""
                    },
                    new Link
                    {
                        Text = MaintCultureTextResources.New,
                        Style = LinkStyle.Button,
                        Url = urlHelper.Action( "Create" , "Culture"),
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Disable,
                        ActionName = "Disable"
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Enable,
                        ActionName = "Enable"
                    },
                },
                Table = new Table
                {
                    Columns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            CellTemplate = x => new CheckBox()
                            {
                                Name = "ids",
                                Value = ((Culture) x.Target).CultureId.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Name",
                            Header = MaintCultureTextResources.CultureName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).Name
                            }
                        },
                        new TableColumn
                        {
                            Sort = "DisplayName",
                            Header = MaintCultureTextResources.CultureDisplayName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).DisplayName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "IsEnabled",
                            Header = MaintCultureTextResources.CultureIsEnabled,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).IsEnabled ? MaintCultureTextResources.Yes : MaintCultureTextResources.No,
                            }
                        },
                        new TableColumn
                        {
                            Header = MaintCultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = urlHelper.Action("Edit", "Culture", new {id = ((Culture) x.Target).CultureId});
                                return new ControlCollection()
                                {
                                    Controls = new List<Control>()
                                    {
                                        new Link
                                        {
                                            Style = LinkStyle.Normal,
                                            Text = MaintCultureTextResources.Edit,
                                            Url = url,
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
            };
            return template;
        }
    }
}