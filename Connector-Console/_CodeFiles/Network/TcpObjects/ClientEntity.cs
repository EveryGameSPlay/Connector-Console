using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

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
        /// Обработка клиента
        /// </summary>
        public void Process()
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
        }
    }
}