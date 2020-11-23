using Connector.Printer;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Connector.ConsoleRealizarions.Displays
{

    public class ConsoleColoredDisplay : IDisplay
    {
        private static readonly Dictionary<Color, ConsoleColor> _colors;

        static ConsoleColoredDisplay()
        {
            _colors = new Dictionary<Color, ConsoleColor>();

            _colors.Add(Color.White, ConsoleColor.White);
            _colors.Add(Color.Blue, ConsoleColor.Blue);
            _colors.Add(Color.Cyan, ConsoleColor.Cyan);
            _colors.Add(Color.DarkBlue, ConsoleColor.DarkBlue);
            _colors.Add(Color.DarkGray, ConsoleColor.DarkGray);
            _colors.Add(Color.DarkGreen, ConsoleColor.DarkGreen);
            _colors.Add(Color.Green, ConsoleColor.Green);
            _colors.Add(Color.DarkMagenta, ConsoleColor.DarkMagenta);
            _colors.Add(Color.Magenta, ConsoleColor.Magenta);
            _colors.Add(Color.DarkRed, ConsoleColor.DarkRed);
            _colors.Add(Color.Gold, ConsoleColor.DarkYellow);
            _colors.Add(Color.Gray, ConsoleColor.Gray);
            _colors.Add(Color.Red, ConsoleColor.Red);
            _colors.Add(Color.Yellow, ConsoleColor.Yellow);
            _colors.Add(Color.Black, ConsoleColor.Black);
        }

        public static ConsoleColor GetConsoledColor(Color color)
        {
            if (_colors.ContainsKey(color))
                return _colors[color];

            return ConsoleColor.Gray;
        }
        
        private Color _previousColor;
        private Color _previousColorBack;
        
        /// <summary>
        /// Выводит сообщение в консоль
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(object message)
        {
            Console.WriteLine(message);
        }
        
        /// <summary>
        /// Выводит сообщение в консоль с выбранным цветом
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void ShowMessage(object message, Color color)
        {
            _previousColor = color;

            Console.ForegroundColor = GetConsoledColor(color);
            ShowMessage(message);
            Console.ResetColor();
        }

        public void ShowMessage(object message, Color color, Color colorBack)
        {
            _previousColorBack = colorBack;

            Console.BackgroundColor = GetConsoledColor(colorBack);

            ShowMessage(message, color);
        }
    }
}