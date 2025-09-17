
namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed partial class NvencEncodeOptions : HardwareOptionsBase
    {
        public const CudaPreset DefaultPreset = CudaPreset.p5;
        public const CudaRateControl DefaultRateControl = CudaRateControl.vbr;
        public const CudaTier DefaultTier = CudaTier.main;
        public const int RcLookAhead_Min = 0;
        public const int RcLookAhead_Max = 32;


        public CudaPreset Preset { get; set => SetValue(ref field, value); } = DefaultPreset;
        public CudaRateControl RateControl { get; set => SetValue(ref field, value); } = DefaultRateControl;
        public bool SpatialAq { get; set => SetValue(ref field, value); }
        public bool TemporalAq { get; set => SetValue(ref field, value); }
        public int RcLookAhead { get; set => SetValue(ref field, Math.Clamp(value, RcLookAhead_Min, RcLookAhead_Max)); }
        public bool ZeroLatency { get; set => SetValue(ref field, value); }
        public CudaTier Tier { get; set => SetValue(ref field, value); } = DefaultTier;
        public bool WeightedPred { get; set => SetValue(ref field, value); }

        public override string CodecSuffix => "nvenc";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_CUDA;
        public override bool NeedsHardwareFrame => true;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (Preset is 0)
            {
                dic.Remove("preset");
            }
            else
            {
                dic["preset"] = Preset.ToString();
            }
            dic["rc"] = RateControl.ToString();
            if (RateControl is CudaRateControl.constqp && QP is >= 0)
            {
                dic["qp"] = QP.ToString();
            }
            if (SpatialAq)
            {
                dic["spatial-aq"] = "1";
            }
            if (TemporalAq)
            {
                dic["temporal-aq"] = "1";
            }
            if (RcLookAhead is >= 0)
            {
                dic["rc-lookahead"] = RcLookAhead.ToString();
            }
            if (Tier is not 0)
            {
                dic["tier"] = Tier.ToString();
            }
            if (WeightedPred)
            {
                dic["weighted_pred"] = "1";
            }
        }
    }
}
