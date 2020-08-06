using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace Connector.Kolyshev.Networks
{
    public static class NetworksSettings
    {

        /// <summary>
        /// IP получателя
        /// </summary>
        static IPAddress _IPReceiver { get; set; }

        /// <summary>
        /// Порт получателя
        /// </summary>
        static int _PortReceiver { get; set; }

        /// <summary>
        /// Порт отправителя
        /// </summary>
        static int _PortSender { get; set; }

        /// <summary>
        /// Установка IP получателя
        /// </summary>
        public static void SetIPReceiver(string ip)
        {
            _IPReceiver = IPAddress.Parse(ip);
        }

        /// <summary>
        /// Установка порта получателя
        /// </summary>
        public static void SetPortReceiver(int port)
        {
            _PortReceiver = port;
        }

        /// <summary>
        /// Установка порта отправителя
        /// </summary>
        public static void SetPortSender(int port)
        {
            _PortSender = port;
        }

        public static void SendCommand(string command)
        {
            UdpClient client = new UdpClient();
            client.Connect(_IPReceiver, _PortReceiver);
            byte[] data = Encoding.UTF8.GetBytes(command);
            client.Send(data, data.Length);
            client.Close();
        }
    }
}
