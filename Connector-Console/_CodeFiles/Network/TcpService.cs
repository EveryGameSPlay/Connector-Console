using Connector.Network.Wrappers;
using Connector.Printer;
using System;
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
            RecieverIp = IPAddress.Any;
            RecieverPort = -1;
            ListenerPort = -1;

            BufferSize = 256;
        }

        public TcpService(int bufferSize) : this()
        {
            BufferSize = bufferSize;
        }

        private TcpClient senderClient;

        private TcpListenerWrapper listenerServer;



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
            if (senderClient.Connected)
            {
                senderClient.Close();
            }
            
            senderClient = new TcpClient();

            var endPoint = CreateEndPoint(RecieverIp, RecieverPort);

            if (endPoint == null)
                return false;
            
            // Подключаемся
            senderClient.Connect(endPoint);

            // Не подключились
            if (!senderClient.Connected)
                return false;
                
            return true;
        }

        public override void SetListenerPort(int port)
        {
            ListenerPort = port;

            listenerServer = new TcpListenerWrapper(IPAddress.Any, port);
            NetworkServiceLogger.Log($"Порт прослушивания {ListenerPort} установлен");

            listenerServer.Start();
            NetworkServiceLogger.Log("Прослушивание началось");
        }

        public override int Send(object message)
        {
            if (!TryConnect())
            {
                MessageNotSended();
                return 0;
            }

            var stream = senderClient.GetStream();
            
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
            if (listenerServer == null)
                return;

            if (!listenerServer.Active)
                return;

            try
            {
                // Пока есть ожидающие подключения
                while (listenerServer.Pending())
                {
                    var client = listenerServer.AcceptTcpClient();

                    var clientEntity = new ClientEntity(this, client, eventActivator);
                    
                    // Запуск потока с обработкой клиента
                    var clientThread = new Thread(new ThreadStart(() => clientEntity.Process()));
                    clientThread.Start();
                }
                
            }
            catch(SocketException ex)
            {
                NetworkServiceLogger.Log("Получение клиентов остановлено извне");
            }
            
            return;
        }


        public override void Close()
        {
            senderClient?.Close();
            listenerServer?.Stop();

            NetworkServiceLogger.Log("TCP протокол закрыт");
        }

        public override void Dispose()
        {
            Close();

            senderClient?.Dispose();
        }
    }
}
