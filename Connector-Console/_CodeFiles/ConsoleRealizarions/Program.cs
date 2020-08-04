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
using System.Threading;
using Connector.Commands.CommandsRealization;
using Connector.ConsoleRealizarions.Displays;
using Connector.ConsoleRealizarions.Input;
using Connector.Input;
using Connector.Loops;
using Gasanov.Tools;

namespace Connector.ConsoleRealizarions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Initialize();
            InitialzieLoops();

            LoopManager.StartAll();
            LoopManager.WaitAll();
        }

        /// <summary>
        /// Функция инициализации программных компонентов
        /// </summary>
        private static void Initialize()
        {
            var display = new ConsoleColoredDisplay();
            Print.SetDisplay(display);
            
            SetupTools();
        }

        /// <summary>
        /// Инициализирует циклы
        /// </summary>
        private static void InitialzieLoops()
        {
            // Цикл чтения
            var readInputLoop = ReadInputLoop.Create();

            // Получаем обработчик команд и добавляем команду
            var commandHandler = Toolbox.GetTool<CommandHandler>();
            commandHandler.Add(new Command("cancel", (string[] args) =>
            {
                readInputLoop.Cancel();
            }));
            
            readInputLoop.Subscribe(commandHandler.Handle);
        }

        /// <summary>
        /// Настройка инструментов
        /// </summary>
        private static void SetupTools()
        {
            var commandHandler = new CommandHandler();
            SetupCommands(commandHandler);
            
            Toolbox.Add(commandHandler);
            Toolbox.Add(new ConsoleInputHandler());
        }

        /// <summary>
        /// Настройка обработчика команд
        /// </summary>
        private static void SetupCommands(CommandHandler commandHandler)
        {
            
        }
    }

}
