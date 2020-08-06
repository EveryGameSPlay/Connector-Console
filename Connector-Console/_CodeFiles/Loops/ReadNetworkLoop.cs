using System;
using System.Threading;
using System.Threading.Tasks;
using Connector.Network;
using Gasanov.Tools;

namespace Connector.Loops
{
    public class ReadNetworkLoop : Loop
    {
        private ReadNetworkLoop() : base()
        {
            Id = "read_network_loop";
        }

        private ReadNetworkLoop(string id) : base()
        {
            Id = id;
        }
        
        /// <summary>
        /// Событие, взывающееся при обновлении
        /// </summary>
        private event Action<string> OnUpdateEvent = delegate(string f) {  };
        
        protected override Task Update()
        {
            var netService = Toolbox.GetTool<INetworkService>();

            while (true)
            {
                if (CheckCancelationToken())
                    return WorkspaceTask;

                if (!IsPaused)
                {
                    if(netService == null)
                        continue;
                    
                    var str = netService.RecieveString();
                    OnUpdateEvent(str);
                }
            }
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
        
        protected override void Dispose()
        {
            return;
        }
        
        
        
        
        
        /// --------------- STATIC -----------------------
        
        /// <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static ReadNetworkLoop Create()
        {
            return Create("read_network_loop");
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static ReadNetworkLoop Create(string id)
        {
            return Create(id, null);
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop и подписывает метод
        /// </summary>
        public static ReadNetworkLoop Create(string id, Action<string> action, bool startOnAwake = false)
        {
            // Новый экземпляр
            var readNetworkLoop = new ReadNetworkLoop(id);
            readNetworkLoop.CancellationTokenSource = new CancellationTokenSource();

            // Подписываем переданный метод
            if (action != null)
                readNetworkLoop.OnUpdateEvent += action;
            
            // Добавляем в менеджер
            LoopManager.AddLoop(readNetworkLoop);
            
            LoopManager.Log($"Loop {readNetworkLoop.Id}: ожидает старта");

            // Запускаем, если нужно
            if (startOnAwake)
            {
                readNetworkLoop.Start();
            }
            
            return readNetworkLoop;
        }

    }
}