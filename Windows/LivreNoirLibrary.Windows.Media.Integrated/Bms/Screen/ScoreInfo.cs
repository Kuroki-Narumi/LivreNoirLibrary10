using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class ScoreInfo : ObservableObjectBase
    {
        private double _lastGage;

        public double DisplayTime { get; set => SetValue(ref field, value); } = 0.5;

        public double InitialGage { get; set => SetValue(ref field, value); } = 22;
        public double MinGage { get; set => SetValue(ref field, value); } = 0;
        public double MaxGage { get; set => SetValue(ref field, value); } = 100;
        public double GageGainThreshold { get; set => SetValue(ref field, value); } = 2;
        public double GageLowThreshold { get; set => SetValue(ref field, value); } = 20;
        public double GageHighThreshold { get; set => SetValue(ref field, value); } = 80;

        public int Combo { get; set => SetValue(ref field, value); }
        public int MaxCombo { get; set => SetValue(ref field, value); }
        public JudgeType JudgeType { get; set => SetValue(ref field, value); }
        public double LastJudgeTime { get; set => SetValue(ref field, value); }
        public bool IsActive { get; set => SetValue(ref field, value); }

        public double Score { get; set => SetValue(ref field, value); }
        public double Gage { get; set => SetValue(ref field, value); }

        public void Clear()
        {
            Combo = 0;
            MaxCombo = 0;
            JudgeType = 0;
            LastJudgeTime = 0;
            Score = 0;
            Gage = InitialGage;
            _lastGage = Gage;
        }

        public void UpdateJudge(BmsTimer timer, double absoluteTime, in JudgeInfo info)
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
                timer.SetJudgeTimer(absoluteTime, type, 1, 0);
            }

            Score += info.ScoreGain;

            var gage = Gage;
            var max = MaxGage;
            var newGage = Math.Clamp(gage + info.GageGain, MinGage, max);
            if (gage != newGage)
            {
                Gage = newGage;

                var lowTh = GageLowThreshold;
                if (newGage >= lowTh)
                {
                    timer.Remove(TimerId.Play_Gage_Low);
                }
                else if (gage >= lowTh)
                {
                    timer.Set(TimerId.Play_Gage_Low, absoluteTime);
                }

                var highTh = GageHighThreshold;
                if (newGage < highTh)
                {
                    timer.Remove(TimerId.Play_Gage_High);
                }
                else if (gage < highTh)
                {
                    timer.Set(TimerId.Play_Gage_High, absoluteTime);
                }

                if (newGage < max)
                {
                    timer.Remove(TimerId.Play_Gage_Max);
                }
                else if (gage < max)
                {
                    timer.Set(TimerId.Play_Gage_Max, absoluteTime);
                }

                var gainTh = GageGainThreshold;
                var stepped = Math.Truncate(newGage / gainTh) * gainTh;
                if (stepped > _lastGage)
                {
                    timer.Set(TimerId.Play_Gage_Gain, absoluteTime);
                }
                _lastGage = stepped;
            }
        }
    }
}
