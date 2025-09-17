using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe partial class HardwareOptionsBase : ObservableObjectBase, IHardwareEncodeOptions
    {
        public const int DefaultQP = -1;
        public const int QP_Min = -1;
        public const int QP_Max = 51;

        public int QP { get; set => SetValue(ref field, Math.Clamp(value, QP_Min, QP_Max)); } = DefaultQP;

        public abstract string CodecSuffix { get; }
        public abstract AVHWDeviceType DeviceType { get; }
        public abstract AVPixelFormat HwPixelFormat { get; }
        public abstract bool NeedsHardwareFrame { get; }

        internal virtual void WriteOptions(Dictionary<string, string?> dic) { }
        void IHardwareEncodeOptions.WriteOptions(Dictionary<string, string?> dic) => WriteOptions(dic);
    }
}
