using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe partial class CodecOptionsBase : ObservableObjectBase, ICodecOptions
    {
        public const AVPixelFormat DefaultPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;
        public const AVFieldOrder DefaultFieldOrder = AVFieldOrder.AV_FIELD_PROGRESSIVE;
        public const AVColorRange DefaultColorRange = AVColorRange.AVCOL_RANGE_MPEG;
        public const AVColorSpace DefaultColorSpace = AVColorSpace.AVCOL_SPC_BT709;
        public const AVColorPrimaries DefaultColorPrimaries = AVColorPrimaries.AVCOL_PRI_BT709;
        public const AVColorTransferCharacteristic DefaultColorTransferCharacteristic = AVColorTransferCharacteristic.AVCOL_TRC_BT709;
        public const AVChromaLocation DefaultChromaLocation = AVChromaLocation.AVCHROMA_LOC_LEFT;

        [ObservableProperty]
        protected AVPixelFormat _pixelFormat = DefaultPixelFormat;
        [ObservableProperty]
        protected AVFieldOrder _fieldOrder = DefaultFieldOrder;
        [ObservableProperty]
        protected AVColorRange _colorRange = DefaultColorRange;
        [ObservableProperty]
        protected AVColorSpace _colorSpace = DefaultColorSpace;
        [ObservableProperty]
        protected AVColorPrimaries _colorPrimaries = DefaultColorPrimaries;
        [ObservableProperty]
        protected AVColorTransferCharacteristic _colorTransferCharacteristic = DefaultColorTransferCharacteristic;
        [ObservableProperty]
        protected AVChromaLocation _chromaLocation = DefaultChromaLocation;
        [ObservableProperty]
        protected Rational _gopSize;
        [ObservableProperty]
        protected long _maxBitrate;
        [ObservableProperty]
        protected long _vbvBufferSize;

        public abstract AVCodecID Codec { get; }

        public Dictionary<string, string?> GetDictionary()
        {
            Dictionary<string, string?> dic = [];
            WriteOptions(dic);
            return dic;
        }

        internal virtual void WriteOptions(Dictionary<string, string?> dic) { }
    }
}
