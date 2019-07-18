using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace PoshWinRT
{
    public abstract class AsyncInfoWrapper<T> : IDisposable where T : IAsyncInfo
    {
        protected T _asyncInfo;

        public AsyncInfoWrapper(object asyncInfo)
        {
            if (asyncInfo == null) throw new ArgumentNullException("asyncInfo");

            _asyncInfo = (T)asyncInfo;
        }

        ~AsyncInfoWrapper()
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
            if (_asyncInfo == null) return;

            if (disposing)
            {
                _asyncInfo.Close();
                _asyncInfo = default(T);
            }
        }

        public AsyncStatus Status
        {
            get { return _asyncInfo.Status; }
        }

    }

}

