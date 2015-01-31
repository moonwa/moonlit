using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;

namespace Moonlit
{
    public class IteratorScope : IDisposable
    {
        public void Next()
        {
            Index++;
        }
        public int Index { get; private set; }

        public static IteratorScope Current
        {
            get { return (IteratorScope)CallContext.GetData(CallContextKey); }
        }

        private IteratorScope _backupScope;
        private const string CallContextKey = "IteratorScope";
        public IteratorScope()
        {
            _backupScope = Current;
            if (_backupScope != null)
                _backupScope.Suspend();
            Resume();
        }

        private void Suspend()
        {
            CallContext.SetData(CallContextKey, null);
        }

        private void Resume()
        {
            CallContext.SetData(CallContextKey, this);
        }

        public void Dispose()
        {
            CallContext.SetData(CallContextKey, null);
            if (_backupScope != null)
                _backupScope.Resume();
            _backupScope = null;
        }
    }
}
