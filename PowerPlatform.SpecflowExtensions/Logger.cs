using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public static class Logger
    {
        public static void WriteLine(string text)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {text}");
        }
    }
}
