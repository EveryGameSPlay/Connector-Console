using System;
using System.Collections.Generic;

namespace Connector.Commands
{
    public static class CommandsExtensions
    {
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<int> ParseInt( string[] args)
        {
            var parsedNumbers = new List<int>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                int parsedNumber = 0;
                var isParsed = int.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
        
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<float> ParseFloat( string[] args)
        {
            var parsedNumbers = new List<float>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                float parsedNumber = 0;
                var isParsed = float.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
        
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<double> ParseDouble( string[] args)
        {
            var parsedNumbers = new List<double>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                double parsedNumber = 0;
                var isParsed = double.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
        
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<int> ParseInt(this ICommand command, string[] args)
        {
            var parsedNumbers = new List<int>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                int parsedNumber = 0;
                var isParsed = int.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
        
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<float> ParseFloat(this ICommand command, string[] args)
        {
            var parsedNumbers = new List<float>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                float parsedNumber = 0;
                var isParsed = float.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
        
        /// <summary>
        /// Возвращает список чисел из аргументов
        /// </summary>
        public static List<double> ParseDouble(this ICommand command, string[] args)
        {
            var parsedNumbers = new List<double>();
            for (var i = 0; i < args.Length; i++)
            {
                var str = args[i];

                double parsedNumber = 0;
                var isParsed = double.TryParse(str, out parsedNumber);
                
                if(isParsed)
                    parsedNumbers.Add(parsedNumber);
            }

            return parsedNumbers;
        }
    }
}