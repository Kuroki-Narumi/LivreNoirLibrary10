using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed partial class AmfEncodeOptions : HardwareOptionsBase, IHardwareEncodeOptions
    {
        public const AmfQuality DefaultQuality = AmfQuality.balanced;
        public const AmfUsage DefaultUsage = AmfUsage.transcoding;
        public const AmfRateControl DefaultRateControl = AmfRateControl.cbr;

        [ObservableProperty]
        private AmfQuality _quality = DefaultQuality;
        [ObservableProperty]
        private AmfUsage _usage = DefaultUsage;
        [ObservableProperty]
        private AmfRateControl _rate_control = DefaultRateControl;

        public override string CodecSuffix => "amf";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_D3D11;
        public override bool NeedsHardwareFrame => false;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (_quality is not 0)
            {
                dic["quality"] = _quality.ToString();
            }
            if (_qp is >= 0)
            {
                dic["qp_i"] = _qp.ToString();
                dic["qp_p"] = _qp.ToString();
            }
            if (_usage is not 0)
            {
                dic["usage"] = _usage.ToString();
            }
            if (_rate_control is not 0)
            {
                dic["rc"] = _rate_control.ToString();
            }
        }
    }
}
