using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Connector.Network.Wrappers
{
    public class TcpListenerWrapper : TcpListener
    {

        public TcpListenerWrapper(int port):base(port)
        {

        }
        public TcpListenerWrapper(IPEndPoint endPoint) : base(endPoint)
        {

        }
        public TcpListenerWrapper(IPAddress ip, int port) : base(ip, port)
        {

        }

        public new bool Active
        {
            get { return base.Active; }
        }

    }
}
