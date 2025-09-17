using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public interface IHardwareEncodeOptions
    {
        public string CodecSuffix { get; }
        public AVHWDeviceType DeviceType { get; }
        public AVPixelFormat HwPixelFormat { get; }
        public bool NeedsHardwareFrame { get; }

        void WriteOptions(Dictionary<string, string?> dic);
    }
}
