using CSScriptLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;

//System
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

//Connector
using Connector.Printer;
using Connector.Buffers;
using Connector.Commands;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using Commands.CommandsRealization;
using Connector.ConsoleRealizarions.Displays;

namespace Connector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Initialize();

            // -------------- Тест объекта представления --------------

            Print.Log("Hello World!");
            Print.Log(134);
            Print.Log('c');

            Print.Assert(true, "Метод Assert вернул true");
            Print.Assert(false, "Метод Assert вернул false");

            Print.LogError("Тест сообщения об ошибке");
            Print.LogWarning("Тест сообщения с предупреждением");

            // ---------------------------------------------

            // -------------- Тест хэндлера команд --------------

            Print.Log("\n--------------Buffer--------------\n", Color.Yellow);

            //создания экземпляра CommandHandler
            CommandHandler testHandler = new CommandHandler();

            //добавление команд 1 и 2 и 3(которая без аргументов)
            testHandler.Add(new TestCommand());
            testHandler.Add(new TestCommand());
            testHandler.Add(new Command("sum", (string[] msg) =>
            {
                Print.Log($"Сумма {msg[0]} и {msg[1]}: {int.Parse(msg[0])+int.Parse(msg[1])}", Color.Yellow);
            }));
            testHandler.Add(new Command("simpleMsg", (string[] msg) =>
            {
                Print.Log("This is simpleMsg", Color.Yellow);
            }));


            //обработка команды 1
            testHandler.Handle("        testCommand      :      param1    , 34,563, 3.5  ,    param2,       param3  param4");

            //обработка команды 2
            testHandler.Handle("        sum      :      100   ,       200   ,       3  123 12 3 param4");

            //обработка команды 3 (без аргументов)
            testHandler.Handle("   simpleMsg :   ");
            testHandler.Handle("   simpleMsg    ");

            Print.Log("\n");

            // GetEnumerator
            foreach (var command in testHandler)
            {
                Print.Log($"Команда[{command}]", Color.Chocolate);
            }

            Print.Log("\n");

            //удаление команд 1 и 2
            testHandler.Remove("testCommand");
            testHandler.Remove("testCommand");
            testHandler.Remove("sum");

            Print.Log("\n\n----------------------------\n", Color.Yellow);

            //// Тест ассинхронной операции
            //Task.Run(() =>
            //{
            //    // Print.Log("Async method");
            //    // Ввод сбоится, когда идет вывод вместе с вводом
            //    // Print.Log(InputHandler.GetInputLine());
            //});

            //for (var i = 0; i < 10; i++)
            //{
            //    dynamic script = CSScript.Evaluator.LoadCode(
            //               @"using System;
            //                 public class Script
            //                 {
            //                     public void SayHello(string greeting)
            //                     {
            //                         Console.WriteLine(""Greeting: "" + greeting);
            //                     }
            //                 }");
            //    script.SayHello("Hello World!");
            //}

            // Ожидает завершения всех задач
            Task.WaitAll();
        }

        /// <summary>
        /// Функция инициализации программных компонентов
        /// </summary>
        private static void Initialize()
        {
            var display = new ConsoleColoredDisplay();
            Print.SetDisplay(display);
            
        }
    }

}
