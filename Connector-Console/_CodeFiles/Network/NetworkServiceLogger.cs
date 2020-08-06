using System.Drawing;
using Connector.Printer;

namespace Connector.Network
{
    public static class NetworkServiceLogger
    {
        public static void Log(object message)
        {
            Print.Log(message, Color.DarkBlue);
        }

        public static void LogWarning(object message)
        {
            Print.LogWarning(message);
        }

        public static void LogError(object message)
        {
            Print.LogError(message);
        }
    }
}