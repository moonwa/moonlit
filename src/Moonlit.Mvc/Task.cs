using System;
using System.Globalization;
using Moonlit.Runtime.Serialization;

namespace Moonlit.Mvc
{
    public class TaskItem
    {
        public string Text { get; set; }
        public DateTime ATime { get; set; }
        public TaskStatus Status { get; set; }
    }

    public enum TaskStatus
    {
        Init, Read, Completed
    }
}