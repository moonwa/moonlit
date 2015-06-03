namespace Moonlit.Mvc
{
    public static class MoonlitModelHelper
    {
        public static T GetObject<T>(this IMoonlitModel model)
        {
            return (T)model.GetObject(typeof(T).FullName);
        }
        public static void SetObject<T>(this IMoonlitModel model, T target)
        {
            model.SetObject(typeof(T).FullName, target);
        }
    }
}