using System;
using Connector.Commands;

namespace Commands.CommandsRealization
{
    /// <summary>
    /// Класс универсального типа команд
    /// </summary>
    public class Command: ICommand
    {
        public Command(string id, Action<string[]> action)
        {
            Id = id;
            Action = action;
        }
        
        public string Id { get; private set; }

        public Action<string[]> Action;
        
        public void Invoke(string[] args)
        {
            Action(args);
        }

        public override string ToString()
        {
            return Id;
        }
    }
}