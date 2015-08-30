using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public static class WebNameHelper
    {
        /// <summary>
        /// 创建数组访问索引 items[0].Count
        /// </summary>
        /// <param name="value"></param>
        /// <param name="prefix"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string MakeArrayIndexName(string prefix, string propertyName, int value)
        {
            return string.Format("{0}[{1}].{2}", prefix, value, propertyName);
        }
    }
}
