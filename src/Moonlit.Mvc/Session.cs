using System;

namespace Moonlit.Mvc
{
    public class Session
    {
        public Session()
        {
            CreationTime = DateTime.Now;
            ExpiredTime = CreationTime;
        }

        public string UserName { get; set; }
        public string AppId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string[] Privileges { get; set; }

        public string SessionId { get; set; }
    }
}