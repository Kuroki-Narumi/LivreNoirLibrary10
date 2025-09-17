using System;

namespace LivreNoirLibrary.Media.Wave
{
    public enum SampleFormat : byte
    {
        Float32 = 0,
        UInt8 = 8,
        Int16 = 16,
        Int24 = 24,
        Int32 = 32,
        Int48 = 48,
        Int64 = 64,

        Invalid = 255,
    }

    public static class SampleFormatExtensions
    {
        public static bool IsValid(this SampleFormat format) => (int)format % 8 is 0;
    }
}
