using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CultureTextListModel : IPagedRequest 
    {
        public CultureTextListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }
        [SelectList(typeof(CultureSelectListItemsProvider))]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureName")]
        public int Culture { get; set; }
        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Keyword")]
        public string Keyword { get; set; }
        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextName")]
        public string Name { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Template CreateTemplate(ControllerContext controllerContext, IMaintDbRepository db)
        {
            var query = db.CultureTexts.Where(x => x.CultureId == Culture);
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

            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.CultureTextList,
                Description = MaintCultureTextResources.CultureTextListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = "Name",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
                Table = new Table
                {
                    Columns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            CellTemplate = x => new CheckBox()
                            {
                                Name = "ids",
                                Value = ((CultureText) x.Target).CultureTextId.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Name",
                            Header = MaintCultureTextResources.CultureTextName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((CultureText) x.Target).Name
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Text",
                            Header = MaintCultureTextResources.CultureTextText,
                            CellTemplate = x => new Literal
                            {
                                Text = ((CultureText) x.Target).Text
                            }
                        },
                        new TableColumn
                        {
                            Header = MaintCultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("editculturetext").MakeUrl(urlHelper, new {id = ((CultureText) x.Target).CultureTextId});
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
                },
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
                        Url = RequestMappings.Current.GetRequestMapping("createculturetext").MakeUrl(urlHelper, null),
                    },
                    new Link
                    {
                        Text = MaintCultureTextResources.Import,
                        Style = LinkStyle.Button,
                        Url = RequestMappings.Current.GetRequestMapping("importculturetext").MakeUrl(urlHelper, null),
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Export,
                        ActionName = "Export"
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Delete,
                        ActionName = "Delete"
                    },
                }
            }; 
            return template;
        } 

    }
}