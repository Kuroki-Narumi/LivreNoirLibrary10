using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsParser
    {
        void InitializeParse(int radix, long lnObj) { }
        void OnLineUnprocessed(ReadOnlySpan<char> line) { }
        void OnLineProcessed(int lineNumber) { }
        void FinalizeParse() { }

        void AddComment(ReadOnlySpan<char> line) { }

        void StartRandom(int value);
        void StartSetRandom(int value);
        void StartIf(int value);
        void StartElseIf(int value);
        void StartElse();
        void EndIf();
        void EndRandom();

        void StartSwitch(int value);
        void StartSetSwitch(int value);
        void StartCase(int value);
        void StartDefault();
        void Skip();
        void EndSwitch();

        void AddHeader(ReadOnlySpan<char> key, ReadOnlySpan<char> value);

        void AddDef(DefType type, long key, string value);
        void AddConductorDef(DefType type, long key, decimal value);

        void AddBar(int number, Channel channel, ReadOnlySpan<char> value);
    }

    public static partial class IBmsParserExtensions
    {
        public static void Parse(this IBmsParser parser, string path)
        {
            BmsTextReader reader;
            using (var file = File.OpenRead(path))
            {
                reader = new(file);
            }
            reader.Parse(parser);
        }

        public static void Parse(this IBmsParser parser, Stream stream)
        {
            BmsTextReader reader = new(stream);
            reader.Parse(parser);
        }
    }
}
