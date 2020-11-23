using System;
using System.Threading;

namespace Connector.Network.TcpObjects
{
    /// <summary>
    /// Класс является Disposable оберткой над ReadWriteLockSlim
    /// </summary>
    public class ReadWriteWrap
    {
        /// <summary>
        /// Блокировщик потоков.
        /// Слим версия сначала работает некоторое время в цикле, а после уходит в спящий режим.
        /// </summary>
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
    
        /// <summary>
        /// Встать в очередь на чтение.
        /// </summary>
        public ReadLockToken ReadLock()
        {
            return new ReadLockToken(_lockSlim);
        }

        /// <summary>
        /// Встать в очередь на запись.
        /// </summary>
        public WriteLockToken WriteLock()
        {
            return new WriteLockToken(_lockSlim);
        }

        public void Dispose()
        {
            _lockSlim.Dispose();
        }
    }
    
    public struct WriteLockToken : IDisposable
    {
        private readonly ReaderWriterLockSlim _lockSlim;
        public WriteLockToken(ReaderWriterLockSlim lockSlim)
        {
            this._lockSlim = lockSlim;
            lockSlim.EnterWriteLock();
        }
        public void Dispose() => _lockSlim.ExitWriteLock();
    }

    public struct ReadLockToken : IDisposable
    {
        private readonly ReaderWriterLockSlim _lockSlim;
        public ReadLockToken(ReaderWriterLockSlim lockSlim)
        {
            this._lockSlim = lockSlim;
            lockSlim.EnterReadLock();
        }
        public void Dispose() => _lockSlim.ExitReadLock();
    }
}