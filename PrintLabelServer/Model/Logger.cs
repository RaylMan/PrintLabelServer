using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintLabelServer.Model
{
    public  class Logger
    {
        object block = new object();
        public void Log(string message)
        {
            lock (block)
            {
                using (StreamWriter sr = new StreamWriter("ErrLog.txt", true))
                {
                   sr.WriteLine($"{DateTime.Now}: {message}");
                }
            }
        }
    }
}
