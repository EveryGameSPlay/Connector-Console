﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connector.Loops
{
    public abstract class Loop : IProgress<LoopStatus>
    {
        protected Loop()
        {
            LoopStatus = LoopStatus.WaitingForStart;
            IsPaused = false;
        }
        
        /// <summary>
        /// Идентификатор цикла
        /// </summary>
        public string Id { get; protected set; }
        
        /// <summary>
        /// Задача, в которой работает экземпляр
        /// </summary>
        public Task WorkspaceTask { get; protected set; }
        
        public CancellationTokenSource CancellationTokenSource { get; protected set; }
        
        public LoopStatus LoopStatus { get; protected set; }

        public bool IsPaused { get; protected set; }

        /// <summary>
        /// Запускает работу цикла в отдельную задачу
        /// </summary>
        public virtual void Start()
        {
            LoopManager.Log($"Loop {Id}: запущен");
            // Запускает новую задачу
            WorkspaceTask = Task.Run(() => Update());
        }

        /// <summary>
        /// Запускает новый проход
        /// </summary>
        /// <returns></returns>
        protected abstract Task Update();
        
        protected abstract void Dispose();

        /// <summary>
        /// Ставит цикл на паузу
        /// </summary>
        public void Stop()
        {
            IsPaused = true;
        }

        /// <summary>
        /// Снимает цикл с паузы
        /// </summary>
        public void Resume()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Останавливает цикл и связанный с ним Task
        /// </summary>
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
            
            LoopManager.Log($"Loop {Id}: вызвана полная остановка");
        }

        
        /// <summary>
        /// Устанавливает нового родителя токена отмены
        /// </summary>
        /// <param name="source"></param>
        public void SetCancellationTokenSource(CancellationTokenSource source)
        {
            if (source == null)
                return;
            
            if(CancellationTokenSource != null)
                LoopManager.LogWarning($"Loop {Id} имеет токен, но будет заменен на новый");

            CancellationTokenSource = source;
        }

        /// <summary>
        /// Проверяет запрос отмены. True если запрос действителен.
        /// </summary>
        protected bool CheckCancelationToken()
        {
            var token = CancellationTokenSource.Token;
            
            // Если запрошена отмена цикла - завершаем работу
            if (token.IsCancellationRequested == true)
            {
                // Очищаем данные
                Dispose();

                LoopStatus = LoopStatus.Canceled;
                    
                LoopManager.Log($"Loop {Id}: остановлен");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Вывод текущего статуса
        /// </summary>
        public void Report(LoopStatus value)
        {
            LoopManager.Log(value);
        }

         
    }

    public enum LoopStatus
    {
        Running = 1,
        WaitingForStart = 0,
        Canceled = 3,
        Faulted = 4,
        Completed = 2
    }
}