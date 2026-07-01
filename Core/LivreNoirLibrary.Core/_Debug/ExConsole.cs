using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Debug
{
    public delegate void ConsoleWrittenEventHandler(string? message);

    public static class ExConsole
    {
        public static event ConsoleWrittenEventHandler? Written;

        private static readonly Lock _lock = new();
        public static ObservableCollection<LogItem> Log { get; } = [];
        public static int LineMax { get; set; } = 1000;

        public static async void Clear() => await Task.Run(ProcessClear);

        private static void ProcessClear()
        {
            lock (_lock)
            {
                Log.Clear();
            }
        }

        public static void Write(string? message) => ProcessWrite(message);
        public static async void WriteAsync(string? message) => await Task.Run(() => ProcessWrite(message));

        private static void ProcessWrite(string? message)
        {
            lock (_lock)
            {
                if (Log.Count >= LineMax)
                {
                    Log.RemoveAt(0);
                }
                Log.Add(new(message));
            }
            System.Diagnostics.Debug.WriteLine(message);
            Written?.Invoke(message);
        }

        public static void Write(params ReadOnlySpan<object?> obj)
        {
            switch (obj.Length)
            {
                case 0:
                    break;
                case 1:
                    var o = obj[0];
                    Write(o is null ? "(null)" : o.ToString());
                    break;
                default:
                    WriteWithStringBuilder(obj);
                    break;
            }
        }

        private static void WriteWithStringBuilder(ReadOnlySpan<object?> obj)
        {
            using var o1 = ObjectPool.Rent<StringBuilder>(out var sb);
            sb.Clear();
            sb.Append('(');
            sb.AppendJoin(", ", obj);
            sb.Append(')');
            Write(sb.ToString());
        }

        public static void WriteNow() => Write($"{DateTime.Now:HH:mm:ss.fff}");
        public static void WriteList<T>(Span<T> span) => Write(span.ToListString());
        public static void WriteList<T>(ReadOnlySpan<T> span) => Write(span.ToListString());
        public static void WriteList<TKey, TValue>(IDictionary<TKey, TValue> dic) => Write(dic.ToListString());
        public static void WriteList<T>(IEnumerable<T> obj) => Write(obj.ToListString());
        public static void WriteList(IEnumerable obj) => Write(obj.ToListString());

        public static void PipeFromConsole() => Console.SetOut(DebugTextWriter.Instance);

        private class DebugTextWriter : TextWriter
        {
            public static DebugTextWriter Instance { get; } = new();

            public override Encoding Encoding => Encoding.UTF8;
            public override void Write(char value) => ExConsole.Write(value.ToString());
            public override void Write(string? value) => ExConsole.Write(value);
            public override void WriteLine(string? value) => ExConsole.Write(value);
        }
    }

    public record LogItem(string? Content)
    {
        public DateTime Time { get; } = DateTime.Now;
        public string TimeText => Time.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
