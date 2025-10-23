using System;
using System.Text;
using System.IO;
using LivreNoirLibrary.ObjectModel;
using System.Collections.Concurrent;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsTextWriter(Stream stream, bool indent, int radix, Encoding encoding) : DisposableBase
    {
        private static readonly ConcurrentDictionary<int, string> _indentPadding = [];

        private readonly StreamWriter _writer = new(stream, encoding, -1, true);
        private int _indent = indent ? 0 : -1;
        private bool _beginning_of_line = true;

        public int Radix { get; } = radix;

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _writer.Dispose();
        }

        public void IndentRight()
        {
            if (_indent >= 0)
            {
                _indent++;
            }
        }

        public void IndentLeft()
        {
            if (_indent > 0)
            {
                _indent--;
            }
        }

        private static string GetIndentPadding(int count) => _indentPadding.GetOrAdd(count, c => new string(' ', c * 2));

        private void WritePadding()
        {
            if (_beginning_of_line && _indent is > 0)
            {
                _writer.Write(GetIndentPadding(_indent));
                _beginning_of_line = false;
            }
        }

        public void Write(string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                WritePadding();
                _writer.Write(text);
            }
        }

        public void Write(string? format, params ReadOnlySpan<object?> arg)
        {
            if (!string.IsNullOrEmpty(format))
            {
                WritePadding();
                _writer.Write(format, arg);
            }
        }

        public void WriteLine()
        {
            _writer.WriteLine();
            _beginning_of_line = true;
        }

        public void WriteLine(string? str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                WritePadding();
                _writer.WriteLine(str);
                _beginning_of_line = true;
            }
        }

        public void WriteLine(string? format, params ReadOnlySpan<object?> arg)
        {
            if (!string.IsNullOrEmpty(format))
            {
                WritePadding();
                _writer.WriteLine(format, arg);
                _beginning_of_line = true;
            }
        }
    }
}
