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

        public AmfQuality Quality { get; set => SetValue(ref field, value); } = DefaultQuality;
        public AmfUsage Usage { get; set => SetValue(ref field, value); } = DefaultUsage;
        public AmfRateControl RateControl { get; set => SetValue(ref field, value); } = DefaultRateControl;

        public override string CodecSuffix => "amf";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_D3D11;
        public override bool NeedsHardwareFrame => false;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (Quality is not 0)
            {
                dic["quality"] = Quality.ToString();
            }
            if (QP is >= 0)
            {
                dic["qp_i"] = QP.ToString();
                dic["qp_p"] = QP.ToString();
            }
            if (Usage is not 0)
            {
                dic["usage"] = Usage.ToString();
            }
            if (RateControl is not 0)
            {
                dic["rc"] = RateControl.ToString();
            }
        }
    }
}
