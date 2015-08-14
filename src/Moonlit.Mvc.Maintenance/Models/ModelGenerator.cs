 

using System.Web.Mvc; 
using System; 
using System.ComponentModel.DataAnnotations;
using Moonlit.Mvc;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Controls;


using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;

namespace Moonlit.Mvc.Maintenance.Models
{

		public partial class AdminUserListModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W6)]
 
		[TextBox] 
		public string Keyword { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserUserName"
			)]
		[Field(FieldWidth.W6)]
 
		[TextBox] 
		public string UserName { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserIsEnabled"
			)]
		[Field(FieldWidth.W6)]
 
		[CheckBox] 
		public bool? IsEnabled { get; private set; }
	partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserList,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserListDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            };
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
		public partial class AdminUserCreateModel  : IEntityMapper<User> {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserUserName"
			)]
		[Field(FieldWidth.W6)]
 
		[TextBox] 
		public string UserName { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserLoginName"
			)]
		[Field(FieldWidth.W6)]
 
		[TextBox] 
		public string LoginName { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserPassword"
			)]
		[Field(FieldWidth.W6)]
 
		[PasswordBox] 
		public string Password { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserGender"
			)]
		[Field(FieldWidth.W6)]
 
		[SelectList(typeof(EnumSelectListProvider))] 
		public Gender? Gender { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserDateOfBirth"
			)]
		[Field(FieldWidth.W6)]
 
		[DatePicker] 
		public DateTime? DateOfBirth { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserIsSuper"
			)]
		[Field(FieldWidth.W6)]
 
		[CheckBox] 
		public bool IsSuper { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserRoles"
			)]
		[Field(FieldWidth.W6)]
 
		[SelectList(typeof(RoleSelectListProvider))] 
		public int[] Roles { get; private set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserIsEnabled"
			)]
		[Field(FieldWidth.W6)]
 
		[CheckBox] 
		public bool IsEnabled { get; private set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 

			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserCreate,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserCreateDescription,
				FormTitle = MaintCultureTextResources.AdminUserInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnToEntity(User entity);
        public void ToEntity(User entity )
        {
			entity.UserName = UserName;
			entity.LoginName = LoginName;
			entity.Password = Password;
			entity.Gender = Gender;
			entity.DateOfBirth = DateOfBirth;
			entity.IsSuper = IsSuper;
  
			entity.Roles = MapRolesToEntity(entity);
			OnToEntity(entity);
		}
 
	} 
}

