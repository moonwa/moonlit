namespace Moonlit
{
    public class HierarchicalTextValuePair : IValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string ParentValue { get; set; }
    }
}