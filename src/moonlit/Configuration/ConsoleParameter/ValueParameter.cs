using System;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 定义包含值的参数, 定义格式如 /x=a, --f=xx, /x:a, --f:x
    /// </summary>
    public class ValueParameter : Parameter
    {
        /// <summary>
        /// 所定义的值.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueParameter"/> class.
        /// </summary>
        /// <param name="name">参数名称.</param>
        /// <param name="discription">参数描述.</param>
        /// <param name="prefixs">命令前缀</param>
        /// <param name="defaultValue">缺省值</param>
        public ValueParameter(string name, string discription, string defaultValue, params PrefixEntity[] prefixs)
            : this(name,discription, true,prefixs)
        {
            this.Value = defaultValue;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueParameter"/> class.
        /// </summary>
        /// <param name="name">参数名称.</param>
        /// <param name="discription">参数描述.</param>
        /// <param name="required">是否必填参数.</param>
        /// <param name="prefixs">命令前缀</param>
        public ValueParameter(string name, string discription, bool required, params PrefixEntity[] prefixs)
            : base(name, discription, required, CreatePrefixEntity(name, prefixs))
        {
        }
        static PrefixEntity[] CreatePrefixEntity(string name, PrefixEntity[] prefixs)
        {
            if (prefixs == null || prefixs.Length == 0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("当没有使用参数时，名称不可为空");
                }
                return new PrefixEntity[] { (SplitPrefix) name[0].ToString().ToLower(), (ShortPrefix)char.ToLower( name[0]), (LongOrSplitPrefix)name };
            }
            return prefixs;
        }
        /// <summary>
        /// 该参数是否使用过
        /// </summary>
        /// <value>如果使用过, 返回 <c>true</c> 否则返回 <c>false</c>.</value>
        public override bool Defined
        {
            get {  return this.Value != null; }
        }
        /// <summary>
        /// 当前参数元素验证其值的正确性
        /// </summary>
        protected override void OnValidate()
        {
        }
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            if (enumer.End)
            {
                throw new ArgumentNullException(string.Format("{0} 未定义值", this.Name));
            }
            this.Value = enumer.Current;
            enumer.MoveNext();
            return true;
        }
    }
}
