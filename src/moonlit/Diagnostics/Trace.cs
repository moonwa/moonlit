using System;
using System.IO;

namespace Moonlit.Diagnostics
{
    public class Trace : IDisposable
    {
        static Trace()
        {
            IndentWitdh = 2;
        }
        public static int IndentWitdh { get; set; }
        private DateTime _start;

        public Trace(string message)
            : this(message, System.Console.Out)
        {

        }
        public Trace(string message, TextWriter writer)
        {
            _start = DateTime.Now;
            _message = message;
            _writer = writer;
            _writer.WriteLine("{0}begin to {1}", new string(' ', IndentWitdh * (_layer++)), _message);
        }

        [ThreadStatic]
        private static int _layer;
        private readonly string _message;
        private readonly TextWriter _writer;

        #region IDisposable Members

        /// <summary>
        /// IDisposable Members
        /// </summary>
        public void Dispose()
        {
            _writer.WriteLine("{0}end to {1}, escape ({2})", new string(' ', IndentWitdh * (--_layer)), _message, (DateTime.Now - _start).TotalMilliseconds);
        }

        #endregion
    }
}