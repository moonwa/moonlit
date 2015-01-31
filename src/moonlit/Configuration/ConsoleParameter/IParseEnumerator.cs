namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 参数枚举器接口
    /// </summary>
    public interface IParseEnumerator
    {
        /// <summary>
        /// 当前参数
        /// </summary>
        string Current { get; set; }
        /// <summary>
        /// 判断是否已经枚举结束
        /// </summary>
        bool End { get; }
        /// <summary>
        /// 迭代到下一个参数
        /// </summary>
        /// <returns></returns>
        bool MoveNext();
    }
}
