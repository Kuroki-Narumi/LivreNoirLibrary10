using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;
using System;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BmsVideoCreateOptions : BmsPlayOptions
    {
        public Rational FrameRate { get; set => SetValue(ref field, value); } = FrameRates.Fps60;
        public bool IsHevc { get; set => SetValue(ref field, value); } = false;
        public int QP { get; set => SetValue(ref field, Math.Clamp(value, HardwareOptionsBase.QP_Min, HardwareOptionsBase.QP_Max)); } = 41;
        public int ApproximateKbps { get; set => SetValue(ref field, value); } = 0;

        public double FadeInDuration { get; set => SetValue(ref field, value); } = 0.5;
        public double LoadDuration { get; set => SetValue(ref field, value); } = 2;
        public double ReadyDuration { get; set => SetValue(ref field, value); } = 1;
        public double AfterMargin { get; set => SetValue(ref field, value); } = 3;
        public double FadeOutDuration { get; set => SetValue(ref field, value); } = 1;
    }
}
