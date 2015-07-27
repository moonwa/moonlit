using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
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

        public int Culture { get; set; }
        public string Keyword { get; set; }
        public string Name { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            if (Culture == 0)
            {
                Culture = new SiteModel(db.SystemSettings).DefaultCulture;
            }
            var culture = db.Cultures.FirstOrDefault(x => x.IsEnabled);
            if (Culture == 0 && culture != null)
            {
                Culture = culture.CultureId;
            }
            var cultures = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
            }).ToList();

            var urlHelper = new UrlHelper(requestContext);
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


            return new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.CultureTextList,
                Description = MaintCultureTextResources.CultureTextListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.Keyword,
                        FieldName = "Keyword",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Keyword
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureTextName,
                        FieldName = "Name",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value= Name,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureTextCulture,
                        FieldName = "Culture",
                        Control = new SelectList(cultures, Culture.ToString()),
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
                            Header =MaintCultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("editculturetext").MakeUrl(urlHelper, new {id= ((CultureText) x.Target).CultureTextId});
                                return new ControlCollection()
                                    { 
                                        Controls= new List<Control>() {
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
                },
                RecordButtons = new IClickable[]
                {
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
                }
            };
        }
    }
}