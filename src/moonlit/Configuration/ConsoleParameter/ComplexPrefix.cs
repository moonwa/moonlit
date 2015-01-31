using System;
using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 混合前缀
    /// </summary>
    public class ComplexPrefix : PrefixEntity
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public override string Key
        {
            get { return this.Prifexs[0].Key; }
        }
        private List<PrefixEntity> Prifexs { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexPrefix"/> class.
        /// </summary>
        /// <param name="prefixs">The prefixs.</param>
        public ComplexPrefix(params PrefixEntity[] prefixs)
        {
            if (prefixs == null || prefixs.Length == 0)
            {
                throw new ArgumentNullException("prefix", "请使用至少一个参数");
            }
            this.Prifexs = new List<PrefixEntity>(prefixs);
        }
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            foreach (var prifex in this.Prifexs)
            {
                if (prifex.Parse(enumer))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
