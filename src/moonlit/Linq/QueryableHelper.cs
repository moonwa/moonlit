using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Moonlit.Linq
{
    public static class QueryableHelper
    {
        public static T3 ToPaged<T, T2, T3>(this IQueryable<T> query, IPagedRequest request, T3 items, Func<T, T2> selector)
            where T3 : IPageOfList<T2>
        {
            items.ItemCount = query.Count();
            if (request.PageSize == 0)
            {
                items.Items = query.ToList().Select(selector).ToList();
                items.PageIndex = request.PageIndex;
                items.PageSize = request.PageSize;
                items.PageCount = 1;
                return items;
            }
            else
            {
                if (string.IsNullOrEmpty(request.OrderBy))
                    throw new Exception("OrderBy first before paging");
                query = DynamicQueryable.OrderBy(query, request.OrderBy.Replace("+", " ").Replace(":", " "));

                var start = (request.PageIndex - 1) * request.PageSize;
                items.Items = query.Skip(start).Take(request.PageSize).ToList().Select(selector).ToList();
                items.PageIndex = request.PageIndex;
                items.PageSize = request.PageSize;
                items.PageCount = PageOfList<T>.GetPageCount(items.ItemCount, items.PageSize);
                return items;
            }
        }
        public static IPageOfList<T> ToPaged<T>(this IQueryable<T> query, IPagedRequest request)
        {
            PageOfList<T> items = new PageOfList<T>();
            items.ItemCount = query.Count();
            if (request.PageSize == 0)
            {
                items.Items = query.ToList();
                items.PageIndex = request.PageIndex;
                items.PageSize = request.PageSize;
                items.PageCount = 1;
                return items;
            }
            else
            {
                if (string.IsNullOrEmpty(request.OrderBy))
                    throw new Exception("OrderBy first before paging");
                query = DynamicQueryable.OrderBy(query, request.OrderBy.Replace("+", " ").Replace(":", " "));

                var start = (request.PageIndex - 1) * request.PageSize;
                items.Items = query.Skip(start).Take(request.PageSize).ToList() ;
                items.PageIndex = request.PageIndex;
                items.PageSize = request.PageSize;
                items.PageCount = PageOfList<T>.GetPageCount(items.ItemCount, items.PageSize);
                return items;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex">start: 1</param>
        /// <returns></returns>
        public static List<T> ToPaged<T>(this IQueryable<T> query, string orderBy, int pageSize, int pageIndex)
        {
            if (string.IsNullOrEmpty(orderBy))
                throw new Exception("OrderBy first before paging");
            query = DynamicQueryable.OrderBy(query, orderBy.Replace("+", " ").Replace(":", " "));
            var start = (pageIndex - 1) * pageSize;
            return query.Skip(start).Take(pageSize).ToList();
        }
        public static int GetPageCount(int totalItemCount, int pageSize)
        {
            if (pageSize == 0)
                return 1;
            return (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}
