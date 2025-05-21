using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed partial class NvencEncodeOptions : HardwareOptionsBase
    {
        public const CudaPreset DefaultPreset = CudaPreset.p5;
        public const CudaRateControl DefaultRateControl = CudaRateControl.vbr;
        public const CudaTier DefaultTier = CudaTier.main;
        public const int RcLookAhead_Min = 0;
        public const int RcLookAhead_Max = 32;

        [ObservableProperty]
        private CudaPreset _preset = DefaultPreset;
        [ObservableProperty]
        private CudaRateControl _rateControl = DefaultRateControl;
        [ObservableProperty]
        private bool _spatialAq;
        [ObservableProperty]
        private bool _temporalAq;
        [ObservableProperty]
        private int _rcLookAhead;
        [ObservableProperty]
        private bool _zeroLatency;
        [ObservableProperty]
        private CudaTier _tier = DefaultTier;
        [ObservableProperty]
        private bool _weightedPred;

        public override string CodecSuffix => "nvenc";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_CUDA;
        public override bool NeedsHardwareFrame => true;

        private static int CoerceRcLookAhead(int value) => Math.Clamp(value, RcLookAhead_Min, RcLookAhead_Max);

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (_preset is 0)
            {
                dic.Remove("preset");
            }
            else
            {
                dic["preset"] = _preset.ToString();
            }
            dic["rc"] = _rateControl.ToString();
            if (_rateControl is CudaRateControl.constqp && _qp is >= 0)
            {
                dic["qp"] = _qp.ToString();
            }
            if (_spatialAq)
            {
                dic["spatial-aq"] = "1";
            }
            if (_temporalAq)
            {
                dic["temporal-aq"] = "1";
            }
            if (_rcLookAhead is >= 0)
            {
                dic["rc-lookahead"] = _rcLookAhead.ToString();
            }
            if (_tier is not 0)
            {
                dic["tier"] = _tier.ToString();
            }
            if (_weightedPred)
            {
                dic["weighted_pred"] = "1";
            }
        }
    }
}
