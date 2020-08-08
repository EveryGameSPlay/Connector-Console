using Connector.Network;
using Connector.Printer;
using Gasanov.Extensions.Float;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Connector.Network
{
    public abstract class NetworkServiceBase : INetworkService
    {
        public IPAddress RecieverIp { get; protected set; }

        public int RecieverPort { get; protected set; }

        public int ListenerPort { get; protected set; }

        public abstract void Close();

        public abstract void Dispose();

        public abstract byte[] Recieve(ref IPEndPoint ip);

        public virtual string RecieveString()
        {
            IPEndPoint ip = CreateEndPoint(IPAddress.Any, ListenerPort);
            if (ip == null)
                return String.Empty;

            var bytes = Recieve(ref ip);

            if (bytes == null)
                return String.Empty;

            return Encoding.UTF8.GetString(bytes);
        }

        public IPEndPoint CreateEndPoint(IPAddress ip, int port)
        {
            if (ip == null)
                return null;

            if (port < 0)
                return null;

            return new IPEndPoint(ip, port);
        }

        public abstract int Send(object message);

        protected void MessageNotSended()
        {
            NetworkServiceLogger.LogWarning("Сообщение не отправлено.");
        }

        protected void MessageNotFullySended(int actual, int expected)
        {
            var percent = ((float)actual / (float)expected)*100;
            NetworkServiceLogger.LogWarning($"Сообщение доставлено на {percent.ToString(1)}%");
        }

        protected void MessageFullySended()
        {
            NetworkServiceLogger.LogWarning("Сообщение доставлено полностью.");
        }

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
