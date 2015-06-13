using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Notices
    {
        public static Notices Current
        {
            get
            {
                Notices notices = HttpContext.Current.GetObject<Notices>();
                if (notices == null)
                {
                    var loader = DependencyResolver.Current.GetService<INoticeLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }
                    notices = loader.Load();
                    HttpContext.Current.SetObject(notices);
                }
                return notices;
            }
        }
        public IEnumerable<Notice> Items { get; private set; }

        public Notices(IEnumerable<Notice> notices)
        {
            var items = notices.ToList();
            Items = items;
            Count = items.Count();
        }

        public string Url { get; set; }
        public int Count { get; set; }
    }
}