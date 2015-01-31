using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Text
{
    /// <summary>
    /// 文件名帮助类
    /// </summary>
    public static class FileSystemNameHelper
    {
        /// <summary>
        /// 将目录名补充完成为以 / 结尾的形式
        /// </summary>
        /// <param name="dir_name">要填充的目录名</param>
        /// <returns></returns>
        public static string CompleteDirectoryName(string dir_name)
        {
            return (dir_name.EndsWith("/") ? dir_name : dir_name + "/");
        } 
    }
}
