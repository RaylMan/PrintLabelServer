using PrintLabelServer.Model;
using PrintLabelServer.Server;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PrintLabelServer
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        static ServerObject server;
        static Thread listenThread;
        static PrintZPL printer;
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            try
            {
                printer = new PrintZPL();
                printer.Connect();
                if (printer.Connected)
                {
                    server = new ServerObject(printer);
                    listenThread = new Thread(new ThreadStart(server.Listen));
                    listenThread.Start();
                }
                else
                    Console.WriteLine("Принтер не найден");
            }
            catch (Exception ex)
            {
                if (server != null)
                    server.Disconnect();
                logger.Log(ex.Message);
                Console.WriteLine(ex.Message);
            }
            //ShowWindow(GetConsoleWindow(), 0);
          
            Console.ReadLine();
        }
    }
}
