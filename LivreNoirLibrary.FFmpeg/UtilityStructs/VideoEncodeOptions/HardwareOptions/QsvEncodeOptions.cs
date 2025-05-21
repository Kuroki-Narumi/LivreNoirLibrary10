using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed partial class QsvEncodeOptions : HardwareOptionsBase, IHardwareEncodeOptions
    {
        public const QsvPreset DefaultPreset = QsvPreset.medium;

        [ObservableProperty]
        private QsvPreset _preset = DefaultPreset;
        [ObservableProperty]
        private bool _lowPower;
        [ObservableProperty]
        private bool _useOpaque; // Opaqueモード制御

        public override string CodecSuffix => "qsv";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_QSV;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_QSV;
        public override bool NeedsHardwareFrame => !_useOpaque;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (_preset is not 0)
            {
                dic["preset"] = _preset.ToString();
            }
            if (_qp > 0)
            {
                dic["global_quality"] = _qp.ToString();
            }
            if (_lowPower)
            {
                dic["low_power"] = "1";
            }
            if (_useOpaque)
            {
                dic["opaque"] = "1";
            }
        }
    }
}
