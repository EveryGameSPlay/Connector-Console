using System;

namespace Connector.Input
{
    public static class InputHandler
    {
        /// <summary>
        /// Получаем введенную строку из консоли
        /// </summary>
        /// <returns></returns>
        public static string GetInputLine()
        {
            return Console.ReadLine();
        }
    }
}