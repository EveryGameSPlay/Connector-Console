using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// Ожидает завершения всех задач в циклах
        /// </summary>
        public static void WaitAll()
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
                    
                    // ПАМЯТКА: Ожидание останавливает поток, в котором исполняется задача (deadlock)
                    // _loops[i].WorkspaceTask.Wait();
                }

                // Все циклы закончили работу
                if (allCompleted == true)
                {
                    ShowTaskStatus();

                    return;
                }
            }
        }

        public static void ShowTaskStatus()
        {
            for (var i = 0; i < _loops.Count; i++)
            {
                var loop = _loops[i];
                Print.Log($"{loop.Id}: задача имеет статус ({loop.LoopStatus})", Color.Gray);
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