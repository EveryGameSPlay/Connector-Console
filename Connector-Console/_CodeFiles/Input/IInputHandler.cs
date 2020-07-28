using System;
using System.Collections.Generic;
using System.Text;

namespace Connector.Input
{
    public interface IInputHandler
    {

        /// <summary>
        /// Получаем введенную строку
        /// </summary>
        string GetInputLine();

    }
}
