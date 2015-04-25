namespace Moonlit.Mvc
{
    public static class Helpers
    {
        public static void AddCssClass(this ICssClass cssClass, string className)
        {
            cssClass.CssClass = cssClass.CssClass ?? "";
            cssClass.CssClass += " " + className;
        }
    }
}