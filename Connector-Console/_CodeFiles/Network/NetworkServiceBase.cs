using Connector.Network;
using Connector.Printer;
using Gasanov.Extensions.Float;
using System;
using System.Collections.Generic;
using System.Net;

namespace Connector.Network
{
    public abstract class NetworkServiceBase : INetworkService
    {
        public IPAddress RecieverIp { get; protected set; }

        public int RecieverPort { get; protected set; }

        public int ListenerPort { get; protected set; }

        public abstract void Close();

        public abstract void Dispose();

        public abstract void ListenString(Action<string> eventActivator);

        /// <summary>
        /// Создание конечной точки.
        /// Если один из аргументов неправильный, то вернет null.
        /// </summary>
        public IPEndPoint CreateEndPoint(IPAddress ip, int port)
        {
            if (ip == null)
                return null;

            if (port < 0)
                return null;

            return new IPEndPoint(ip, port);
        }

        public abstract int Send(object message);

        /// <summary>
        /// Уведомляет о неуспешной отправке сообщения
        /// </summary>
        protected void MessageNotSended()
        {
            NetworkServiceLogger.LogWarning("Сообщение не отправлено.");
        }

        /// <summary>
        /// Уведомляет о неполной отправке сообщения.
        /// </summary>
        protected void MessageNotFullySended(int actual, int expected)
        {
            var percent = ((float)actual / (float)expected)*100;
            NetworkServiceLogger.LogWarning($"Сообщение доставлено на {percent.ToString(1)}%");
        }

        /// <summary>
        /// Уведомляет о полной отправке сообщения.
        /// </summary>
        protected void MessageFullySended()
        {
            NetworkServiceLogger.LogWarning("Сообщение доставлено полностью.");
        }

        /// <summary>
        /// Проверка на количество отправленных байт.
        /// </summary>
        protected bool IsFullSended(byte[] data, int actual)
        {
            if (actual == data.Length)
                return true;

            return false;
        }

        public abstract void SetListenerPort(int port);

        public virtual void SetRecieverIp(string ip)
        {
            IPAddress parsedIp;
            var isParsed = IPAddress.TryParse(ip, out parsedIp);

            if (isParsed == true)
            {
                RecieverIp = parsedIp;
                NetworkServiceLogger.Log($"Адрес получателя {RecieverIp} установлен.");

                return;
            }

            NetworkServiceLogger.LogWarning($"IP адрес \"{ip}\" имеет некорректный формат. (x*.x*.x*.x*)");
        }

        public virtual void SetRecieverPort(int port)
        {
            RecieverPort = port;

            NetworkServiceLogger.Log($"Порт получателя {RecieverPort} установлен");
        }

        public virtual void SetRecieverInfo(string ip, int port)
        {
            SetRecieverIp(ip);
            SetRecieverPort(port);
        }
    }
}
