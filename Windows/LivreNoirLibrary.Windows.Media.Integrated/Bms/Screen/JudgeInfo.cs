using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class JudgeInfo : ObservableObjectBase
    {
        public double DisplayTime { get; set => SetValue(ref field, value); } = 0.5;

        public int Combo { get; set => SetValue(ref field, value); }
        public JudgeType Type { get; set => SetValue(ref field, value); }
        public double LastTime { get; set => SetValue(ref field, value); }
        public bool IsActive { get; set => SetValue(ref field, value); }

        public void Clear()
        {
            Combo = 0;
            Type = 0;
            LastTime = 0;
        }

        public void UpdateJudge(BmsTimer timer, double absoluteTime, JudgeType type, ComboChange comboChange = ComboChange.Continue)
        {
            switch (comboChange)
            {
                case ComboChange.Increase:
                    Combo++;
                    break;
                case ComboChange.Reset:
                    Combo = 0;
                    break;
            }
            if (absoluteTime > LastTime)
            {
                Type = type;
                LastTime = absoluteTime;
                timer.SetJudgeTimer(absoluteTime, type, 1, 0);
            }
        }
    }
}
