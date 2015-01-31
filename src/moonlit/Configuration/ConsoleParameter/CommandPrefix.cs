namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 命令前缀，如 /xx等
    /// </summary>
    public abstract class PrefixEntity : ParseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixEntity"/> class.
        /// </summary>
        public PrefixEntity()
            :base(null)
        {

        }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public abstract string Key { get; }
    }
}
