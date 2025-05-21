using System;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class DisposableBase : IDisposable
    {
        private bool _disposed;

        public virtual void VerifyAccess() => ObjectDisposedException.ThrowIf(_disposed, this);

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeUnmanaged();
                if (disposing)
                {
                    DisposeManaged();
                }
                _disposed = true;
            }
        }

        protected virtual void DisposeManaged() { }
        protected virtual void DisposeUnmanaged() { }

        ~DisposableBase() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
