using System;

namespace WebApp.MinifyCLI
{
    public class ConsoleHelper
    {
        private int _errorCount;
        private ConsoleColor  _oldColor;

        public ConsoleHelper()
        {
            _errorCount = 0;
            _oldColor = Console.ForegroundColor;
        }

        public void RestoreInitForegroundColor()
        {
            Console.ForegroundColor = _oldColor;
        }

        public int ErrorCount { get => _errorCount; }

        public void WriteError(string text)
        {
            _errorCount++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
        }
        public void WriteSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
        }

        public void WriteInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
        }
    }
}