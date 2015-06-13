using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Tasks
    {
        public static Tasks Current
        {
            get
            {
                Tasks tasks = HttpContext.Current.GetObject<Tasks>();
                if (tasks == null)
                {
                    var loader = DependencyResolver.Current.GetService<ITaskLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }
                    tasks = loader.LoadTasks();
                    HttpContext.Current.SetObject(tasks);
                }
                return tasks;
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