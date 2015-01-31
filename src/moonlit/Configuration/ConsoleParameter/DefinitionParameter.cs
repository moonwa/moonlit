using System;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 定义类型的参数, 支持 /x, --x, -x 写法
    /// </summary>
    public class DefinitionParameter : Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionParameter"/> class.
        /// </summary>
        /// <param name="name">参数名.</param>
        /// <param name="discription">参数描述.</param>
        /// <param name="prefixs">命令前缀</param>
        public DefinitionParameter(string name, string discription, params PrefixEntity[] prefixs)
            : base(name, discription, false, CreatePrefixEntity( name, prefixs))
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
                return new PrefixEntity[] { (ShortPrefix)(char.ToLower( name[0])), (LongOrSplitPrefix)name };
            }
            return prefixs;
        }
        private bool IsDefined { get; set; }
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            this.IsDefined = true;
            return true;
        }

        /// <summary>
        /// 当前参数元素验证其值的正确性
        /// </summary>
        protected override void OnValidate()
        {
        }

        /// <summary>
        /// 该参数是否使用过
        /// </summary>
        /// <value>如果使用过, 返回 <c>true</c> 否则返回 <c>false</c>.</value>
        public override bool Defined
        {
            get { return this.IsDefined; }
        }

    }
}
