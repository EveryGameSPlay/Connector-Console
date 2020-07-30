using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Connector.ConsoleRealizarions.Input;
using Connector.Input;

namespace Connector.Loops
{
    public class ReadInputLoop: Loop
    {
        private ReadInputLoop()
        {
            Id = "read_input_loop";
            
            _consoleInputHandler = new ConsoleInputHandler();
        }

        private ReadInputLoop(string id)
        {
            Id = id;
            
            _consoleInputHandler = new ConsoleInputHandler();
        }
        
        /// <summary>
        /// Событие, взывающееся при обновлении
        /// </summary>
        private event Action<string> OnUpdateEvent = delegate(string f) {  };

        private ConsoleInputHandler _consoleInputHandler;
        
        protected override Task Update()
        {
            LoopStatus = LoopStatus.Running;
            
            while (true)
            {
                var token = CancellationTokenSource.Token;
            
                // Если запрошена отмена цикла - завершаем работу
                if (token.IsCancellationRequested == true)
                {
                    // Очищаем данные
                    Dispose();

                    LoopStatus = LoopStatus.Canceled;
                    
                    LoopManager.Log($"Цикл {Id} остановлен");
                    return WorkspaceTask;
                }
                
                // LoopManager.Log($"Введите значение");
                
                var str = _consoleInputHandler.GetInputLine();
                
                // WORKSPACE
                
                // END WORKSPACE
                
                if(str == "cancel")
                    Cancel();
                
                // LoopManager.Log($"Введено значение: {str}");
            
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

            // Запускаем, если нужно
            if (startOnAwake)
            {
                updateLoop.Start();
            }
            
            return updateLoop;
        }
        
        
    }
}