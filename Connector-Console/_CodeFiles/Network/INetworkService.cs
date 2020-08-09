using System;
using System.Net;

namespace Connector.Network
{
    public interface INetworkService : IDisposable
    {
        /// <summary>
        /// IP адрес получателя
        /// </summary>
        IPAddress RecieverIp { get; }
        
        /// <summary>
        /// Порт получателя
        /// </summary>
        int RecieverPort { get; }
        
        /// <summary>
        /// Порт получения данных
        /// </summary>
        int ListenerPort { get; }
        
        
        
        /// <summary>
        /// Устанавливает IP адрес получателя
        /// </summary>
        void SetRecieverIp(string ip);
        
        /// <summary>
        /// Устанавливает порт получателя
        /// </summary>
        void SetRecieverPort(int port);
        
        /// <summary>
        /// Устанавливает IP адрес и порт получателя
        /// </summary>
        void SetRecieverInfo(string ip, int port);

        /// <summary>
        /// Устанавливает порт для получения данных
        /// </summary>
        void SetListenerPort(int port);
        
        /// <summary>
        /// Отправляет сообщение
        /// </summary>
        int Send(object message);

        /// <summary>
        /// Получение данных с порта прослушивания.
        /// Также возвращает адрес отправителя.
        /// Может вернуть null.
        /// </summary>
        void ListenString(Action<string> eventActivator);

        /// <summary>
        /// Закрывает сервис и останавливает все соединения
        /// </summary>
        void Close();
    }
}