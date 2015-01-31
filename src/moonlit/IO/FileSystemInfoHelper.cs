using System.IO;

namespace Moonlit.IO
{
    /// <summary>
    /// 文件系统扩展方法
    /// </summary>
    public static class FileSystemInfoHelper
    { 
        public static void ForceDelete(this FileSystemInfo fi)
        {
            fi.Attributes &=  ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
            fi.Delete();
        }
        /// <summary>
        /// 强制删除磁盘目录
        /// </summary>
        /// <param name="di">被删除的磁盘目录</param>
        /// <param name="recursive">是否替归</param>
        public static void ForceDelete(this DirectoryInfo di, bool recursive)
        {
            di.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
            di.Delete(recursive);
        }
    }
}
