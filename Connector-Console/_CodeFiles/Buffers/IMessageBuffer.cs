using System.Collections;
using System.Collections.Generic;

namespace Connector.Buffers
{
    public interface IMessageBuffer
    {
        /// <summary>
        /// Строка для хранения ID буфера
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Список для хранения сообщений
        /// </summary>
        List<string> Messages { get; }

        /// <summary>
        /// Добавляет строку в конец списка
        /// </summary>
        void Add(string newMessage);

        /// <summary>
        /// Возвращает первую строку из списка и удаляет её оттуда
        /// </summary>
        string Queue();

        /// <summary>
        /// Возвращает n-строку из списка и удаляет её оттуда
        /// </summary>
        string Queue(int n);

        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach.
        /// </summary>
        IEnumerable Query();

        /// <summary>
        /// умеет задавать начальную позицию цикла
        /// </summary>
        IEnumerable Query(int start);

        /// <summary>
        /// умеет задавать начальную и конечную позиции цикла
        /// </summary>
        IEnumerable Query(int start, int end);

        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach
        /// и удалять i-ые элементы из списка
        /// </summary>
        IEnumerable QueryQueue();

        /// <summary>
        /// умеет задавать начальную позицию цикла
        /// </summary>
        IEnumerable QueryQueue(int start);

        /// <summary>
        /// умеет задавать начальную и конечную позиции цикла
        /// </summary>
        IEnumerable QueryQueue(int start, int end);

        /// <summary>
        /// Удаляет все сообщения в списке
        /// </summary>
        void Clear();
    }
}