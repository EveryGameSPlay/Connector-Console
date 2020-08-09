using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Connector.Network
{
    public class UdpService : NetworkServiceBase
    {
        public UdpService()
        {
            senderClient = new UdpClient();
        }
        
        private UdpClient senderClient;

        private UdpClient listenerClient;

        public override void SetListenerPort(int port)
        {
            ListenerPort = port;

            listenerClient = new UdpClient(ListenerPort);

            NetworkServiceLogger.Log($"Порт прослушивания {ListenerPort} установлен");
        }

        public override int Send(object message)
        {
            var endPoint = CreateEndPoint(RecieverIp, RecieverPort);
            
            if (endPoint == null)
            {
                MessageNotSended();
                return 0;
            }

            var str = message.ToString();
            var bytes = Encoding.UTF8.GetBytes(str);

            // Отправка
            var bytesCount = senderClient.Send(bytes, bytes.Length, endPoint);
            
            if (!IsFullSended(bytes, bytes.Length))
            {
                MessageNotFullySended(bytesCount, bytes.Length);
            }
            else
            {
                MessageFullySended();
            }

            return bytesCount;
        }

        public override void ListenString(Action<string> eventActivator)
        {
            if (listenerClient == null)
                return;

            // С таким аргументом прослушка будет идти по любому подключению.
            // Порт был задан ранее.
            IPEndPoint endPoint = null;

            var bytes = listenerClient.Receive(ref endPoint);

            var str = Encoding.UTF8.GetString(bytes);

            if (string.IsNullOrEmpty(str) == true)
                return;
            
            // Активируем событие
            eventActivator(str);
        }

        public override void Close()
        {
            senderClient?.Close();
            listenerClient?.Close();

            NetworkServiceLogger.Log("UDP протокол закрыт");
        }

        public override void Dispose()
        {
            Close();
            
            senderClient?.Dispose();
            listenerClient?.Dispose();
        }
    }
}