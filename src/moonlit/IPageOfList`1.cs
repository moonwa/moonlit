using System.Collections.Generic;

namespace Moonlit
{
    public interface IPageOfList<T> : IPageOfList 
    {
        List<T> Items { get; set; }
    }
}