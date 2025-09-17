using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsParseException(int lineNumber, string line, Exception? innerException) : Exception(CreateMessage(lineNumber, line), innerException)
    {
        public int LineNumber { get; } = lineNumber;
        public string Line { get; } = line;

        private static string CreateMessage(int lineNumber, string line) => $"BMS parse failed at line {lineNumber}:\"{line}\"";
    }
}
