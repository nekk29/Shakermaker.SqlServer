using System;

namespace Shakermaker.SqlServer.Core.Utils
{
    public class Logger
    {
        public static void Reset()
        {
            Console.ResetColor();
        }

        public static void Log(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
        }

        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
        }

        public static void LogErrorObject(object error)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(error);
        }
    }
}
