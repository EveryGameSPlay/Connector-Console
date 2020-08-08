using System;
using System.Collections.Generic;
using System.Text;

namespace Gasanov.Extensions.Float
{
    public static class FloatExtensions
    {

        /// <summary>
        /// Самый эффективный способ округления числа и перевода его в строку
        /// </summary>
        /// <param name="f">Число</param>
        /// <param name="digits">Количество знаков после запятой</param>
        /// <returns></returns>
        public static string ToString(this float f, int digits)
        {
            return System.Math.Round(f, digits).ToString();
        }

    }
}
