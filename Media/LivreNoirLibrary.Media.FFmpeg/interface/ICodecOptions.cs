using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe interface ICodecOptions
    {
        public AVCodecID Codec { get; }
        public AVPixelFormat PixelFormat { get; }
        public AVFieldOrder FieldOrder { get; }
        public AVColorRange ColorRange { get; }
        public AVColorSpace ColorSpace { get; }
        public AVColorPrimaries ColorPrimaries { get; }
        public AVColorTransferCharacteristic ColorTransferCharacteristic { get; }
        public AVChromaLocation ChromaLocation { get; }
        public Rational GopSize { get; }
        public int MaxBFrames => 0;
        public Rational AspectRatio => Rational.One;

        Dictionary<string, string?> GetDictionary();
    }
}
