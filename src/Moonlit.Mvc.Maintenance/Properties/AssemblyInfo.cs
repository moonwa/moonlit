using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Moonlit.Mvc;
using Moonlit.Mvc.Maintenance;
using Moonlit.Mvc.Maintenance.Properties;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Moonlit.Mvc.Demo.Web")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Moonlit.Mvc.Demo.Web")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: Mvc()]
[assembly: Sitemap("Main", IsDefault = true)]
[assembly: Sitemap("Profile", IsDefault = false)]
[assembly: SitemapNode(Name = "BasicData", Text = "BasicDataIndex", ResourceType = typeof(MaintCultureTextResources))]
[assembly: SitemapNode(Name = "Site", Text = "SiteMaintenance", ResourceType = typeof(MaintCultureTextResources))]
[assembly: SitemapNode(Name = "Devtools", Text = "DevTools", ResourceType = typeof(MaintCultureTextResources))]
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("39d53f7c-cbfb-476d-bdb0-d281028401dd")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.4.9")]
[assembly: AssemblyFileVersion("1.0.4.9")]


[assembly: Privilege(Text = "PrivilegeAdminUser", Name = MaintPrivileges.PrivilegeAdminUser, ResourceType = typeof(MaintCultureTextResources))]
[assembly: Privilege(Text = "PrivilegeRole", Name = MaintPrivileges.PrivilegeRole, ResourceType = typeof(MaintCultureTextResources))]
[assembly: Privilege(Text = "PrivilegeCulture", Name = MaintPrivileges.PrivilegeCulture, ResourceType = typeof(MaintCultureTextResources))]
[assembly: Privilege(Text = "PrivilegeCultureText", Name = MaintPrivileges.PrivilegeCultureText, ResourceType = typeof(MaintCultureTextResources))]
[assembly: Privilege(Text = "PrivilegeSite", Name = MaintPrivileges.PrivilegeSite, ResourceType = typeof(MaintCultureTextResources))]
[assembly: Privilege(Text = "PrivilegeSystemJob", Name = MaintPrivileges.PrivilegeSystemJob, ResourceType = typeof(MaintCultureTextResources))]
