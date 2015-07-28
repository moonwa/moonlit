using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public interface IRenderJudge
    {
        bool IsRender(ViewContext vc, object param);
    }
}