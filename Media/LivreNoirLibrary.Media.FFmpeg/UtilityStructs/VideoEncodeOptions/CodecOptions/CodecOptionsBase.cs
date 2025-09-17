using System;
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

        public AVPixelFormat PixelFormat { get; set => SetValue(ref field, value); } = DefaultPixelFormat;
        public AVFieldOrder FieldOrder { get; set => SetValue(ref field, value); } = DefaultFieldOrder;
        public AVColorRange ColorRange { get; set => SetValue(ref field, value); } = DefaultColorRange;
        public AVColorSpace ColorSpace { get; set => SetValue(ref field, value); } = DefaultColorSpace;
        public AVColorPrimaries ColorPrimaries { get; set => SetValue(ref field, value); } = DefaultColorPrimaries;
        public AVColorTransferCharacteristic ColorTransferCharacteristic { get; set => SetValue(ref field, value); } = DefaultColorTransferCharacteristic;
        public AVChromaLocation ChromaLocation { get; set => SetValue(ref field, value); } = DefaultChromaLocation;
        public Rational GopSize { get; set => SetValue(ref field, value); }
        public long MaxBitrate { get; set => SetValue(ref field, value); }
        public long VbvBufferSize { get; set => SetValue(ref field, value); }

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
