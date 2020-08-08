using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Connector.Network
{
    public class TcpService : INetworkService
    {

        public TcpService()
        {
            recieverClient = new TcpClient();

            RecieverIp = IPAddress.Any;
            RecieverPort = -1;

            BufferSize = 256;
        }

        public TcpService(int bufferSize) : this()
        {
            BufferSize = bufferSize;
        }

        private TcpClient recieverClient;

        private TcpListener listenerServer;


        public IPAddress RecieverIp { get; private set; }

        public int RecieverPort { get; private set; }

        public int ListenerPort { get; private set; }

        /// <summary>
        /// Размер буфера получаемых сообщений.
        /// Стандартное значение: 256 байт.
        /// </summary>
        public int BufferSize { get; set; }


        public void SetRecieverIp(string ip)
        {
            IPAddress parsedIp;
            var isParsed = IPAddress.TryParse(ip, out parsedIp);

            if (isParsed == true)
            {
                RecieverIp = parsedIp;
                NetworkServiceLogger.Log("Адрес получателя установлен.");

                return;
            }

            NetworkServiceLogger.LogWarning($"IP адрес \"{ip}\" имеет некорректный формат. (x*.x*.x*.x*)");
        }

        public void SetRecieverPort(int port)
        {
            RecieverPort = port;

            NetworkServiceLogger.LogWarning("Порт получателя установлен");
        }

        public void SetRecieverInfo(string ip, int port)
        { 
            SetRecieverIp(ip);
            SetRecieverPort(port);
        }

        private bool TryConnect()
        {
            if (RecieverIp.Equals(IPAddress.Any) == true)
                return false;

            if (RecieverPort < 0)
                return false;

            recieverClient.Close();

            recieverClient.Connect(RecieverIp, RecieverPort);

            return true;
        }

        public void SetListenerPort(int port)
        {
            ListenerPort = port;

            listenerServer = new TcpListener(IPAddress.Any, port);
            NetworkServiceLogger.Log("Порт прослушивания установлен");

            listenerServer.Start();
            NetworkServiceLogger.Log("Прослушивание началось");
        }

        public int Send(object message)
        {
            if (!TryConnect())
                return 0;

            var str = message.ToString();

            NetworkStream stream = recieverClient.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(str);
            stream.Write(data, 0, data.Length);

            return data.Length;
        }

        public byte[] Recieve(ref IPEndPoint ip)
        {
            TcpClient client = listenerServer.AcceptTcpClient();

            NetworkStream stream = client.GetStream();

            byte[] data = new byte[BufferSize];
            int bytes = stream.Read(data, 0, data.Length);

            stream.Close();
            client.Close();

            return data.Take(bytes).ToArray();
        }

        public string RecieveString()
        {
            IPEndPoint ip = null;
            var bytes = Recieve(ref ip);

            if (bytes == null)
                return String.Empty;

            return Encoding.UTF8.GetString(bytes);
        }


        public void Close()
        {
            recieverClient.Close();
            listenerServer.Stop();
        }

        public void Dispose()
        {
            Close();

            recieverClient?.Dispose();
        }
    }
}
