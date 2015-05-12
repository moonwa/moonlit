using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            var clipOneTheme = new ClipOneTheme();
            Themes.Current.Register(clipOneTheme);
            Themes.Current.RegisterDefault(clipOneTheme);
            new MoonlitMvcRegister(RouteTable.Routes).Register();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
    public class ClipOneTheme : Theme
    {
        public ClipOneTheme()
        {
            this.RegisterControl(typeof(TextBox), ThemeName + "/Controls/TextBox");
            this.RegisterControl(typeof(Button), ThemeName + "/Controls/Button");
            this.RegisterControl(typeof(Field), ThemeName + "/Controls/Field");
            this.RegisterControl(typeof(Link), ThemeName + "/Controls/Link");
            this.RegisterControl(typeof(Pager), ThemeName + "/Controls/Pager");
            this.RegisterControl(typeof(ControlCollection), ThemeName + "/Controls/ControlCollection");
            this.RegisterControl(typeof(Form), ThemeName + "/Controls/Form");
            this.RegisterControl(typeof(Panel), ThemeName + "/Controls/Panel");
            this.RegisterControl(typeof(Table), ThemeName + "/Controls/Table");
            this.RegisterControl(typeof(Literal), ThemeName + "/Controls/Literal");
        }
        private const string ThemeName = "clip-one";
        protected override void PreRequest(MoonlitContext context, RequestContext requestContext)
        {
            UrlHelper url = new UrlHelper(requestContext);
            context.RegisterStyleLink("plugins:bootstrap", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap/css/bootstrap.min.css") });
            context.RegisterStyleLink("plugins:font-awesome", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/font-awesome/css/font-awesome.min.css") });
            context.RegisterStyleLink("fonts:style", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/fonts/style.css") });
            context.RegisterStyleLink("css:main", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/main.css") });
            context.RegisterStyleLink("css:main-responsive", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/main-responsive.css") });
            context.RegisterStyleLink("plugins:iCheck", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/iCheck/skins/all.css") });
            context.RegisterStyleLink("plugins:bootstrap-colorpalette", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css") });
            context.RegisterStyleLink("plugins:perfect-scrollbar", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/perfect-scrollbar.css") });
            context.RegisterStyleLink("css:theme_light", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/theme_light.css"), Id = "skin_color" });
            context.RegisterStyleLink("css:print", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/print.css"), Media = StyleLinkMedia.Print });

            context.RegisterStyleLink("plugins:font-awesome-ie7", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/font-awesome/css/font-awesome-ie7.min.css"), Criteria = new IeVersionCriteria(7, IeVersionCriteriaOperator.Eq) });

            //
            context.RegisterScript("plugins:respond", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/respond.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
            context.RegisterScript("plugins:excanvas", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/excanvas.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
            context.RegisterScript("plugins:jquery", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jQuery-lib/1.10.2/jquery.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
            context.RegisterScript("plugins:jquery", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jQuery-lib/2.0.3/jquery.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Gte) });
            context.RegisterScript("plugins:jquery-ui", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js") });
            context.RegisterScript("plugins:bootstrap", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap/js/bootstrap.min.js") });
            context.RegisterScript("plugins:bootstrap-hover-dropdown", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js") });
            context.RegisterScript("plugins:blockUI", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/blockUI/jquery.blockUI.js") });
            context.RegisterScript("plugins:perfect-scrollbar:mousewheel", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/jquery.mousewheel.js") });
            context.RegisterScript("plugins:perfect-scrollbar", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/perfect-scrollbar.js") });
            context.RegisterScript("plugins:less", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/less/less-1.5.0.min.js") });
            context.RegisterScript("plugins:jquery-cookie", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jquery-cookie/jquery.cookie.js") });
            context.RegisterScript("plugins:bootstrap-colorpalette", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-colorpalette/js/bootstrap-colorpalette.js") });
            context.RegisterScript("js:main", new Script() { Src = url.Content("~/assets/" + ThemeName + "/js/main.js") });
        }

        public override string Name
        {
            get { return "clip-one"; }
        }
    }
}
