using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Connector.Buffers
{

    /// <summary>
    /// Класс, представляющий собой буфер
    /// для хранения строковых сообщений
    /// </summary>
    public class MessageBuffer : IMessageBuffer
    {
        public MessageBuffer()
        {
            Id = "default";
            Messages = new List<string>();
        }
        
        /// <param name="id">Идентификатор буфера</param>
        public MessageBuffer(string id) : this()
        {
            Id = id;
        }
        
        /// <summary>
        /// Строка для хранения ID буфера
        /// </summary>
        public string Id { get; private set; }
        
        /// <summary>
        /// Список для хранения сообщений
        /// </summary>
        public List<string> Messages { get; private set; }

        
        /// <summary>
        /// Добавляет строку в конец списка
        /// </summary>
        public virtual void Add(string newMessage)
        {
            Messages.Add(newMessage);
        }
        
        /// <summary>
        /// Возвращает первую строку из списка и удаляет её оттуда
        /// </summary>
        public virtual string Queue()
        {
            if (Messages.Count == 0)
                return null;

            string firstElement = Messages[0];
            Messages.RemoveAt(0);
            return firstElement;
        }
        
        /// <summary>
        /// Возвращает n-строку из списка и удаляет её оттуда
        /// </summary>
        public virtual string Queue(int n)
        {
            if (n < 0 || n >= Messages.Count)
                return null;

            string firstElement = Messages[n];
            Messages.RemoveAt(n);
            return firstElement;
        }

        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach.
        /// </summary>
        public virtual IEnumerable Query()
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                yield return Messages[i];
            }
        }
        
        /// <summary>
        /// умеет задавать начальную позицию цикла
        /// </summary>
        public virtual IEnumerable Query(int start)
        {
            for (int i = start; i < Messages.Count; i++)
            {
                if (i < 0)
                    yield break;
                else
                    yield return Messages[i];
            }
        }
        
        /// <summary>
        /// умеет задавать начальную и конечную позиции цикла
        /// </summary>
        public virtual IEnumerable Query(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                if (i < 0 || i > Messages.Count-1)
                    yield break;
                else
                    yield return Messages[i];
            }
        }

        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach
        /// и удалять i-ые элементы из списка
        /// </summary>
        public virtual IEnumerable QueryQueue()
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                string firstElement = Messages[0];
                Messages.RemoveAt(0);
                yield return firstElement;
            }
        }
        
        /// <summary>
        /// умеет задавать начальную позицию цикла
        /// </summary>
        public virtual IEnumerable QueryQueue(int start)
        {
            for (int i = start; i < Messages.Count; i++)
            {
                if (i < 0)
                    yield break;
                else
                {
                    string firstElement = Messages[start];
                    Messages.RemoveAt(start);
                    yield return firstElement;
                }
            }

        }
        
        /// <summary>
        /// умеет задавать начальную и конечную позиции цикла
        /// </summary>
        public virtual IEnumerable QueryQueue(int start, int end)
        {
            for (int i = start; i < end; i++)
            {

                if (i < 0 || end >= Messages.Count)
                    yield break;
                else
                {
                    string firstElement = Messages[start];
                    Messages.RemoveAt(start);
                    yield return firstElement;
                }
            }
        }

        /// <summary>
        /// Удаляет все сообщения в списке
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }

    }
}
