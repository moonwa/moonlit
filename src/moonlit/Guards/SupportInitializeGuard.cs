using System;
using System.ComponentModel;

namespace Moonlit.Guards
{
    public class SupportInitializeGuard : IDisposable
    {
        private readonly ISupportInitialize _supportInitialize;

        public SupportInitializeGuard(ISupportInitialize supportInitialize)
        {
            if (supportInitialize == null) throw new ArgumentNullException("supportInitialize");
            _supportInitialize = supportInitialize;

            supportInitialize.BeginInit();
        }

        public void Dispose()
        {
            _supportInitialize.EndInit();
        }
    }
}
