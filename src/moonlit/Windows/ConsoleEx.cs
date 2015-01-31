using System;

namespace Moonlit.Windows
{
    /// <summary>
    /// 版权所有: 版权所有(C) 2007，联友科技
    /// 内容摘要: 控制台输出
    /// 完成日期：2007年7月31日
    /// 版    本：V 1.0 
    /// 作    者：詹张
    /// </summary>
    public static class ConsoleEx
    {
        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        /// <param name="format">格式</param>
        /// <param name="objs">参数</param>
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
        /// 输出文本
        /// </summary>
        /// <param name="foreColor">前景色</param>
        /// <param name="format">格式</param>
        /// <param name="objs">参数</param>
        public static void WriteLine(ConsoleColor foreColor, string format, params object[] objs)
        {
            Write(foreColor, Console.BackgroundColor, format + Environment.NewLine, objs);
        }

        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="foreColor">前景色</param>
        /// <param name="format">格式</param>
        /// <param name="objs">参数</param>
        public static void Write(ConsoleColor foreColor, string format, params object[] objs)
        {
            Write(foreColor, Console.BackgroundColor, format, objs);
        }

        /// <summary>
        /// 输出一行文本
        /// </summary>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        /// <param name="format">格式</param>
        /// <param name="objs">参数</param>
        public static void WriteLine(ConsoleColor foreColor, ConsoleColor backColor, string format, params object[] objs)
        {
            Write(foreColor, backColor, format + Environment.NewLine, objs);
        }
    }
}