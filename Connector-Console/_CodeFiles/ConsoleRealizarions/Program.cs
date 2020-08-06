using CSScriptLib;
using Microsoft.CodeAnalysis.CSharp.Syntax;

//System
using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

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
using Connector.Network;
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
            
            // Цикл чтения сокетов
            var readNetworkLoop = ReadNetworkLoop.Create();

            var messageBuffer = Toolbox.GetTool<MessageBuffer>();
            if (messageBuffer == null)
                Toolbox.Add(MessageBuffer.Create("network_buffer"));

            readNetworkLoop.Subscribe((string str) => messageBuffer.Add(str));
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
            commandHandler.Add(new Command("set_protocol", (string[] args) =>
            {
                var networkService = Toolbox.GetTool<INetworkService>();

                if (networkService != null)
                {
                    networkService.Dispose();
                    Toolbox.Remove(networkService);
                }



                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "udp":
                            var udpService = new UdpService();
                            Toolbox.Add(udpService);
                            Print.Assert(true, "Установлен протокол UDP");
                            break;
                        
                        case "tcp":
                            var udpServiceTCP = new UdpService();
                            Toolbox.Add(udpServiceTCP);
                            Print.Assert(false, "Установлен протокол UDP. СДЕЛАЙТЕ TCP РЕАЛИЗАЦИЮ.");
                            break;
                        
                        default:
                            var udpServiceDefault = new UdpService();
                            Toolbox.Add(udpServiceDefault);
                            Print.LogWarning("По умолчанию выбран протокол UDP");
                            break;
                    }
                }
                else
                {
                    var udpServiceDefault = new UdpService();
                    Toolbox.Add(udpServiceDefault);
                    Print.LogWarning("По умолчанию выбран протокол UDP");
                }
                
            })).Invoke(new string[0]);
            
            commandHandler.Add(new Command("set_reciever_ip", (string[] args) =>
            {
                var networkService = Toolbox.GetTool<INetworkService>();
                
                if (networkService == null)
                {
                    Print.LogWarning("Протокол не установлен!");
                    return;
                }

                if (args.Length > 0)
                {
                    networkService.SetRecieverIp(args[0]);
                    return;
                }
                
                Print.LogError("Отсутствует аргумент адреса!");
                
            }));
            
            commandHandler.Add(new Command("set_reciever_port", (string[] args) =>
            {
                var networkService = Toolbox.GetTool<INetworkService>();
                
                if (networkService == null)
                {
                    Print.LogWarning("Протокол не установлен!");
                    return;
                }

                if (args.Length > 0)
                {
                    var numList = CommandsExtensions.ParseInt(args);

                    if (numList.Count > 0)
                    {
                        networkService.SetRecieverPort(numList[0]);
                        return;
                    }
                }
                
                Print.LogError("Отсутствует аргумент порта!");
                
            }));
            
            commandHandler.Add(new Command("set_listener_port", (string[] args) =>
            {
                var networkService = Toolbox.GetTool<INetworkService>();
                
                if (networkService == null)
                {
                    Print.LogWarning("Протокол не установлен!");
                    return;
                }

                if (args.Length > 0)
                {
                    var numList = CommandsExtensions.ParseInt(args);

                    if (numList.Count > 0)
                    {
                        networkService.SetListenerPort(numList[0]);
                        return;
                    }
                }
                
                Print.LogError("Отсутствует аргумент порта!");
                
            }));
            
            commandHandler.Add(new Command("send_ping", (string[] args) =>
            {
                var networkService = Toolbox.GetTool<INetworkService>();

                if (networkService == null)
                {
                    Print.LogWarning("Протокол не установлен!");
                    return;
                }

                networkService.Send($"ping from {Process.GetCurrentProcess().Id}");

            }));
            
            commandHandler.Add(new Command("show_msbuf", (string[] args) =>
            {
                var msbuf = Toolbox.GetTool<MessageBuffer>();

                if (msbuf == null)
                {
                    Print.LogError("Буфер сообщений отсутствует.");
                    return;
                }

                if (msbuf.Messages.Count == 0)
                {
                    Print.Log("Сообщений нет.", Color.DarkBlue);
                    return;
                }

                foreach (var str in msbuf.Query())
                {
                    Print.Log(str,Color.Blue);
                }
            }));
        }
    }

}
