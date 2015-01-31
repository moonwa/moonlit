namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 参数实体基类
    /// </summary>
    public abstract class ParseBase : IParseEntity
    {
        private IParseEntity InnerParseEntity { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseBase"/> class.
        /// </summary>
        /// <param name="innerParseEntity">The inner entities.</param>
        public ParseBase(IParseEntity innerParseEntity)
        {
            this.InnerParseEntity = innerParseEntity;
        }
        #region IEntity Members
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">参数枚举器</param>
        /// <returns>如果解析成功返回 <c>true</c>，否则返回 <c>false</c></returns>
        public bool Parse(IParseEnumerator enumer)
        {
            if (this.InnerParseEntity == null)
            {
                return this.OnParse(enumer);
            }
            if (InnerParseEntity.Parse(enumer))
            {
                return this.OnParse(enumer);
            }
            return false;
        }
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected abstract bool OnParse(IParseEnumerator enumer);
        #endregion
    }
}
