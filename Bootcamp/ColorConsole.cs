using System;
using System.Collections.Generic;
using System.Text;

namespace Bootcamp
{
    public class ColorConsole
    {
        private static void WriteLineWithColor(string message, ConsoleColor color)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }

        public static void WriteLineGreen(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Green);
        }

        public static void WriteLineCyan(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Cyan);
        }

        public static void WriteLineYellow(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Yellow);
        }

        public static void WriteLineRed(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Red);
        }

        public static void WriteLineGray(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Gray);
        }

        public static void WriteMagenta(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Magenta);
        }
    }
}
