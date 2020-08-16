using System;
using System.Threading;

namespace Connector.Loops
{
    public static class LoopFactory
    {
        
        /// <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static T Create<T, TU>() 
            where T: LoopGeneric<TU>, new()
            where TU: class
        {
            return Create<T, TU>("read_input_loop");
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop
        /// </summary>
        public static T Create<T,TU>(string id)
            where T: LoopGeneric<TU>, new()
            where TU: class
        {
            return Create<T,TU>(id, null);
        }

        // <summary>
        /// Создает новый экземпляр ReadInputLoop и подписывает метод
        /// </summary>
        public static T Create<T,TU>(string id, Action<TU> action, bool startOnAwake = false)
            where T: LoopGeneric<TU>, new()
            where TU: class
        {
            
            // Новый экземпляр
            var loopT = new T();
            
            loopT.SetCancellationTokenSource(new CancellationTokenSource());

            // Подписываем переданный метод
            if (action != null)
                loopT.Subscribe(action);
            
            // Добавляем в менеджер
            LoopManager.AddLoop(loopT);
            
            LoopManager.Log($"Loop {loopT.Id}: ожидает старта");

            // Запускаем, если нужно
            if (startOnAwake)
            {
                loopT.Start();
            }
            
            return loopT;
        }
    }
}