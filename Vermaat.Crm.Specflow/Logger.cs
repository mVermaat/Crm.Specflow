using System;

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
