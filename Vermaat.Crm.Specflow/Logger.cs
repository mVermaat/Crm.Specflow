using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public static class Logger
    {
        public static void WriteLine(string text)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} - {text}");
        }
    }
}
