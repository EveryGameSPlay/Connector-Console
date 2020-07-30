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
<<<<<<< Updated upstream:Connector-Console/_CodeFiles/ConsoleRealizarions/Program.cs
using Connector.ConsoleRealizarions.Displays;
=======
using Connector.Input;
using Connector.Loops;
>>>>>>> Stashed changes:Connector-Console/_CodeFiles/Program.cs

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
