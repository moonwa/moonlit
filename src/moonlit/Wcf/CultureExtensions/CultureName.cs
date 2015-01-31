namespace Moonlit.Wcf.CultureExtensions
{
    public class CultureName
    {
        public const string LocalName = "__CultureName";
        public const string Ns = "ns:CultureName";
        public string Name { get; set; }
        public CultureName(string name)
        {
            Name = name;
        }

        public CultureName()
        {

        }
    }
}