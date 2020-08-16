using Connector.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connector.Commands.CommandsRealization
{
    public abstract class CommandBase : ICommand
    {

        protected CommandBase()
        {
        }

        public string Id
        {
            get
            {
                return id;
            }
            protected set 
            {
                id = value.ToLower();
            }
        }

        private string id;

        public abstract void Invoke(string[] args);
    }
}
