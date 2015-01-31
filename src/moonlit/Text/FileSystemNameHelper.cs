using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Text
{
    /// <summary>
    /// �ļ���������
    /// </summary>
    public static class FileSystemNameHelper
    {
        /// <summary>
        /// ��Ŀ¼���������Ϊ�� / ��β����ʽ
        /// </summary>
        /// <param name="dir_name">Ҫ����Ŀ¼��</param>
        /// <returns></returns>
        public static string CompleteDirectoryName(string dir_name)
        {
            return (dir_name.EndsWith("/") ? dir_name : dir_name + "/");
        } 
    }
}
