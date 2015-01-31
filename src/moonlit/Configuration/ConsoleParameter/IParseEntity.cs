namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 元素实体
    /// </summary>
    public interface IParseEntity 
    {
        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">参数枚举器</param>
        /// <returns>如果解析成功返回 <c>true</c>，否则返回 <c>false</c></returns>
        bool Parse(IParseEnumerator enumer);
    }    
}
