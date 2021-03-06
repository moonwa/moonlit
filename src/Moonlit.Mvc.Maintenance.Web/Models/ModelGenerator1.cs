﻿ 

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

	public partial class AdminUserIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserUserName"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string UserName { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "AdminUserIsEnabled"
			)]
		[Field(FieldWidth.W4)]
 
		[CheckBox] 
		public bool? IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.AdminUserIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class RoleIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RoleIsEnabled"
			)]
		[Field(FieldWidth.W4)]
 
		[CheckBox] 
		public bool? IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class RoleCreateModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RoleName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RolePrivileges"
			)]
		[Field(FieldWidth.W4)]
 
		[MultiSelectList(typeof(PrivilegeSelectListProvider))] 
		[Mapping()]
		public string[] PrivilegeArray { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RoleIsEnabled"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public bool IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleCreate,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleCreateDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(Role entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(Role entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				Name = entity.Name;
  
				PrivilegeArray = MappingPrivilegeArrayFromEntity(entity, controllerContext);
 
				IsEnabled = entity.IsEnabled;
			}
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(Role entity, ControllerContext controllerContext);
        public void ToEntity(Role entity, ControllerContext controllerContext)
        {
			entity.Name = Name;
  
			entity.PrivilegeArray = MappingPrivilegeArrayToEntity(entity, controllerContext);
			entity.IsEnabled = IsEnabled;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class RoleEditModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RoleName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RolePrivileges"
			)]
		[Field(FieldWidth.W4)]
 
		[MultiSelectList(typeof(PrivilegeSelectListProvider))] 
		[Mapping()]
		public string[] PrivilegeArray { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "RoleIsEnabled"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public bool IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleEdit,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleEditDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.RoleInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(Role entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(Role entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				Name = entity.Name;
  
				PrivilegeArray = MappingPrivilegeArrayFromEntity(entity, controllerContext);
 
				IsEnabled = entity.IsEnabled;
			}
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(Role entity, ControllerContext controllerContext);
        public void ToEntity(Role entity, ControllerContext controllerContext)
        {
			entity.Name = Name;
  
			entity.PrivilegeArray = MappingPrivilegeArrayToEntity(entity, controllerContext);
			entity.IsEnabled = IsEnabled;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class CultureIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureName"
			)]
		[Field(FieldWidth.W4)]
		public string Name { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class CultureCreateModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureDisplayName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string DisplayName { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureIsEnabled"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public bool IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureCreate,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureCreateDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(Culture entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(Culture entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				Name = entity.Name;
 
				DisplayName = entity.DisplayName;
 
				IsEnabled = entity.IsEnabled;
			}
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(Culture entity, ControllerContext controllerContext);
        public void ToEntity(Culture entity, ControllerContext controllerContext)
        {
			entity.Name = Name;
			entity.DisplayName = DisplayName;
			entity.IsEnabled = IsEnabled;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class CultureEditModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureDisplayName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string DisplayName { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureIsEnabled"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public bool IsEnabled { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureEdit,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureEditDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(Culture entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(Culture entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				Name = entity.Name;
 
				DisplayName = entity.DisplayName;
 
				IsEnabled = entity.IsEnabled;
			}
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(Culture entity, ControllerContext controllerContext);
        public void ToEntity(Culture entity, ControllerContext controllerContext)
        {
			entity.Name = Name;
			entity.DisplayName = DisplayName;
			entity.IsEnabled = IsEnabled;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class CultureTextIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextCulture"
			)]
		[Field(FieldWidth.W4)]
 
		[SelectList(typeof(CultureSelectListProvider))] 
		public int Culture { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextName"
			)]
		[Field(FieldWidth.W4)]
		public string Name { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class CultureTextCreateModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextCulture"
			)]
		[Field(FieldWidth.W4)]
 
		[SelectList(typeof(CultureSelectListProvider))] 
		[Mapping()]
		public int CultureId { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextText"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Text { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextCreate,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextCreateDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(CultureText entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(CultureText entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				CultureId = entity.CultureId;
 
				Name = entity.Name;
 
				Text = entity.Text;
			}
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(CultureText entity, ControllerContext controllerContext);
        public void ToEntity(CultureText entity, ControllerContext controllerContext)
        {
			entity.CultureId = (int)CultureId;
			entity.Name = Name;
			entity.Text = Text;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class CultureTextEditModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextCulture"
			)]
		[Field(FieldWidth.W4)]
 
		[SelectList(typeof(CultureSelectListProvider))] 
		[Mapping(To="CultureId")]
		public int? CultureId { get; internal set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; internal set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "CultureTextText"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Text { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextEdit,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextEditDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.CultureTextInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(CultureText entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(CultureText entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				Text = entity.Text;
			}
 
			CultureId = entity.CultureId;
 
			Name = entity.Name;
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(CultureText entity, ControllerContext controllerContext);
        public void ToEntity(CultureText entity, ControllerContext controllerContext)
        {
			entity.Text = Text;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class ExceptionLogIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "StartTime"
			)]
		[Field(FieldWidth.W4)]
		public DateTime? StartTime { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "EndTime"
			)]
		[Field(FieldWidth.W4)]
		public DateTime? EndTime { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.ExceptionLogIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.ExceptionLogIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class SystemJobIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[TextBox] 
		public string Keyword { get; set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "SystemJobStatus"
			)]
		[Field(FieldWidth.W4)]
		public SystemJobStatus? Status { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.SystemJobIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.SystemJobIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
	public partial class SystemJobEditModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "SystemJobName"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public string Name { get; internal set; }
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "SystemJobStartTime"
			)]
		[Field(FieldWidth.W4)]
		[Mapping()]
		public DateTime StartTime { get; set; }
		partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext); 
		public Template CreateTemplate(ControllerContext controllerContext)
		{ 
			var template = new AdministrationSimpleEditTemplate
			{
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.SystemJobEdit,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.SystemJobEditDescription,
				FormTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.SystemJobInfo,
				Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
			};
			OnTemplate(template, controllerContext);
			return template;
		}
		partial void OnFromEntity(SystemJob entity, bool isPostback, ControllerContext controllerContext);
        public void FromEntity(SystemJob entity, bool isPostback, ControllerContext controllerContext)
        {
			if(!isPostback){
 
				StartTime = entity.StartTime;
			}
 
			Name = entity.Name;
			OnFromEntity(entity, isPostback, controllerContext);
		}
		partial void OnToEntity(SystemJob entity, ControllerContext controllerContext);
        public void ToEntity(SystemJob entity, ControllerContext controllerContext)
        {
			entity.StartTime = StartTime;
			OnToEntity(entity, controllerContext);
		}
 
	} 
	public partial class UserLoginFailedLogIndexModel  {
 
		[Display(
			ResourceType = typeof(Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources),
			Name = "Keyword"
			)]
		[Field(FieldWidth.W4)]
 
		[Link] 
		public int? UserId { get; set; }
		partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext);

		public Template CreateTemplate(ControllerContext controllerContext)
        {
            var query = GetDataSource(controllerContext);
            var template = new AdministrationSimpleListTemplate(query)
            { 
                Title = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.UserLoginFailedLogIndex,
                Description = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.UserLoginFailedLogIndexDescription,
                QueryPanelTitle = Moonlit.Mvc.Maintenance.Properties.MaintCultureTextResources.PanelQuery,
                DefaultSort = OrderBy,
                DefaultPageSize = PageSize,
                Criteria = new FieldsBuilder().ForEntity(this, controllerContext).Build(), 
            }; 
			
			OnTemplate (template, controllerContext);
            return template;
        }
 
	} 
}

