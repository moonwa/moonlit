namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 短命令参数
    /// </summary>
    public class ShortPrefix : PrefixEntity
    {
        private string _key;
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public override string Key
        {
            get { return this._key; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortPrefix"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ShortPrefix(char key)
        {
            this._key = key.ToString();
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Char"/> to <see cref="ShortPrefix"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ShortPrefix(char    key)
        {
            return new ShortPrefix(key);
        }
        
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            string target = enumer.Current;
            if (target.StartsWith("/")
                || (target.StartsWith("-") && !target.StartsWith("--")))
            {
                if (target.Substring(1) == this.Key)
                {
                    enumer.MoveNext();
                    return true;
                }
            }
            return false;
        }
    }
}
