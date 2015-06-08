using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Tasks
    {
        public static Tasks Current
        {
            get
            {
                var loader = DependencyResolver.Current.GetService<ITaskLoader>();
                if (loader == null)
                {
                    return null;
                }
                return loader.Tasks;
            }
        }
        public IEnumerable<TaskItem> Items { get; private set; }

        public Tasks(IEnumerable<TaskItem> tasks)
        {
            var items = tasks.ToList();
            Items = items;
            Count = items.Count();
        }

        public int Count { get; set; }
    }
}