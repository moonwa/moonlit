using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(
        ResourceType = typeof(MaintCultureTextResources),
        Name = "Keyword"
        )]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string Keyword { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserUserName"
            )]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string UserName { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserIsEnabled"
            )]
        [Field(FieldWidth.W6)]
        [CheckBox]
        public bool? IsEnabled { get; set; }


        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.AdminUserList,
                Description = MaintCultureTextResources.AdminUserListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = TemplateHelper.MakeFields(this, controllerContext),
            };
            OnTemplate(template, controllerContext);
            return template;
        }
        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.GetRequestMappingUrl("CreateUser"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            template.Table = new TableBuilder<User>().ForEntity(controllerContext).Build();
           
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }


        private IQueryable<User> GetDataSource(ControllerContext controllerContext)
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