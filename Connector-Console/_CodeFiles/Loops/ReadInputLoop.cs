using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Connector.Commands.CommandsRealization;
using Connector.Commands;
using Connector.ConsoleRealizarions.Input;
using Connector.Input;
using Gasanov.Tools;

namespace Connector.Loops
{
    public class ReadInputLoop: LoopGeneric<string>
    {
        public ReadInputLoop() : base()
        {
            Id = "read_input_loop";
        }
       

        protected override void Update()
        {
            LoopStatus = LoopStatus.Running;

            var inputHandler = Toolbox.GetTool<IInputHandler>();
            while (true)
            {
                if (CheckCancelationToken())
                    return;

                // Если не стоит пауза
                if(!IsPaused)
                {
                    var str = inputHandler.GetInputLine();
                    
                    // Вызываем обновление
                    InvokeUpdateEvent(str);
                }
            }
        }

        protected override void Dispose()
        {
            base.Dispose();
        }
        
    }
}