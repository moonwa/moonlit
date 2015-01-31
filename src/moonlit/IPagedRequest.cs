namespace Moonlit
{
    public interface IPagedRequest
    {
        string OrderBy { get; set; }

        /// <summary>
        /// pageindex, start from 1
        /// </summary>
        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}