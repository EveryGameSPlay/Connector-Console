
using System.Drawing;

namespace Connector.Printer
{
    /// <summary>
    /// Класс отвечающий за вывод информации
    /// </summary>
    public static class Print
    {
        /// <summary>
        /// Текущий объект представления информации
        /// </summary>
        public static IDisplay Display { get; private set; }

        /// ---------------- LOG -----------------------
        
        /// <summary>
        /// Выводит сообщение
        /// </summary>
        public static void Log(object log)
        {
            Display.ShowMessage(log);
        }
        /// <summary>
        /// Перегрузка Log: выводит сообщение заданного цвета
        /// </summary>
        public static void Log(object log, Color color)
        {
            Display.ShowMessage(log, color);
        }
        /// <summary>
        /// Перегрузка Log: выводит сообщение заданного цвета с заданным background
        /// </summary>
        public static void Log(object log, Color color, Color colorBack)
        {
            Display.ShowMessage(log, color, colorBack);
        }
        
        /// ---------------- ASSERT -----------------------

        /// <summary>
        /// Возвращает булевский параметр для условных выражений
        /// </summary>
        public static bool Assert(bool condition)
        {
            return condition;
        }

        /// <summary>
        /// Принимает сообщения в параметре и выводит их
        /// </summary>
        public static bool Assert(bool condition, object message)
        {
            Print.Log(message, (condition) ? Color.Green : Color.Red);
            return condition;
        }
        
        /// ---------------- ERROR -----------------------
        
        /// <summary>
        /// Выводит сообщение об ошибке
        /// </summary>
        public static void LogError(object message)
        {
            Log(message, Color.White, Color.Red);
        }

        /// ---------------- WARNING ---------------------

        /// <summary>
        /// Выводит сообщение с предупреждением
        /// </summary>
        public static void LogWarning(object message)
        {
            Log(message, Color.Black, Color.Yellow);
        }
        
        /// ---------------- OTHER -----------------------
        
        /// <summary>
        /// Устанавливает текущий объект представления информации
        /// </summary>
        public static void SetDisplay(IDisplay display)
        {
            if (display != null)
                Display = display;
        }
    }
}