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
using Commands.CommandsRealization;
using Connector.ConsoleRealizarions.Displays;
using Connector.Input;
using Connector.Loops;

namespace Connector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Initialize();

            var readInputLoop = ReadInputLoop.Create();
            readInputLoop.Start();
            
            

            LoopManager.WaitAll();
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
