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
        
        /// <summary>
        /// Событие, взывающееся при обновлении
        /// </summary>
        private event Action<string> OnUpdateEvent = delegate(string f) {  };
        
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
                    netService.ListenString((string str) => OnUpdateEvent(str));
                }
                
                Sleep();
            }
        }
        
        protected override void Dispose()
        {
            base.Dispose();
        }
        
        /// <summary>
        /// Подписывает метод на события обновления
        /// </summary>
        public ReadNetworkLoop Subscribe(Action<string> action)
        {
            if (action != null)
                OnUpdateEvent += action;

            return this;
        }

        /// <summary>
        /// Отписывает метод от событий обновления
        /// </summary>
        public void Unsubscribe(Action<string> action)
        {
            if (action != null)
                OnUpdateEvent -= action;
        }
        
        

    }
}