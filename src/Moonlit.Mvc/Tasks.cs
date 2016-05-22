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
                    var loader = MoonlitDependencyResolver.Current.Resolve<ITaskLoader>(false);
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
        public List<TaskItem> Items { get; set; }
        public string Url { get; set; }

        public int Count { get; set; }
    }
}