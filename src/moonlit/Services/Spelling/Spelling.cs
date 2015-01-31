namespace Moonlit.Services.Spelling
{ 
    public class Spelling : ISpelling
    {
        public string ToPlural(string word)
        {
            return word + "s";
        }
    }
}