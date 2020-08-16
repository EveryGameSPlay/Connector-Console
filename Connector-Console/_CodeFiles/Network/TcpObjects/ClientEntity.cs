using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Connector.Network.TcpObjects
{
    public class ClientEntity
    {
        private readonly TcpService _service;
        private readonly TcpClient _client;
        
        /// <summary>
        /// Событие при получении данных
        /// </summary>
        private readonly Action<string> _eventActivator;

        /// <param name="eventActivator">Событие при получении данных</param>
        public ClientEntity(TcpService _service, TcpClient tcpClient, Action<string> eventActivator)
        {
            _client = tcpClient;
            _eventActivator = eventActivator;
        }

        /// <summary>
        /// Вызывается при завершении обработки
        /// </summary>
        public event Action<ClientEntity> OnProcessComplete;
        
        private Thread _workspaceThread;

        /// <summary>
        /// Обработка клиента
        /// </summary>
        public void Process()
        {
            if (_workspaceThread == null)
                _workspaceThread = Thread.CurrentThread;
            
            try
            {
                if (!_client.Connected)
                    return;

                var stream = _client.GetStream();

                var br = new BinaryReader(stream);

                // Чтение строки
                var str = br.ReadString();

                br.Close();
                stream.Close();
                _client.Close();

                if (string.IsNullOrEmpty(str) == true)
                    return;

                _eventActivator(str);
                OnProcessComplete(this);
            }
            catch (ObjectDisposedException ex)
            {
                NetworkServiceLogger.LogWarning($"Клиент был закрыт извне.");
            }
            catch(ThreadAbortException abortException)
            {
                NetworkServiceLogger.LogWarning($"Обработка клиента была завершена извне!");
            }
        }

        /// <summary>
        /// Убивает поток, в котором выполняется задача
        /// </summary>
        public void AbortProcessThread()
        {
            _workspaceThread.Abort();
        }

        /// <summary>
        /// Устанавливает ссылку на поток, в котором будет происходить обработка
        /// </summary>
        public void SetThreadReference(Thread thread)
        {
            _workspaceThread = thread;
        }
    }
}