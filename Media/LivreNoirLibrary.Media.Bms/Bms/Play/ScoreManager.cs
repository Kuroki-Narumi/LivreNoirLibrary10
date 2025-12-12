using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class ScoreManager(IJudgeProvider judgeProvider) : ObservableObjectBase
    {
        private double _lastGauge;

        public IJudgeProvider JudgeProvider { get; set => SetValue(ref field, value); } = judgeProvider;

        public int Combo { get; set => SetValue(ref field, value); }
        public int MaxCombo { get; set => SetValue(ref field, value); }
        public JudgeType JudgeType { get; set => SetValue(ref field, value); }
        public double LastJudgeTime { get; set => SetValue(ref field, value); }
        public bool IsJudgeActive { get; set => SetValue(ref field, value); }

        public double Score { get; set => SetValue(ref field, value); }
        public double Gauge { get; set => SetValue(ref field, value); }
        public double GaugeGainThreshold { get; set => SetValue(ref field, value); } = 2;

        public void Clear()
        {
            Combo = 0;
            MaxCombo = 0;
            JudgeType = 0;
            LastJudgeTime = 0;
            Score = 0;
            Gauge = JudgeProvider.GaugeDefinition.InitialValue;
            _lastGauge = Gauge;
        }

        public void UpdateJudge(IBmsTimer timer, double absoluteTime, in JudgeInfo info)
        {
            switch (info.ComboChange)
            {
                case ComboChange.Increase:
                    Combo++;
                    MaxCombo = Math.Max(MaxCombo, Combo);
                    break;
                case ComboChange.Reset:
                    Combo = 0;
                    break;
            }

            if (absoluteTime > LastJudgeTime)
            {
                var type = info.Type;
                JudgeType = type;
                LastJudgeTime = absoluteTime;
                timer.SetJudgeTimer(absoluteTime, type, 1, info.Error);
            }

            Score += info.ScoreGain;

            var gaugeDef = JudgeProvider.GaugeDefinition;
            var gage = Gauge;
            var max = gaugeDef.MaximumValue;
            var newGauge = Math.Clamp(gage + info.GaugeGain, gaugeDef.MinimumValue, max);
            if (gage != newGauge)
            {
                Gauge = newGauge;

                var lowTh = gaugeDef.LowValue;
                if (newGauge >= lowTh)
                {
                    timer.Remove(TimerId.Play_Gauge_Low);
                }
                else if (gage >= lowTh)
                {
                    timer.Set(TimerId.Play_Gauge_Low, absoluteTime);
                }

                var highTh = gaugeDef.PassingValue;
                if (newGauge < highTh)
                {
                    timer.Remove(TimerId.Play_Gauge_High);
                }
                else if (gage < highTh)
                {
                    timer.Set(TimerId.Play_Gauge_High, absoluteTime);
                }

                if (newGauge < max)
                {
                    timer.Remove(TimerId.Play_Gauge_Max);
                }
                else if (gage < max)
                {
                    timer.Set(TimerId.Play_Gauge_Max, absoluteTime);
                }

                var gainTh = GaugeGainThreshold;
                var stepped = Math.Truncate(newGauge / gainTh) * gainTh;
                if (stepped > _lastGauge)
                {
                    timer.Set(TimerId.Play_Gauge_Gain, absoluteTime);
                }
                _lastGauge = stepped;
            }
        }
    }
}
