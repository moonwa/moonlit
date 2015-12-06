using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.IO
{
    public static class FileInfoHelper
    {
        /// <summary>
        /// 移除文件属性
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="attributes"></param>
        public static void RemoveAttribute(this FileInfo fileInfo, FileAttributes attributes)
        {
            fileInfo.Attributes = (FileAttributes)((ulong)fileInfo.Attributes & (0xFFFFFFFF ^ (ulong)attributes));
        }
        /// <summary>
        /// 读取文件中的字符串
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> ReadAsStringAsync(this FileInfo fileInfo, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;
            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fs, encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
        /// <summary>
        /// 将文本写入文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task WriteAsStringAsync(this FileInfo fileInfo, string text, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;
            using (var fs = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fs, encoding))
                {
                    await writer.WriteAsync(text);
                }
            }
        }
        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        /// <param name="fileInfo"></param> 
        /// <returns></returns>
        public static FileInfo EntureDirectoryExist(this FileInfo fileInfo)
        {
            DirectoryHelper.EnsureFromFile(fileInfo.FullName);
            return fileInfo;
        }
    }
}
