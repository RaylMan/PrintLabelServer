using PrintLabelServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrintLabelServer.Server
{
    class ServerObject
    {
        object block = new object();
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения
        PrintZPL printer;
        Logger logger = new Logger();
        public ServerObject(PrintZPL printer)
        {
            this.printer = printer;
        }
        protected internal void AddConnection(ClientObject clientObject)
        {
            lock (block)
            {
                clients.Add(clientObject);
            }
        }
        protected internal void RemoveConnection(string id)
        {
            lock (block)
            {
                // получаем по id закрытое подключение
                ClientObject client = clients.FirstOrDefault(c => c.Id == id);
                // и удаляем его из списка подключений
                if (client != null)
                    clients.Remove(client);
            }
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                logger.Log("Сервер запущен!");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this, printer);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                Disconnect();
            }
        }
        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}
