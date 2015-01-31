namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 切分形式的前缀，如 /active:yes效果
    /// </summary>
   public  class SplitPrefix : PrefixEntity
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
       /// Initializes a new instance of the <see cref="SplitPrefix"/> class.
       /// </summary>
       /// <param name="key">The key.</param>
       public SplitPrefix(string key)
       {
           this._key = key;
       }
       /// <summary>
       /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="SplitPrefix"/>.
       /// </summary>
       /// <param name="key">The key.</param>
       /// <returns>The result of the conversion.</returns>
       public static implicit operator SplitPrefix(string key)
       {
           return new SplitPrefix(key);
       }
       /// <summary>
       /// 解析输入参数
       /// </summary>
       /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
       /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            string target = enumer.Current;
            
            if (target.StartsWith("/"))
            {
                target = target.Substring(1);
            }
            if (target == enumer.Current)
            {
                return false;
            }

            int kvSplidPos = target.IndexOf("=");
            if (kvSplidPos == -1)
            {
                kvSplidPos = target.IndexOf(":");
            }
            if (kvSplidPos == -1)
            {
                return false;
            }

            string key = target.Substring(0, kvSplidPos);

            if (key != this.Key)
            {
                return false;
            }

            enumer.Current = enumer.Current.Substring(2 + key.Length);
            return true;
        }
    }
}
