using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace gmp
{
    class Server
    {
        private TcpListener Listener;

        static void Main(string[] args)
        {
            try
            {
                new Server(Properties.Settings.Default.TcpPort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        // запуск сервера
        public Server(int Port)
        {
            Console.WriteLine("Initialization...");

            Console.Title = Properties.Settings.Default.Title.Trim() + " (Control-X to stop)";

            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Protocol).TypeHandle);

            DbConnect DbConnect = DbConnect.CreateConnection();

            DbConnect.OpenConnection();
            DbConnect.CloseConnection();

            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Server.ReceiveTimeout = Properties.Settings.Default.TcpTimeout;
            Listener.Server.SendTimeout = Properties.Settings.Default.TcpTimeout;
            Listener.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                if (Console.KeyAvailable) 
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                    if (KeyInfo.Key == ConsoleKey.X && KeyInfo.Modifiers == ConsoleModifiers.Control) break;
                }

                if (Listener.Pending())
                {
                    //Thread Thread = new Thread(StartClient);
                    ////Thread.IsBackground = true;
                    //Thread.Start(Listener.AcceptTcpClient());

                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartClient), Listener.AcceptTcpClient());
                }
            }

            Console.WriteLine("Finishing...");

            if (Listener != null)
            {
                Listener.Stop();
            }

            while (true)
            {
                int maxWorkerThreads, maxIOThreads;
                int availableWorkerThreads, availableIOThreads;

                ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIOThreads);
                ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableIOThreads);

                if (maxWorkerThreads == availableWorkerThreads) break;

                System.Threading.Thread.Sleep(Properties.Settings.Default.TcpTimeout / 10);
            }
        }

        // запуск клиента в новом потоке
        static void StartClient(Object Client)
        {
            new Client((TcpClient)Client);
        }
    }
    
    class Client
    {
        private DbConnect DbConnect;

        public Client(TcpClient Client)
        {
            byte[] Buffer = new byte[Constant.BufferSize];

            IncomingPacket IncomingPacket;
            OutgoingPacket OutgoingPacket;
            
            int Count;

            try
            {
                DbConnect = DbConnect.CreateConnection();
                
                DbConnect.OpenConnection();

                while (true)
                {
                    // новый входящий пакет
                    IncomingPacket = new IncomingPacket();

                    // получаем префикс
                    Count = Client.GetStream().Read(Buffer, 0, Constant.PrefixSize);
                    if (Count < Constant.PrefixSize) throw new Exception("Prefix read error");
                    
                    // декодируем префикс
                    IncomingPacket.DecodePrefix(Buffer);
                    // записываем префикс в базу
                    IncomingPacket.WritePrefix(DbConnect);

                    // получаем блоки данных
                    Count = Client.GetStream().Read(Buffer, Constant.PrefixSize, IncomingPacket.Size - Constant.PrefixSize);
                    if (Count < IncomingPacket.Size - Constant.PrefixSize) throw new Exception("Packet read error");
                    
                    // разбиваем буфер на блоки
                    IncomingPacket.DecodeData(Buffer);
                    // декодируем блоки и записываем данные блоков в базу
                    IncomingPacket.WriteData(DbConnect);
                    
                    // новый исходящий пакет
                    OutgoingPacket = new OutgoingPacket(IncomingPacket);

                    // записываем данные для тестового запроса
                    //OutgoingPacket.WriteTest(DbConnect);
                    // читаем данные для запроса, ищем подходящую квитанцию, кодируем запрос
                    OutgoingPacket.ReadData(DbConnect);

                    // кодируем квитанцию и помещаем результат в буфер
                    OutgoingPacket.EncodeData(Buffer);
                    // кодируем префикс и помещаем результат в буфер
                    OutgoingPacket.EncodePrefix(Buffer);

                    // отправляем исходящий пакет
                    Client.GetStream().Write(Buffer, 0, OutgoingPacket.Size);

                    // отмечаем отправленный запрос
                    OutgoingPacket.WriteData(DbConnect, 2);
                }
            }
            catch (Exception e)
            {
                if (e.HResult == Constant.ExceptionDisconnect)
                {
                    // не ошибка
                }
                else if (e.HResult == Constant.ExceptionCRC)
                {
                    Console.WriteLine(e.Message);
                }
                else
                {
                    Console.WriteLine(e.Message);
                }
            }

            try
            {
                DbConnect.CloseConnection();

                Client.Close();

                //Console.WriteLine("Client disconnected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
