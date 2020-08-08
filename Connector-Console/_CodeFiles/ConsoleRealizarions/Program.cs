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
using System.Net;

namespace Connector.ConsoleRealizarions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Initialize();
            InitialzieLoops();

            LoopManager.StartAll();
            LoopManager.WaitAll(5000);
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

            commandHandler.Add(new Command("go_fuck", (string[] args) => { Print.Log("Я работаю");  }));

            readInputLoop.Subscribe(commandHandler.Handle);
            
            // Цикл чтения сокетов
            var readNetworkLoop = ReadNetworkLoop.Create();

            var messageBuffer = Toolbox.GetTool<MessageBuffer>();
            if (messageBuffer == null)
            {
                messageBuffer = MessageBuffer.Create("network_buffer");
                Toolbox.Add(messageBuffer);
            }
                

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
                IPAddress recieverIp = null;
                int recieverPort = -1;
                int listenerPort = -1;

                if (networkService != null)
                {
                    recieverIp = networkService.RecieverIp;
                    recieverPort = networkService.RecieverPort;
                    listenerPort = networkService.ListenerPort;

                    networkService.Dispose();
                    Toolbox.Remove(networkService);
                }
                else
                {
                    recieverIp = IPAddress.Loopback;
                }

                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "udp":
                            var udpService = new UdpService();
                            udpService.SetRecieverInfo(recieverIp.ToString(), recieverPort);

                            if (listenerPort >= 0)
                                udpService.SetListenerPort(listenerPort);

                            Toolbox.Add(udpService);
                            Print.Log("Установлен протокол UDP", Color.DarkBlue);
                            break;
                        
                        case "tcp":
                            var tcpService = new TcpService();
                            tcpService.SetRecieverInfo(recieverIp.ToString(), recieverPort);

                            if (listenerPort >= 0)
                                tcpService.SetListenerPort(listenerPort);

                            Toolbox.Add(tcpService);
                            Print.Log("Установлен протокол TCP", Color.DarkBlue);
                            break;
                        
                        default:
                            var udpServiceDefault = new UdpService();
                            udpServiceDefault.SetRecieverInfo(recieverIp.ToString(), recieverPort);

                            if (listenerPort >= 0)
                                udpServiceDefault.SetListenerPort(listenerPort);

                            Toolbox.Add(udpServiceDefault);
                            Print.LogWarning("По умолчанию выбран протокол UDP");
                            break;
                    }
                }
                else
                {
                    var udpServiceDefault = new UdpService();
                    udpServiceDefault.SetRecieverInfo(recieverIp.ToString(), recieverPort);

                    if (listenerPort >= 0)
                        udpServiceDefault.SetListenerPort(listenerPort);

                    Toolbox.Add(udpServiceDefault);
                    Print.LogWarning("По умолчанию выбран протокол UDP");
                }
                
            })).Invoke(new string[] { "tcp" });
            
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

                foreach (var str in msbuf.QueryQueue())
                {
                    Print.Log(str,Color.Blue);
                }
            }));
        }
    }

}
