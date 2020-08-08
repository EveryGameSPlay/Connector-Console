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
            recieverClient = new UdpClient();
        }
        
        private UdpClient recieverClient;

        private UdpClient listenerClient;

        public override void SetListenerPort(int port)
        {
            ListenerPort = port;

            listenerClient = new UdpClient(ListenerPort);

            NetworkServiceLogger.Log($"Порт прослушивания {ListenerPort} установлен");
        }

        public override int Send(object message)
        {

            if (RecieverIp == null)
            {
                MessageNotSended();
                return 0;
            }

            var str = message.ToString();

            var bytes = Encoding.UTF8.GetBytes(str);

            var endPoint = new IPEndPoint(RecieverIp, RecieverPort);

            var bytesCount = recieverClient.Send(bytes, bytes.Length, endPoint);
            

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

        public override byte[] Recieve(ref IPEndPoint ip)
        {
            if (listenerClient == null)
                return null;

            if (ip.Address == IPAddress.Any)
                ip = null;

            var bytes = listenerClient.Receive(ref ip);

            return bytes;
        }

        public override void Close()
        {
            recieverClient?.Close();
            listenerClient?.Close();

            NetworkServiceLogger.Log("UDP протокол закрыт");
        }

        public override void Dispose()
        {
            Close();
            
            recieverClient?.Dispose();
            listenerClient?.Dispose();
        }
    }
}