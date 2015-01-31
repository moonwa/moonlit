using System;

namespace Moonlit
{ 
    public abstract class DisposableBase : IDisposable {
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            this.disponse(true);
        }
        /// <summary>
        /// Disponses the specified disponsing.
        /// </summary>
        /// <param name="disposing">是否正在调用 Dispose 方法.</param>
        private void disponse(bool disposing) {
            this.OnDisponse(disposing);
            if (disposing) {
                GC.SuppressFinalize(this);
            }

        }

        #endregion

        /// <summary>
        /// Called when [disponse].
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected abstract void OnDisponse(bool disposing);

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBase() {
            this.disponse(false);
        }
    }
}