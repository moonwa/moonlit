namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 长命令前缀, 支持 
    /// </summary>
    public class LongPrefix : PrefixEntity
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
        /// Initializes a new instance of the <see cref="LongPrefix"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public LongPrefix(string key)
        {
            this._key = key;
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="LongPrefix"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator LongPrefix(string key)
        {
            return new LongPrefix(key);
        }
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            string target = enumer.Current;

            if (target.StartsWith("--"))
            {
                target = target.Substring(2);
            }
            else if (target.StartsWith("/"))
            {
                target = target.Substring(1);
            }
            if (target == enumer.Current)
            {
                return false;
            }

            if (target == this.Key)
            {
                enumer.MoveNext();
                return true;
            }
            return false;
        }
    }
}
 