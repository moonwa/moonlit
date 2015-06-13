using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Sample
{
    public class ClipOneTheme : Theme
    {
        public ClipOneTheme()
        {
            this.RegisterControl(typeof(List), ThemeName + "/Controls/List");
            this.RegisterControl(typeof(TextBox), ThemeName + "/Controls/TextBox");
            this.RegisterControl(typeof(PasswordBox), ThemeName + "/Controls/PasswordBox");
            this.RegisterControl(typeof(Button), ThemeName + "/Controls/Button");
            this.RegisterControl(typeof(Field), ThemeName + "/Controls/Field");
            this.RegisterControl(typeof(Link), ThemeName + "/Controls/Link");
            this.RegisterControl(typeof(Pager), ThemeName + "/Controls/Pager");
            this.RegisterControl(typeof(ControlCollection), ThemeName + "/Controls/ControlCollection");
            this.RegisterControl(typeof(Form), ThemeName + "/Controls/Form");
            this.RegisterControl(typeof(Panel), ThemeName + "/Controls/Panel");
            this.RegisterControl(typeof(Table), ThemeName + "/Controls/Table");
            this.RegisterControl(typeof(Literal), ThemeName + "/Controls/Literal");
            this.RegisterControl(typeof(DropdownList), ThemeName + "/Controls/DropdownList");
            this.RegisterControl(typeof(CheckBox), ThemeName + "/Controls/CheckBox");
        }
        private const string ThemeName = "clip-one";
        protected override void PreRequest(RequestContext requestContext)
        {
            UrlHelper url = new UrlHelper(requestContext);
            var styles = Styles.Current;
            var scripts = Scripts.Current;
            if (styles != null)
            {
                styles.RegisterStyleLink("plugins:bootstrap", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap/css/bootstrap.min.css") });
                styles.RegisterStyleLink("plugins:font-awesome", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/font-awesome/css/font-awesome.min.css") });
                styles.RegisterStyleLink("fonts:style", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/fonts/style.css") });
                styles.RegisterStyleLink("css:main", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/main.css") });
                styles.RegisterStyleLink("css:main-responsive", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/main-responsive.css") });
                styles.RegisterStyleLink("plugins:iCheck", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/iCheck/skins/all.css") });
                styles.RegisterStyleLink("plugins:bootstrap-colorpalette", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css") });
                styles.RegisterStyleLink("plugins:perfect-scrollbar", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/perfect-scrollbar.css") });
                styles.RegisterStyleLink("css:theme_light", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/theme_light.css"), Id = "skin_color" });
                styles.RegisterStyleLink("css:print", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/css/print.css"), Media = StyleLinkMedia.Print });

                styles.RegisterStyleLink("plugins:font-awesome-ie7", new StyleLink() { Href = url.Content("~/assets/" + ThemeName + "/plugins/font-awesome/css/font-awesome-ie7.min.css"), Criteria = new IeVersionCriteria(7, IeVersionCriteriaOperator.Eq) });
            }

            //
            if (scripts != null)
            {
                scripts.RegisterScript("plugins:respond", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/respond.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
                scripts.RegisterScript("plugins:excanvas", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/excanvas.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
                scripts.RegisterScript("plugins:jquery", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jQuery-lib/1.10.2/jquery.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Lt) });
                scripts.RegisterScript("plugins:jquery", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jQuery-lib/2.0.3/jquery.min.js"), Criteria = new IeVersionCriteria(9, IeVersionCriteriaOperator.Gte) });
                scripts.RegisterScript("plugins:jquery-ui", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js") });
                scripts.RegisterScript("plugins:bootstrap", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap/js/bootstrap.min.js") });
                scripts.RegisterScript("plugins:bootstrap-hover-dropdown", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js") });
                scripts.RegisterScript("plugins:blockUI", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/blockUI/jquery.blockUI.js") });
                scripts.RegisterScript("plugins:perfect-scrollbar:mousewheel", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/jquery.mousewheel.js") });
                scripts.RegisterScript("plugins:perfect-scrollbar", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/perfect-scrollbar/src/perfect-scrollbar.js") });
                scripts.RegisterScript("plugins:less", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/less/less-1.5.0.min.js") });
                scripts.RegisterScript("plugins:jquery-cookie", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/jquery-cookie/jquery.cookie.js") });
                scripts.RegisterScript("plugins:bootstrap-colorpalette", new Script() { Src = url.Content("~/assets/" + ThemeName + "/plugins/bootstrap-colorpalette/js/bootstrap-colorpalette.js") });
                scripts.RegisterScript("js:main", new Script() { Src = url.Content("~/assets/" + ThemeName + "/js/main.js") });
            }
        }

        public override string Name
        {
            get { return "clip-one"; }
        }
    }
}