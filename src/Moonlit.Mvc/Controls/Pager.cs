namespace Moonlit.Mvc.Controls
{
    public class Pager : Control
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int ItemCount { get; set; }
        public string OrderBy { get; set; }
    }
}