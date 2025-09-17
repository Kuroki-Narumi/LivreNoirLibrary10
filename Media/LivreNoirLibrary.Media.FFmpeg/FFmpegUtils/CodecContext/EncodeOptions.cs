using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static partial class FFmpegUtils
    {
        public static void ValidateSampleRate(string paramName, int value)
        {
            if (value is <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"sample rate must be > 0. ({value})");
            }
        }

        public static void ValidateChannels(string paramName, int value)
        {
            if (value is <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"number of channels must be > 0. ({value})");
            }
        }

        public static void ValidateSize(string paramName, int value)
        {
            if (value is <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"pixel size must be > 0. ({value})");
            }
        }

        public static void ValidateFrameRate(string paramName, Rational value)
        {
            if (value.IsNegativeOrZero())
            {
                throw new ArgumentOutOfRangeException(paramName, $"frame rate must be > 0. ({value})");
            }
        }
    }
}
