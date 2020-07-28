using System;
using System.Drawing;

namespace Connector.Printer
{
    public interface IDisplay
    {
        /// <summary>
        /// Показывает сообщение
        /// </summary>
        void ShowMessage(object message);
        void ShowMessage(object message, Color color);
        void ShowMessage(object message, Color color, Color colorBack);
    }
}