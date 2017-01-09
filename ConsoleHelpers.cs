using System;

namespace BallInChair
{
    public static class ConsoleHelpers
    {
        public static void WriteRedLine(string value) => WriteColoredLine(value, ConsoleColor.Red);
        public static void WriteGreenLine(string value) => WriteColoredLine(value, ConsoleColor.Green);

        private static void WriteColoredLine(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}