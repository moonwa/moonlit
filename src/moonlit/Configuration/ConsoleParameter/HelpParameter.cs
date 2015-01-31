namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 
    /// </summary>
    public class HelpParameter : DefinitionParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpParameter"/> class.
        /// </summary>
        public HelpParameter()
            :base("help", "显示帮助文档", (LongPrefix)"help", (ShortPrefix)'?')
        {
        }
    }
}
