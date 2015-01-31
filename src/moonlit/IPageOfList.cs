using System.Collections;

namespace Moonlit
{
    public interface IPageOfList : IEnumerable
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int PageCount { get; set; }
        int ItemCount { get; set; }
        string OrderBy { get; set; }
    }
}
