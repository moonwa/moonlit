using System;
using System.Collections.Generic;

namespace Moonlit.Mvc.Sample
{
    public class MyTaskLoader : ITaskLoader
    {
        public Tasks LoadTasks()
        {
            return new Tasks
            {
                Items = new List<TaskItem>
                {
                    new TaskItem {Content = "Staff Meeting", ExpiredTime = DateTime.Today},
                    new TaskItem
                    {
                        Content = "New frontend layout",
                        ExpiredTime = DateTime.Today,
                        Status = TaskStatus.Completed
                    },
                    new TaskItem {Content = "Hire developers", ExpiredTime = DateTime.Today.AddDays(1)},
                    new TaskItem {Content = "Staff Meeting", ExpiredTime = DateTime.Today.AddDays(1)},
                    new TaskItem {Content = "New frontend layout", ExpiredTime = DateTime.Today.AddDays(4)},
                    new TaskItem {Content = "New frontend layout", ExpiredTime = DateTime.Today.AddDays(13)},
                },
                Count = 10,
                Url = "/tasks"
            };
        }
    }
}