using Connector.Printer;
using System;
using System.Drawing;

namespace Connector.ConsoleRealizarions.Displays
{
    public class ConsoleDisplay : IDisplay
    {
        public void ShowMessage(object message)
        {
            Console.WriteLine(message);
        }
        public void ShowMessage(object message, Color color)
        {
            ShowMessage(message);
        }
        public void ShowMessage(object message, Color color, Color colorBack)
        {
            ShowMessage(message);
        }
    }
}