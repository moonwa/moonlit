
namespace Moonlit.Mvc
{
    public class FlashMessage
    {
        public string Text { get; set; }
        public FlashMessageType MessageType { get; set; }
        public static FlashMessage Current
        {
            get
            {
                var flash = MoonlitDependencyResolver.Current.Resolve<IFlash>();
                if (flash == null)
                {
                    return null;
                }
                return flash.Get<FlashMessage>();
            }
        }
    }
}