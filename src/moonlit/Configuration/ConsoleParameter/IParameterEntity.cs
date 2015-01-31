using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{ 
    /// <summary>
    /// 参数实体接口
    /// </summary>
    public interface IParameterEntity : IParseEntity
    {
        /// <summary>
        /// 参证当前参数元素的正确性
        /// </summary>
        void Validate();
        /// <summary>
        /// 返回当前参数元素的描述
        /// </summary>
        /// <value>描述.</value>
        List<string> Description { get; }
        /// <summary>
        /// 获取实体名称
        /// </summary>
        /// <value>实体名称.</value>
        string Name { get; }
        /// <summary>
        /// 前缀名称.
        /// </summary>
        /// <value>The prefix key.</value>
        string PrefixKey { get; }
    }
}
