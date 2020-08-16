using Connector.Network.Wrappers;
using Connector.Printer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Connector.Network.TcpObjects;

namespace Connector.Network
{
    public class TcpService : NetworkServiceBase
    {
        public TcpService()
        {
            clients = new List<ClientEntity>();
            _rwLock = new ReadWriteWrap();
            
            RecieverIp = IPAddress.Any;
            RecieverPort = -1;
            ListenerPort = -1;

            BufferSize = 256;
        }

        public TcpService(int bufferSize) : this()
        {
            BufferSize = bufferSize;
        }

        private TcpClient _senderClient;

        private TcpListenerWrapper _listenerServer;

        private List<ClientEntity> clients;

        private ReadWriteWrap _rwLock;

        /// <summary>
        /// Размер буфера получаемых сообщений.
        /// Стандартное значение: 256 байт.
        /// </summary>
        public int BufferSize { get; set; }

        
        /// <summary>
        /// Попытка подключения
        /// </summary>
        private bool TryConnect()
        {
            if (RecieverIp.Equals(IPAddress.Any) == true)
                return false;

            if (RecieverPort < 0)
                return false;

            // Если подключен, то закрываем соединение
            if (_senderClient.Connected)
            {
                _senderClient.Close();
            }
            
            _senderClient = new TcpClient();

            var endPoint = CreateEndPoint(RecieverIp, RecieverPort);

            if (endPoint == null)
                return false;
            
            // Подключаемся
            _senderClient.Connect(endPoint);

            // Не подключились
            if (!_senderClient.Connected)
                return false;
                
            return true;
        }

        public override void SetListenerPort(int port)
        {
            ListenerPort = port;

            _listenerServer = new TcpListenerWrapper(IPAddress.Any, port);
            NetworkServiceLogger.Log($"Порт прослушивания {ListenerPort} установлен");

            _listenerServer.Start();
            NetworkServiceLogger.Log("Прослушивание началось");
        }

        public override int Send(object message)
        {
            if (!TryConnect())
            {
                MessageNotSended();
                return 0;
            }

            var stream = _senderClient.GetStream();
            
            var bw = new BinaryWriter(stream);
            
            
            var str = message.ToString();
            // Запись в поток / отправка сообщения
            bw.Write(str);
            
            bw.Close();
            stream.Close();
            
            MessageFullySended();

            return Encoding.UTF8.GetByteCount(str);
        }

        public override void ListenString(Action<string> eventActivator)
        {
            if (_listenerServer == null)
                return;

            if (!_listenerServer.Active)
                return;

            try
            {
                // Пока есть ожидающие подключения
                while (_listenerServer.Pending())
                {
                    var client = _listenerServer.AcceptTcpClient();

                    var clientEntity = new ClientEntity(this, client, eventActivator);
                    clientEntity.OnProcessComplete += (x) => RemoveClient(x);
                    clients.Add(clientEntity);
                    
                    // Запуск потока с обработкой клиента
                    var clientThread = new Thread(new ThreadStart(() => clientEntity.Process()));
                    clientEntity.SetThreadReference(clientThread);
                    clientThread.Start();
                }
                
            }
            catch(SocketException ex)
            {
                NetworkServiceLogger.Log($"Получение клиентов остановлено извне {ex.ErrorCode}");
            }
            
            return;
        }

        /// <summary>
        /// Потокобезопасное удаление клиента из списка
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(ClientEntity client)
        {
            // Остальные потоки будут ожидать
            using (_rwLock.WriteLock())
            {
                clients.Remove(client);
            }
        }

        /// <summary>
        /// Потокобезопасное удаление из списка клиентов.
        /// Остановка потоков не производится.
        /// </summary>
        public void RemoveAllClients()
        {
            using (_rwLock.WriteLock())
            {
                for (var i = 0; i < clients.Count;i++)
                {
                    clients.Remove(clients[i]);
                }
            }
        }

        /// <summary>
        /// Потокобезопасная остановка всех потоков обработки клиентов
        /// </summary>
        public void AbortAllClients()
        {
            using (_rwLock.ReadLock())
            {
                for (var i = 0; i < clients.Count;i++)
                {
                    clients[i].AbortProcessThread();
                }
            }
        }


        public override void Close()
        {
            _senderClient?.Close();
            _listenerServer?.Stop();

            NetworkServiceLogger.Log("TCP протокол закрыт");
        }

        public override void Dispose()
        {
            Close();

            _senderClient?.Dispose();
        }
    }
}
