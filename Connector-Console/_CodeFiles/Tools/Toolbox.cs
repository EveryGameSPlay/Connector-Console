using System;
using System.Collections.Generic;
using System.Linq;
using Gasanov.Extensions.Linq;

namespace Gasanov.Tools
{
    /// <summary>
    /// Предоставляет инструментарий для получения компонентов
    /// </summary>
    public static class Toolbox
    {
        static Toolbox()
        {
            _tools = new List<object>();    
        }
        
        /// <summary>
        /// Список с инструментами
        /// </summary>
        private static List<object> _tools;

        
        /// <summary>
        /// Добавление инструмента.
        /// </summary>
        public static bool Add(object tool)
        {
            // Если инструмент не был добавлен
            if (!_tools.Contains(tool))
            {
                _tools.Add(tool);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Удаление инструмента по ссылке
        /// </summary>
        public static void Remove(object tool)
        {
            _tools.Remove(tool);
        }

        public static void Remove<T>() where T: class
        {
            var coincidence = GetTool<T>();

            if (coincidence != null)
                _tools.Remove(coincidence);
        }

        /// <summary>
        /// Удаление инструмента по условию
        /// </summary>
        public static void Remove<T>(Predicate<T> predicate) where T: class
        {
            var predicated = GetTool<T>();

            if (predicated != null)
                _tools.Remove(predicated);
        }
        
        /// <summary>
        /// Получение инструмента по типу. Возвращает первое совпадение.
        /// Может вернуть null.
        /// </summary>
        public static T GetTool<T>() where T : class
        {
            return _tools.FindType<T>();
        }

        /// <summary>
        /// Получение инструмента по типу и условию. Возвращает первое совпадение.
        /// Может вернуть null.
        /// </summary>
        public static T GetTool<T>(Predicate<T> predicate) where T : class
        {
            var toolsT = _tools.FindTypes<T>();

            for (var i = 0; i < toolsT.Count; i++)
            {
                var toolT = toolsT[i];
                var predicated = predicate(toolsT[i]);

                // Если инструмент удовлетворяет условиям
                if (predicated == true)
                    return toolT;
            }

            return default(T);
        }        
        
    }
}