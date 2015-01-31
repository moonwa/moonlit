using System.Collections.Generic;

namespace Moonlit.Text
{
    /// <summary>
    /// 字符串列表扩展方法集
    /// </summary>
    public static class StringListExtension
    {
        /// <summary>
        /// 将字符串数组中的空字符串清理掉
        /// </summary>
        /// <param name="target">The target.</param>
        public static void Trim(this List<string> target)
        {
            for (int i = target.Count - 1; i >= 0; i--)
            {
                if (target[i].Trim().Length == 0)
                {
                    target.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// 将字符串数组中的空字符串清理掉
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static List<string> Trim(IEnumerable<string> target)
        {
            List<string> returns = new List<string>();
            foreach (var s in target)
            {
                returns.Add(s);
            }
            return returns;
        }
    }
}
