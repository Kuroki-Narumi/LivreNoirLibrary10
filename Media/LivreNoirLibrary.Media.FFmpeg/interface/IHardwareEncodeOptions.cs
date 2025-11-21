using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public interface IHardwareEncodeOptions
    {
        string CodecSuffix { get; }
        AVHWDeviceType DeviceType { get; }
        AVPixelFormat HwPixelFormat { get; }
        bool NeedsHardwareFrame { get; }

        void WriteOptions(Dictionary<string, string?> dic);
    }
}
