using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Connector.Printer;

namespace Connector.Loops
{
    public static class LoopManager
    {
        static LoopManager()
        {
            _loops = new List<Loop>();    
        }
        
        private static List<Loop> _loops;

        /// <summary>
        /// Добавляет новый экземпляр Loop
        /// </summary>
        /// <param name="loop"></param>
        public static void AddLoop(Loop loop)
        {
            if (loop == null)
                return;

            if (_loops.Exists(x => x.Id == loop.Id))
            {
                Print.LogWarning($"{loop.Id} цикл не был добавлен, " +
                                 $"т.к. уже существует цикл с таким же идентификатором");

                return;
            } 
            
            _loops.Add(loop);
            
            Print.Log($"Добавлен цикл {loop.Id}", Color.Green);
        }
        
        /// <summary>
        /// Запускает все циклы, ожидающие старта.
        /// Циклы ожидают старта при создании
        /// </summary>
        public static void StartAll()
        {
            for (var i = 0; i < _loops.Count; i++)
            {
                var loop = _loops[i];

                if (loop.LoopStatus == LoopStatus.WaitingForStart)
                {
                    loop.Start();
                }
            }
        }

        /// <summary>
        /// Ожидает завершения всех задач в циклах.
        /// Останавливает поток на millisecondsDelay каждую итерацию.
        /// </summary>
        public static void WaitAll(int msDelay)
        {
            // Пока не дождемся окончания всех задач
            while (true)
            {
                // Считаем, что все завершили свою работу
                var allCompleted = true;
                for (var i = 0; i < _loops.Count; i++)
                {
                    var loop = _loops[i];
                    // Цикл все ещё работает
                    if (loop.LoopStatus == LoopStatus.Running ||
                        loop.LoopStatus == LoopStatus.WaitingForStart)
                    {
                        allCompleted = false;
                    }
                }

                // Все циклы закончили работу
                if (allCompleted == true)
                {
                    ShowLoopStatus();
                    AbortLoopThreads();
                    

                    return;
                }
                
                // Задержка для освобождения ресурсов
                Thread.Sleep(msDelay);
            }
        }

        public static void ShowLoopStatus()
        {
            for (var i = 0; i < _loops.Count; i++)
            {
                var loop = _loops[i];
                Print.Log($"Loop {loop.Id}: задача имеет статус ({loop.LoopStatus})", Color.Gray);
            }
        }

        public static void AbortLoopThreads()
        {
            for (var i = 0; i < _loops.Count; i++)
            {
                var loop = _loops[i];
                loop.WorkspaceThread.Abort();
                Print.Log($"Loop {loop.Id}: поток был закрыт)", Color.Gray);
            }
        }

        public static void Log(object message)
        {
            Print.Log(message);
        }

        public static void LogWarning(object message)
        {
            Print.LogWarning(message);
        }

        public static void LogError(object message)
        {
            Print.LogError(message);
        }

    }
}