using Connector.Network.Wrappers;
using Connector.Printer;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Connector.Network
{
    public class TcpService : NetworkServiceBase
    {

        public TcpService()
        {
            recieverClient = new TcpClient();

            RecieverIp = IPAddress.Any;
            RecieverPort = -1;
            ListenerPort = -1;

            BufferSize = 256;
        }

        public TcpService(int bufferSize) : this()
        {
            BufferSize = bufferSize;
        }

        private TcpClient recieverClient;

        private TcpListenerWrapper listenerServer;



        /// <summary>
        /// Размер буфера получаемых сообщений.
        /// Стандартное значение: 256 байт.
        /// </summary>
        public int BufferSize { get; set; }

        private bool TryConnect()
        {
            if (RecieverIp.Equals(IPAddress.Any) == true)
                return false;

            if (RecieverPort < 0)
                return false;

            if (!recieverClient.Connected)
                recieverClient.Connect(RecieverIp, RecieverPort);

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

            var str = message.ToString();

            NetworkStream stream = recieverClient.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(str);
            stream.Write(data, 0, data.Length);

            MessageFullySended();

            return data.Length;
        }


        // TODO: заменить Recieve на Listen и передавать в него активатор события Loop`а.
        // TODO: для каждого TCP соединения сделать отдельный поток
        public override byte[] Recieve(ref IPEndPoint ip)
        {

            if (listenerServer == null)
                return null;

            if (!listenerServer.Active)
                return null;

            try
            {
                NetworkServiceLogger.Log("Ожидание клиента");
                TcpClient client = listenerServer.AcceptTcpClient();
                NetworkServiceLogger.Log("Клиент получен");
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[BufferSize];
                int bytes = stream.Read(data, 0, data.Length);

                //stream.Close();
                //client.Close();

                return data.Take(bytes).ToArray();
            }
            catch(SocketException ex)
            {
                NetworkServiceLogger.Log("Получение клиентов остановлено извне");
            }

            return null;
        }


        public override void Close()
        {
            recieverClient?.Close();
            listenerServer?.Stop();

            NetworkServiceLogger.Log("TCP протокол закрыт");
        }

        public override void Dispose()
        {
            Close();

            recieverClient?.Dispose();
        }
    }
}
