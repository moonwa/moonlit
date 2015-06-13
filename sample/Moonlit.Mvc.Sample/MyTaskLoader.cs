using System;
using System.Collections.Generic;

namespace Moonlit.Mvc.Sample
{
    public class MyTaskLoader : ITaskLoader
    {
        private Tasks _tasks;
        public Tasks Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    _tasks = new Tasks(new List<TaskItem>
                    {
                        new TaskItem {Text = "Staff Meeting", ATime = DateTime.Today},
                        new TaskItem {Text = "New frontend layout", ATime = DateTime.Today, Status = TaskStatus.Completed},
                        new TaskItem {Text = "Hire developers", ATime = DateTime.Today.AddDays(1)},
                        new TaskItem {Text = "Staff Meeting", ATime = DateTime.Today.AddDays(1)},
                        new TaskItem {Text = "New frontend layout", ATime = DateTime.Today.AddDays(4)},
                        new TaskItem {Text = "New frontend layout", ATime = DateTime.Today.AddDays(13)},
                    });
                }
                return _tasks;
            }
        }
    }
}