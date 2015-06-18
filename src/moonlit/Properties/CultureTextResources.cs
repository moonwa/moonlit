namespace Moonlit.Properties
{
    public static class CultureTextResources
    {
        static CultureTextResources()
        {
            LanguageLoader = new NullLanguageLoader();
        }
        public static ILanguageLoader LanguageLoader { get; set; }
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