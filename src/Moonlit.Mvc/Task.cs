using System;
using System.Globalization;
using Moonlit.Runtime.Serialization;

namespace Moonlit.Mvc
{
    public class TaskItem
    {
        public string Content { get; set; }
        public DateTime ExpiredTime { get; set; }
        public DateTime CreationTime { get; set; }
        public TaskStatus Status { get; set; }
    }
}