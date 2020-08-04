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
    public class ReadInputLoop: Loop
    {
        private ReadInputLoop() : base()
        {
            Id = "read_input_loop";
        }

        private ReadInputLoop(string id) : base()
        {
            Id = id;
        }
        
        /// <summary>
        /// Событие, взывающееся при обновлении
        /// </summary>
        private event Action<string> OnUpdateEvent = delegate(string f) {  };

        protected override Task Update()
        {
            LoopStatus = LoopStatus.Running;

            var inputHandler = Toolbox.GetTool<IInputHandler>();
            while (true)
            {
                var token = CancellationTokenSource.Token;
            
                // Если запрошена отмена цикла - завершаем работу
                if (token.IsCancellationRequested == true)
                {
                    // Очищаем данные
                    Dispose();

                    LoopStatus = LoopStatus.Canceled;
                    
                    LoopManager.Log($"Loop {Id}: остановлен");
                    return WorkspaceTask;
                }

                // Если не стоит пауза
                if(!IsPaused)
                {
                    var str = inputHandler.GetInputLine();
                    
                    // Вызываем обновление
                    OnUpdateEvent(str);
                }

            }

            LoopStatus = LoopStatus.Completed;
            return WorkspaceTask;
        }

        protected override void Dispose()
        {
        }


        /// <summary>
        /// Подписывает метод на события обновления
        /// </summary>
        public ReadInputLoop Subscribe(Action<string> action)
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
        
        
        
        
        
        
        
        
        
        
        /// --------------- STATIC -----------------------
        
        /// <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static ReadInputLoop Create()
        {
            return Create("read_input_loop");
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static ReadInputLoop Create(string id)
        {
            return Create(id, null);
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop и подписывает метод
        /// </summary>
        public static ReadInputLoop Create(string id, Action<string> action, bool startOnAwake = false)
        {
            // Новый экземпляр
            var updateLoop = new ReadInputLoop(id);
            updateLoop.CancellationTokenSource = new CancellationTokenSource();

            // Подписываем переданный метод
            if (action != null)
                updateLoop.OnUpdateEvent += action;
            
            // Добавляем в менеджер
            LoopManager.AddLoop(updateLoop);
            
            LoopManager.Log($"Loop {updateLoop.Id}: ожидает старта");

            // Запускаем, если нужно
            if (startOnAwake)
            {
                updateLoop.Start();
            }
            
            return updateLoop;
        }
        
    }
}