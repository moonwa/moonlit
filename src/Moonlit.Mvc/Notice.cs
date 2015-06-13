using System;
using System.Web.Management;

namespace Moonlit.Mvc
{
    public class Notice
    {
        public string Url { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public NoticeType NoticeType { get; set; }
    }

    public enum NoticeType
    {
        Success,
        Warning,
        Primary
    }
}