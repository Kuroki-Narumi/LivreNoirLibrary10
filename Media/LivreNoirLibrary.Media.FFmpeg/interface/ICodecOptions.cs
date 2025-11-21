using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe interface ICodecOptions
    {
        AVCodecID Codec { get; }
        AVPixelFormat PixelFormat { get; }
        AVFieldOrder FieldOrder { get; }
        AVColorRange ColorRange { get; }
        AVColorSpace ColorSpace { get; }
        AVColorPrimaries ColorPrimaries { get; }
        AVColorTransferCharacteristic ColorTransferCharacteristic { get; }
        AVChromaLocation ChromaLocation { get; }
        Rational GopSize { get; }
        int MaxBFrames => 0;
        Rational AspectRatio => Rational.One;

        Dictionary<string, string?> GetDictionary();
    }
}
