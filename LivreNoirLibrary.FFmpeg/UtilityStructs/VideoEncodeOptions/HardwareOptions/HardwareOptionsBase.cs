using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe partial class HardwareOptionsBase : ObservableObjectBase, IHardwareEncodeOptions
    {
        public const int DefaultQP = -1;
        public const int QP_Min = -1;
        public const int QP_Max = 51;

        [ObservableProperty(Name = "QP")]
        protected int _qp = DefaultQP;

        public abstract string CodecSuffix { get; }
        public abstract AVHWDeviceType DeviceType { get; }
        public abstract AVPixelFormat HwPixelFormat { get; }
        public abstract bool NeedsHardwareFrame { get; }

        private static int CoerceQP(int value) => Math.Clamp(value, QP_Min, QP_Max);

        internal virtual void WriteOptions(Dictionary<string, string?> dic) { }
        void IHardwareEncodeOptions.WriteOptions(Dictionary<string, string?> dic) => WriteOptions(dic);
    }
}
