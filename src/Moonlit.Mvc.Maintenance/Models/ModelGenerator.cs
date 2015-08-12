 

	using System.Web.Mvc; 
	using System; 
	using System.ComponentModel.DataAnnotations;
	using Moonlit.Mvc;
	using Moonlit.Mvc.Templates;
	using Moonlit.Mvc.Maintenance.Domains;
	using Moonlit.Mvc.Controls;


		using Moonlit.Mvc.Maintenance.Properties;
namespace Moonlit.Mvc.Maintenance.Models
{

 

	public partial class AdminUserListModel {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W6)]
				[TextBox]
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserUserName"
			)]
		[Field(FieldWidth.W6)]
				[TextBox]
		public string UserName { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
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
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserListModelList,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserListModelListDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = TemplateHelper.MakeFields(this, controllerContext), 
            };
			OnTemplate (template, controllerContext);
            return template;
        }
	}

}

