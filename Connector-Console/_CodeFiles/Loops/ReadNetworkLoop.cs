using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Connector.Network;
using Connector.Printer;
using Gasanov.Tools;

namespace Connector.Loops
{
    public class ReadNetworkLoop : LoopGeneric <string>
    { 
        public ReadNetworkLoop() : base()
        {
            Id = "read_network_loop";
        }
        
        protected override void Update()
        {
            var netService = Toolbox.GetTool<INetworkService>();

            while (true)
            {
                if (CheckCancelationToken())
                    return;

                if (!IsPaused)
                {
                    if(netService == null)
                        continue;
                    
                    // Передача события, которые вызовется при получении данных
                    netService.ListenString((string str) => InvokeUpdateEvent(str));
                }
                
                Sleep();
            }
        }
        
        protected override void Dispose()
        {
            base.Dispose();
        }
    }
}