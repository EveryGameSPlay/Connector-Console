using System;

namespace Connector.Loops
{
    public abstract class LoopGeneric<T> :Loop
    {
        /// <summary>
        /// Событие, взывающееся при обновлении
        /// </summary>
        protected event Action<T> OnUpdateEvent = delegate(T arg) {  };
        
        
        /// <summary>
        /// Вызывает событие
        /// </summary>
        protected void InvokeUpdateEvent(T arg)
        {
            OnUpdateEvent(arg);
        }
        
        /// <summary>
        /// Подписывает метод на события обновления
        /// </summary>
        public void Subscribe(Action<T> action)
        {
            if (action != null)
                OnUpdateEvent += action;
        }

        /// <summary>
        /// Отписывает метод от событий обновления
        /// </summary>
        public void Unsubscribe(Action<T> action)
        {
            if (action != null)
                OnUpdateEvent -= action;
        }
         
    }
}