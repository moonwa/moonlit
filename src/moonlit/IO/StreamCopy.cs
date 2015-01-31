using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Moonlit.IO
{
    /// <summary>
    /// 流复制任务
    /// </summary>
    public class StreamCopy
    {
        /// <summary>
        /// 是否执行 flush 操作
        /// </summary>
        public bool Flush { get; set; }

        /// <summary>
        /// 进度改变通知事件
        /// </summary>
        public event ProcessChangedEventHandler ProcessChanged;
        private int _bufferSize = 1024 * 1024;
        /// <summary>
        /// 获取 / 设置 缓冲区大小
        /// </summary>
        /// <value>缓冲区大小.</value>
        public int BufferSize
        {
            get { return this._bufferSize; }
            set { this._bufferSize = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamCopy"/> class.
        /// </summary>
        public StreamCopy()
        {
            this.Flush = false;
        }
        /// <summary>
        /// 复制流
        /// </summary>
        /// <param name="src">被复制的源.</param>
        /// <param name="dst">复制目标.</param>
        public void Copy(Stream src, Stream dst)
        {
            long oldPos = src.Position;
            try
            {
                int readCount = -1;
                byte[] buffer = new byte[BufferSize];

                while (readCount != 0)
                {
                    readCount = src.Read(buffer, 0, BufferSize);
                    dst.Write(buffer, 0, readCount);
                    if (!this.RaiseProcessChanged(src, oldPos))
                    {
                        return;
                    }
                }
                if (this.Flush)
                {
                    dst.Flush();
                }
            }
            finally
            {
                if (src.CanSeek)
                {
                    src.Position = oldPos;
                }
            }
        }

        private bool RaiseProcessChanged(Stream src, long oldPos)
        {
            if (this.ProcessChanged == null)
            {
                return true;
            }
            double total = 0;
            if (src.CanSeek)
            {
                total = src.Length - oldPos;
            }
            double current = src.Position - oldPos;
            ProcessChangedEventArgs e = new ProcessChangedEventArgs(current, total);
            this.ProcessChanged(e);
            return !e.Cancel;
        }
    }
}
