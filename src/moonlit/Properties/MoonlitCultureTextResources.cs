namespace Moonlit.Properties
{
    public static class MoonlitCultureTextResources
    { 
        public static ILanguageLoader LanguageLoader
        {
            get { return MoonlitDependencyResolver.Current.Resolve<ILanguageLoader>(); }
        }

        public static string FriendlyTimeInMunites
        {
            get { return LanguageLoader.Get("FriendlyTime.InMunites"); }
        }
        public static string FriendlyTimeInHours
        {
            get { return LanguageLoader.Get("FriendlyTime.InHours"); }
        }
        public static string FriendlyTimeInDays
        {
            get { return LanguageLoader.Get("FriendlyTime.InDays"); }
        }
        public static string FriendlyTimeInMonths
        {
            get { return LanguageLoader.Get("FriendlyTime.InMonths"); }
        }
    }
}