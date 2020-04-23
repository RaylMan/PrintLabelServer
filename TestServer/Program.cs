using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Random t = new Random();
            List<Task<TCPClient>> tasks = new List<Task<TCPClient>>();
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(GetClient());
            }
            try
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    Thread.Sleep(t.Next(0,20));
                    tasks[i].Result.SendMessage($"Thread {i}");
                }
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }
           
            Console.ReadLine();
        }
        static Task<TCPClient> GetClient()
        {
            return Task.Factory.StartNew(() =>new TCPClient());
        }
    }
}
