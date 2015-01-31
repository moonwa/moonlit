using System;

namespace Moonlit.Diagnostics
{ 
    public class Trace : IDisposable
    {
        public static Action<string> Output = delegate { };
        static Trace()
        {
            IndentWitdh = 2;
        }
        public static int IndentWitdh { get; set; }
        private DateTime _start;
        public Trace(string message)
        {
            _start = DateTime.Now;
            _message = message;

            Output(string.Format("{0}begin to {1}", new string(' ', IndentWitdh * (_layer++)), _message));
        }

        [ThreadStatic]
        private static int _layer;
        private readonly string _message;


        #region IDisposable Members

        /// <summary>
        /// IDisposable Members
        /// </summary>
        public void Dispose()
        {
            Output(string.Format("{0}end to {1}, escape ({2})", new string(' ', IndentWitdh * (--_layer)), _message, (DateTime.Now - _start).TotalMilliseconds));
        }

        #endregion
    }
}