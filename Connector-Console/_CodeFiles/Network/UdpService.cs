using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Connector.Network
{
    public class UdpService : INetworkService
    {
        public UdpService()
        {
            recieverClient = new UdpClient();
        }
        
        private UdpClient recieverClient;

        private UdpClient listenerClient;
        
        public IPAddress RecieverIp { get; private set; }
        
        public int RecieverPort { get; private set; }
        
        public int ListenerPort { get; private set; }

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
            
            NetworkServiceLogger.LogWarning($"IP адрес \"{ip}\" имеет некоректный формат. (x*.x*.x*.x*)");
        }

        public void SetRecieverPort(int port)
        {
            RecieverPort = port;
            NetworkServiceLogger.Log("Порт получателя установлен.");
        }

        public void SetRecieverInfo(string ip, int port)
        {
            SetRecieverIp(ip);
            SetRecieverPort(port);
        }

        public void SetListenerPort(int port)
        {
            ListenerPort = port;

            listenerClient = new UdpClient(ListenerPort);
            
            NetworkServiceLogger.Log("Порт прослушивания установлен.");
        }

        public int Send(object message)
        {
            var str = message.ToString();

            var bytes = Encoding.UTF8.GetBytes(str);

            //var endPoint = new IPEndPoint(RecieverIp, RecieverPort);

            var bytesCount = recieverClient.Send(bytes, bytes.Length, endPoint);
            
            NetworkServiceLogger.Log("Данные отправлены");

            return bytesCount;
        }

        public byte[] Recieve(ref IPEndPoint ip)
        {
            if (listenerClient == null)
                return null;

            var bytes = listenerClient.Receive(ref ip);

            return bytes;
        }

        public string RecieveString()
        {
            IPEndPoint ip = null;
            var bytes = Recieve(ref ip);

            if(bytes == null)
                return String.Empty;
            
            return Encoding.UTF8.GetString(bytes);
        }

        public void Close()
        {
            recieverClient.Close();
            listenerClient.Close();
        }

        public void Dispose()
        {
            Close();
            
            recieverClient?.Dispose();
            listenerClient?.Dispose();
        }
    }
}