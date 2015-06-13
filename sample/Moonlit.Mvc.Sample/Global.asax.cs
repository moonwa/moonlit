using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Sample.Controllers;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var builder = new ContainerBuilder();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<SessionCachingFlash>().As<IFlash>();
            builder.RegisterType<TestUserLoader>().As<IUserLoader>();
            builder.RegisterType<Authenticate>().AsSelf();


            var controllerAssemblies = new[] { typeof(MvcApplication).Assembly };
            builder.RegisterControllers(controllerAssemblies).PropertiesAutowired();
            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(controllerAssemblies);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.


            var clipOneTheme = new ClipOneTheme();
            Themes themes = new Themes();
            themes.Register(clipOneTheme);
            builder.Register(context => new DefaultThemeLoader("clip-one", themes)).As<IThemeLoader>();

            builder.RegisterType<ReflectionSitemapsLoader>().As<ISitemapsLoader>();
            builder.RegisterType<MyTaskLoader>().As<ITaskLoader>();
            builder.RegisterType<MyNoticeLoader>().As<INoticeLoader>();
            builder.RegisterType<MyMessageLoader>().As<IMessageLoader>();
            builder.RegisterType<TestUserLoader>().As<IUserLoader>();

            RequestMappings.Current.Register(RouteTable.Routes);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }

    public class MyMessageLoader : IMessageLoader
    {
        public Messages LoadMessages()
        {
            var user = new User
            {
                UserName = "Tom",
            };
            return new Messages(new List<Message>
            {
                new Message(){ Content = "新用户注册", Title = "Tom", User = user, CreationTime = DateTime.Now},
                new Message(){ Content = "新用户注册", Title = "Tom", User = user,  CreationTime = DateTime.Now},
                new Message(){ Content = "新用户注册", Title = "Tom", User = user,  CreationTime = DateTime.Now},
                new Message(){ Content = "新用户注册", Title = "Tom", User = user,  CreationTime = DateTime.Now},
                new Message(){ Content = "新用户注册", Title = "Tom", User = user,  CreationTime = DateTime.Now},
                new Message(){ Content = "新用户注册", Title = "Tom", User = user,  CreationTime = DateTime.Now},
            });
        }
    }

    public class MyNoticeLoader : INoticeLoader
    {
        public Notices Load()
        {
            return new Notices(new List<Notice>
            {
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
                new Notice(){ Content = "新用户注册", CreationTime = DateTime.Now},
            });
        }
    }

    public class TestUserLoader : IUserLoader
    {
        public IUserPrincipal GetUserPrincipal(string name)
        {
            return new TestUserPrincipal(name);
        }
    }

    public class TestUserPrincipal : IUserPrincipal
    {
        private readonly string _name;

        public TestUserPrincipal(string name)
        {
            _name = name;
            this.Privileges = new string[] { "edit" };
        }

        public bool IsInRole(string role)
        {
            if (Privileges == null)
            {
                return false;
            }
            return Privileges.Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase));
        }

        public IIdentity Identity
        {
            get
            {
                return new User()
                {
                    UserName = "Tom",
                };
            }
        }

        public string[] Privileges { get; set; }
        public string Avatar { get { return "#"; } }
        public string Name { get { return this.Identity.Name; } }
    }


}
