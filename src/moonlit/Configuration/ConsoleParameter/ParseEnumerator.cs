using System;
using System.Collections.Generic;
using Moonlit.Diagnostics;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 参数枚举器
    /// </summary>
    internal class ParseEnumerator : IParseEnumerator
    {
        string _current;
        List<string>.Enumerator _enumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseEnumerator"/> class.
        /// </summary>
        /// <param name="enumer">The enumer.</param>
        public ParseEnumerator(List<string>.Enumerator enumer)
        {
            _enumer = enumer;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _enumer.Dispose();
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// 当前参数
        /// </summary>
        /// <value></value>
        public string Current
        {
            get {
                Validator.Validate<Exception>(!this.End, "不可在编历结束后访问当前元素");
                return _current; 
            }
            set
            {
                Validator.Validate<Exception>(!this.End, "不可在编历结束后访问当前元素");
                _current = value;
            }
        }
        /// <summary>
        /// 获取是否已经枚举结束
        /// </summary>
        /// <value><c>true</c> if end; otherwise, <c>false</c>.</value>
        public bool End { get; private set; }

        /// <summary>
        /// 迭代到下一个参数
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            this.End = !this._enumer.MoveNext();
            if (this.End)
            {
                _current = null;
            }
            else
            {
                _current = this._enumer.Current;
            }
            return !End;
        }

        #endregion
    }
}
