using System;
using System.IO;

namespace LivreNoirLibrary.Media.Ogg
{
    public class OggException(string? message, Exception? innerException = null) : Exception(message, innerException)
    {
        public const string MessageFormat_Checksum = "Wrong checksum: {0:X8}(expected {1:X8})";
        public const string MessageFormat_StructureVersion = "Invalid structure version: {0}(expected 0)";
        public const string MessageFormat_PageNumberDiscontinuity = "Page number is discontinuous: {0}(expected {1})";

        public static void ThrowIfEndOfStream(int read, int expected)
        {
            if (read < expected)
            {
                throw new EndOfStreamException();
            }
        }

        public static void Verify_StructureVersion(byte actual)
        {
            if (actual is not 0)
            {
                throw new OggException(string.Format(MessageFormat_StructureVersion, actual));
            }
        }

        public static void Verify_Checksum(uint actual, uint expected)
        {
            if (actual != expected)
            {
                throw new OggException(string.Format(MessageFormat_Checksum, actual, expected));
            }
        }

        public static void Verify_PageNumber(int actual, int expected)
        {
            if (actual != expected)
            {
                throw new OggException(string.Format(MessageFormat_PageNumberDiscontinuity, actual, expected));
            }
        }

        public static void ThrowInvalidDataIf(bool condition)
        {
            if (condition)
            {
                throw new InvalidDataException();
            }
        }

        public static void ThrowInvalidOperationIf(bool condition)
        {
            if (condition)
            {
                throw new InvalidOperationException();
            }
        }

        public static void ThrowOutOfRangeIf(bool condition, string paramName)
        {
            if (condition)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }
    }
}
