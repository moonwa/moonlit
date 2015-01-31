namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 
    /// </summary>
    public class LongOrSplitPrefix : ComplexPrefix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LongOrSplitPrefix"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public LongOrSplitPrefix(string key)
            :base(new LongPrefix(key), new SplitPrefix(key))
        {

        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="LongOrSplitPrefix"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator LongOrSplitPrefix(string key)
        {
            return new LongOrSplitPrefix(key);
        }
    }
}
