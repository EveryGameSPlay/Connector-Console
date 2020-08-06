using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using Connector.Printer;

namespace Connector.Commands
{

    /// <summary>
    /// Класс, отвечающий за
    /// обработку комманд
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Регулярные выражения
        /// для поиска комманд в строке
        /// </summary>
        private static Regex _regexCommand = new Regex(@"^\s*(\w+)\s*(:(.*))?");
        private static Regex _regexCommandParams = new Regex(@"[^,]+(?=\s*,|\s*$)");
        
        
        /// <summary>
        /// Содержит список команд
        /// </summary>
        public List<ICommand> Commands { get; private set; }

        /// -------------- Конструктор --------------
        /// <summary>
        /// Инициализирует Commands
        /// </summary>
        public CommandHandler()
        {
            Commands = new  List<ICommand>();
        }
        /// ---------------------------------------------

        /// -------------- Handle() --------------
        /// <summary>
        /// Обрабатывает введенные команды
        /// </summary>  
        public void Handle(string command)
        {

            Match commandMatch = _regexCommand.Match(command);

            if (commandMatch.Success)
            {
                string commandId = commandMatch.Groups[1].Value.ToLower();

                string[] commandParams = _regexCommandParams
                    .Matches(commandMatch.Groups[3].Value)
                    .Select(m => m.Value)
                    .ToArray();

                for (var i = 0; i < commandParams.Length; i++)
                {
                    commandParams[i] = commandParams[i].Trim();
                }

                var commandObject = Commands.FirstOrDefault(x => x.Id == commandId);

                if (commandObject != null)
                {
                    commandObject.Invoke(commandParams);
                }
                else
                {
                    Print.LogWarning($"Команды {commandId} нет в словаре");
                }
            }
            else
            {
                Print.LogWarning($"Команды {command} не существует");
            }
        }
        /// ---------------------------------------------
        
        /// -------------- Add() --------------
        /// <summary>
        /// Добавляет новую команду в словарь с проверкой
        /// на наличие её ключа в текущем словаре команд
        /// </summary>
        public ICommand Add(ICommand command)
        {
            if (command == null)
                return null;
            
            if (Commands.Exists(x=> x.Id == command.Id))
            {
                Print.LogWarning($"Команда с ключом {command.Id} уже есть в словаре!");
                return null;
            }

            Commands.Add(command);
            return command;
        }
        /// ---------------------------------------------

        /// -------------- Remove() --------------
        /// <summary>
        /// Удаляет команду по id из словаря
        /// </summary>
        public void Remove(string id)
        {
            if (Commands.RemoveAll(x => x.Id == id) == 0)
                Print.LogWarning($"Элемент с ключом {id} отсутствует в списке");
        }

        public void Remove(ICommand command)
        {
            Commands.Remove(command);
        }
        /// ---------------------------------------------
        
        /// -------------- Clear() --------------
        /// <summary>
        /// Чистит словарь
        /// </summary>
        public void Clear()
        {
            Commands.Clear();
        }
        /// ---------------------------------------------

        /// -------------- GetEnumerator() --------------
        /// <summary>
        /// Возвращает Enumerator словаря
        /// </summary>
        public List<ICommand>.Enumerator GetEnumerator()
        {
            return Commands.GetEnumerator();
        }
        /// ---------------------------------------------


    }
}
