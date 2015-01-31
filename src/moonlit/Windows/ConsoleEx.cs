using System;

namespace Moonlit.Windows
{
    /// <summary>
    /// ��Ȩ����: ��Ȩ����(C) 2007�����ѿƼ�
    /// ����ժҪ: ����̨���
    /// ������ڣ�2007��7��31��
    /// ��    ����V 1.0 
    /// ��    �ߣ�ղ��
    /// </summary>
    public static class ConsoleEx
    {
        /// <summary>
        /// ����ı�
        /// </summary>
        /// <param name="foreColor">ǰ��ɫ</param>
        /// <param name="backColor">����ɫ</param>
        /// <param name="format">��ʽ</param>
        /// <param name="objs">����</param>
        public static void Write(ConsoleColor foreColor, ConsoleColor backColor, string format, params object[] objs)
        {
            ConsoleColor oldForeColor = Console.ForegroundColor;
            ConsoleColor oldBackColor = Console.BackgroundColor;

            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
            if (objs == null || objs.Length == 0)
                Console.Write(format);
            else
                Console.Write(format, objs);

            Console.ForegroundColor = oldForeColor;
            Console.BackgroundColor = oldBackColor;
        }

        /// <summary>
        /// ����ı�
        /// </summary>
        /// <param name="foreColor">ǰ��ɫ</param>
        /// <param name="format">��ʽ</param>
        /// <param name="objs">����</param>
        public static void WriteLine(ConsoleColor foreColor, string format, params object[] objs)
        {
            Write(foreColor, Console.BackgroundColor, format + Environment.NewLine, objs);
        }

        /// <summary>
        /// ����ı�
        /// </summary>
        /// <param name="foreColor">ǰ��ɫ</param>
        /// <param name="format">��ʽ</param>
        /// <param name="objs">����</param>
        public static void Write(ConsoleColor foreColor, string format, params object[] objs)
        {
            Write(foreColor, Console.BackgroundColor, format, objs);
        }

        /// <summary>
        /// ���һ���ı�
        /// </summary>
        /// <param name="foreColor">ǰ��ɫ</param>
        /// <param name="backColor">����ɫ</param>
        /// <param name="format">��ʽ</param>
        /// <param name="objs">����</param>
        public static void WriteLine(ConsoleColor foreColor, ConsoleColor backColor, string format, params object[] objs)
        {
            Write(foreColor, backColor, format + Environment.NewLine, objs);
        }
    }
}