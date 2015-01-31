using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.DirectoryServices
{
    public partial class IisDirectoryEntry
    {
        public class Enumerator : IEnumerator<IisDirectoryEntry>
        {
            System.Collections.IEnumerator m_baseEnumer;
            public Enumerator(System.Collections.IEnumerator baseEnumer)
            {
                this.m_baseEnumer = baseEnumer;
            }
            #region IEnumerator<IISDirectoryEntry> 成员
            public IisDirectoryEntry Current
            {
                get 
                {
                    return new IisDirectoryEntry((System.DirectoryServices.DirectoryEntry) this.m_baseEnumer.Current);
                    // return (IISDirectoryEntry)this.m_baseEnumer.Current;
                }
            }

            #endregion

            #region IDisposable 成员

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator 成员

            object System.Collections.IEnumerator.Current
            {
                get 
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                return this.m_baseEnumer.MoveNext();
            }

            public void Reset()
            {
                this.m_baseEnumer.Reset();
            }

            #endregion
        }
    }
}
