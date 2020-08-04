using System;
using Connector.Commands;

namespace Connector.Commands.CommandsRealization
{
    /// <summary>
    /// Класс универсального типа команд
    /// </summary>
    public class Command: CommandBase
    {
        public Command(string id, Action<string[]> action) : base()
        {
            Id = id;
            Action = action;
        }
        

        public Action<string[]> Action;
        
        public override void Invoke(string[] args)
        {
            Action(args);
        }

        public override string ToString()
        {
            return Id;
        }
    }
}