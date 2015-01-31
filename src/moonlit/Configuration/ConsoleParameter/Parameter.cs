using System;
using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 参数实体基类
    /// </summary>
    public abstract class Parameter : ParseBase,  IParameterEntity
    {
        private string _name;
        /// <summary>
        /// 是否必填项
        /// </summary>
        private bool Required { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="name">参数名.</param>
        /// <param name="description">参数描述.</param>
        /// <param name="required">是否必填项.</param>
        /// <param name="prefixs">命令前缀</param>
        public Parameter(string name, string description, bool required, params PrefixEntity[] prefixs)
            : base(new ComplexPrefix(CreatePrefixEntity(name, prefixs)))
        {
            this._name = name;
            this.Required = required;
            this._discription.Add(description);
            this.PrefixKey = new ComplexPrefix(CreatePrefixEntity(name, prefixs)).Key;
        }
        static PrefixEntity[] CreatePrefixEntity(string name, PrefixEntity[] prefixs)
        {
            if (prefixs == null || prefixs.Length ==0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("当没有使用参数时，名称不可为空");
                }
                return new PrefixEntity[] { (LongPrefix)name };
            }
            return prefixs;
        }
        #region IArgumentEntity Members
        /// <summary>
        /// 当前参数元素验证其值的正确性
        /// </summary>
        protected abstract void OnValidate();


        /// <summary>
        /// 该参数是否使用过
        /// </summary>
        /// <value>如果使用过, 返回 <c>true</c> 否则返回 <c>false</c>.</value>
        public abstract bool Defined { get; }
        /// <summary>
        /// 参证当前参数元素的正确性
        /// </summary>
        public void Validate()
        {
            this.OnValidate();
            if (this.Required && !this.Defined )
            {
                throw new Exception(string.Format("参数 {0} 必填", this.Name));
            }
        }

        private List<string> _discription = new List<string>();

        /// <summary>
        /// 返回当前参数元素的描述
        /// </summary>
        /// <value>描述.</value>
        public virtual List<string> Description
        {
            get { return this._discription; }
        }

        /// <summary>
        /// 获取实体名称
        /// </summary>
        /// <value>实体名称.</value>
        public string Name
        {
            get { return this._name; }
        }
        /// <summary>
        /// 前缀名称.
        /// </summary>
        /// <value>The prefix key.</value>
        public string PrefixKey { get; private set; }
        #endregion
    }
}
