using System.Collections;
using System.Collections.Generic;

namespace Gasanov.Extensions.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Возвращает первый объект заданного типа. Может вернуть null.
        /// </summary>
        public static T FindType<T>(this IEnumerable source) where T : class
        {
            T coincidence = null;

            foreach (var obj in source)
            {
                coincidence = obj as T;

                // Если есть совпадение
                if (coincidence != null)
                {
                    return coincidence;
                }
            }

            return coincidence;
        }

        /// <summary>
        /// Возвращает все типы заданного типа. Может вернуть пустой список.
        /// </summary>
        public static List<T> FindTypes<T>(this IEnumerable source) where T : class
        {    
            var coincidences = new List<T>();

            foreach (var obj in source)
            {
                var coincidence = source as T;

                // Если тип совпадает
                if (coincidence != null)
                {
                    coincidences.Add(coincidence);
                }
            }

            return coincidences;
        }
    }
}