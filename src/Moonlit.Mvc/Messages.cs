using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Messages
    {
        public static Messages Current
        {
            get
            {
                Messages messages = HttpContext.Current.GetObject<Messages>();
                if (messages == null)
                {
                    var loader = DependencyResolver.Current.GetService<IMessageLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }
                    messages = loader.LoadMessages();
                    HttpContext.Current.SetObject(messages);
                }
                return messages;
            }
        }
        public IEnumerable<Message> Items { get; private set; }

        public Messages(IEnumerable<Message> tasks)
        {
            var items = tasks.ToList();
            Items = items;
            Count = items.Count();
        }

        public string Url { get; set; }
        public int Count { get; set; }
    }
}