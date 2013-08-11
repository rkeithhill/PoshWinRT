using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace PoshWinRT
{
    public class AsyncOperationWrapper<T> : IDisposable
    {
        private IAsyncOperation<T> _asyncOperation;

        public AsyncOperationWrapper(object asyncOperation)
        {
            if (asyncOperation == null) throw new ArgumentNullException("asyncOperation");

            _asyncOperation = (IAsyncOperation<T>)asyncOperation;
        }

        ~AsyncOperationWrapper()
        {
            GC.SuppressFinalize(this);
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_asyncOperation == null) return;

            if (disposing)
            {
                _asyncOperation.Close();
                _asyncOperation = null;
            }
        }

        public AsyncStatus Status
        {
            get { return _asyncOperation.Status; }
        }

        public object AwaitResult()
        {
            return AwaitResult(-1);
        }

        public object AwaitResult(int millisecondsTimeout)
        {
            var task = _asyncOperation.AsTask();
            task.Wait(millisecondsTimeout);

            if (task.IsCompleted)
            {
                return task.Result;
            }
            else if (task.IsFaulted)
            {
                throw task.Exception;
            }
            else
            {
                throw new TaskCanceledException(task);
            }
        }
    }
}
