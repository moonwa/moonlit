using System.Collections.Generic;

namespace Moonlit
{
    public interface IPageOfList<T> : IPageOfList, IEnumerable<T>
    {
        List<T> Items { get; set; }
    }
}