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
    public class MessageBuffer
    {
        /// <summary>
        /// Строка для хранения ID буфера
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Список для хранения сообщений
        /// </summary>
        public List<string> Messages { get; private set; }

        /// -------------- Конструктор --------------
        /// <summary>
        /// Инициализирует Id и список Messages
        /// </summary>
        private MessageBuffer()
        {
            Id = "default";
            Messages = new List<string>();
        }
        
        /// <summary>
        /// умеет принимать строку для инициализации Id
        /// </summary>
        /// <param name="id">Идентификатор буфера</param>
        private MessageBuffer(string id) : this()
        {
            Id = id;
        }
        /// ---------------------------------------------

        /// -------------- Create() --------------
        /// <summary>
        /// Создаёт экземпляр MessageBuffer
        /// </summary>
        public static MessageBuffer Create()
        {
            return new MessageBuffer();
        }
        /// <summary>
        /// Умеет принимать строку и передавать её
        /// конструтору MessageBuffer
        /// </summary>
        public static MessageBuffer Create(string id)
        {
            return new MessageBuffer(id);
        }
        /// ---------------------------------------------

        /// -------------- Add() --------------
        /// <summary>
        /// Добавляет строку в конец списка
        /// </summary>
        public void Add(string newMessage)
        {
            Messages.Add(newMessage);
        }
        /// ---------------------------------------------

        /// -------------- Queue() --------------
        /// <summary>
        /// Возвращает первую строку из списка и удаляет её оттуда
        /// </summary>
        public string Queue()
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
        public string Queue(int n)
        {
            if (n < 0 || n >= Messages.Count)
                return null;

            string firstElement = Messages[n];
            Messages.RemoveAt(n);
            return firstElement;
        }
        /// ---------------------------------------------

        /// -------------- Query() --------------
        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach.
        /// </summary>
        public IEnumerable Query()
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                yield return Messages[i];
            }
        }
        /// <summary>
        /// умеет задавать начальную позицию цикла
        /// </summary>
        public IEnumerable Query(int start)
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
        public IEnumerable Query(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                if (i < 0 || i > Messages.Count-1)
                    yield break;
                else
                    yield return Messages[i];
            }
        }
        /// ---------------------------------------------

        /// -------------- QueryQueue() --------------
        /// <summary>
        /// Возвращает перечисление, с помощью которого
        /// можно проходится по всем сообщениям в foreach
        /// и удалять i-ые элементы из списка
        /// </summary>
        public IEnumerable QueryQueue()
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
        public IEnumerable QueryQueue(int start)
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
        public IEnumerable QueryQueue(int start, int end)
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
        ///---------------------------------------------

        /// -------------- Clear() --------------
        /// <summary>
        /// Удаляет все сообщения в списке
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }
        /// ---------------------------------------------

    }
}
