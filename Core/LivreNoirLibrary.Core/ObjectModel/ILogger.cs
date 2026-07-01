using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface ILogger
    {
        public static ILogger Default { get; } = new DefaultLogger();

        void Write(string message);
        void WriteLine(string message);
        void Error(Exception exception);

        private class DefaultLogger : ILogger
        {
            public void Write(string message) => Console.Write(message);
            public void WriteLine(string message) => Console.WriteLine(message);
            public void Error(Exception exception) => Console.WriteLine(exception.ToString());
        }
    }
}
