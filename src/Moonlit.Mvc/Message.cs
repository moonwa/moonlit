using System;

namespace Moonlit.Mvc
{
    public class Message
    {
        public string Url { get; set; }
        public IUser User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }
}