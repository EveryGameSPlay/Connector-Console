using System;
using Connector.Input;


namespace Connector.ConsoleRealizarions.Input
{
    public class ConsoleInputHandler : IInputHandler
    {
        /// <summary>
        /// Получаем введенную строку из консоли
        /// </summary>
        public string GetInputLine()
        {
            return Console.ReadLine();
        }
    }
}