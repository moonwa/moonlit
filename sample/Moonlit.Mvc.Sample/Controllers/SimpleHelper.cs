using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{
    public static class SimpleHelper
    {
        public static Site CreateSite()
        {
            return new Site
            {
                Title = "Moonlit Mvc 测试站点",
                CopyRight = "2015 © moonlit by moon.wa",
            };
        }
    }
}