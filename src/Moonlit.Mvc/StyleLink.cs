namespace Moonlit.Mvc
{
    public class StyleLink
    {
        public string Id { get; set; }
        public string Href { get; set; }
        public StyleLinkMedia Media { get; set; }
        public IeVersionCriteria Criteria { get; set; }

        public StyleLink()
        {
            Media = StyleLinkMedia.Normal;
        }
    }
}