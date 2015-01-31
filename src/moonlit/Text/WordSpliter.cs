namespace Moonlit.Text
{
    public class WordSpliter
    {
        public char Character { get; set; }
        public bool Include { get; set; }

        static WordSpliter()
        {
            End = new WordSpliter();
            Start = new WordSpliter();
        }
        public static WordSpliter End { get; private set; }
        public static WordSpliter Start { get; private set; }

        public WordSpliter()
        {
        }

        public WordSpliter(char character, bool include)
        {
            Character = character;
            Include = include;
        }
    }
}