
namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed partial class QsvEncodeOptions : HardwareOptionsBase, IHardwareEncodeOptions
    {
        public const QsvPreset DefaultPreset = QsvPreset.medium;

        public QsvPreset Preset { get; set => SetValue(ref field, value); } = DefaultPreset;
        public bool LowPower { get; set => SetValue(ref field, value); }
        public bool UseOpaque { get; set => SetValue(ref field, value); } // Opaqueモード制御

        public override string CodecSuffix => "qsv";
        public override AVHWDeviceType DeviceType => AVHWDeviceType.AV_HWDEVICE_TYPE_QSV;
        public override AVPixelFormat HwPixelFormat => AVPixelFormat.AV_PIX_FMT_QSV;
        public override bool NeedsHardwareFrame => !UseOpaque;

        internal override void WriteOptions(Dictionary<string, string?> dic)
        {
            base.WriteOptions(dic);
            if (Preset is not 0)
            {
                dic["preset"] = Preset.ToString();
            }
            if (QP > 0)
            {
                dic["global_quality"] = QP.ToString();
            }
            if (LowPower)
            {
                dic["low_power"] = "1";
            }
            if (UseOpaque)
            {
                dic["opaque"] = "1";
            }
        }
    }
}
