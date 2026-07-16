using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class ScoreManager(IJudgeProvider judgeProvider) : ObservableObjectBase, IClear
    {
        private double _lastGauge;
        private readonly Dictionary<int, Judge> _judges = [];

        public IJudgeProvider JudgeProvider { get; set => SetValue(ref field, value); } = judgeProvider;

        public int MaxNoteCount { get; set => SetValue(ref field, value); }
        public int CurrentNoteCount { get; private set => SetValue(ref field, value); }
        public int Combo { get; set => SetValue(ref field, value); }
        public int MaxCombo { get; set => SetValue(ref field, value); }

        public double Score { get; set => SetValue(ref field, value); }
        public double Gauge { get; set => SetValue(ref field, value); }
        public double GaugeGainThreshold { get; set => SetValue(ref field, value); } = 2;

        public void Clear()
        {
            MaxNoteCount = 0;
            CurrentNoteCount = 0;
            foreach (var (_, judge) in _judges)
            {
                judge.Clear();
            }
            Combo = 0;
            MaxCombo = 0;
            Score = 0;
            Gauge = JudgeProvider.GaugeDefinition.InitialValue;
            _lastGauge = Gauge;
        }

        public void UpdateJudge(IBmsTimer timer, double absoluteTime, double duration, in JudgeInfo info)
        {
            switch (info.ComboChange)
            {
                case ComboChange.Increase:
                    Combo++;
                    MaxCombo = Math.Max(MaxCombo, Combo);
                    if (++CurrentNoteCount == MaxNoteCount)
                    {
                        timer.Set(TimerId.Play_FullCombo, absoluteTime);
                    }
                    break;
                case ComboChange.Reset:
                    Combo = 0;
                    break;
            }

            var combo = Combo;
            var type = info.Type;
            var player = info.Player;
            var error = info.Error;
            UpdateJudgeDisplay(timer, 0, combo, type, absoluteTime, duration, error);
            UpdateJudgeDisplay(timer, player, combo, type, absoluteTime, duration, error);

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

        private void UpdateJudgeDisplay(IBmsTimer timer, int player, int combo, JudgeType type, double absoluteTime, double duration, double error)
        {
            _judges.GetOrAdd(player, p => new(p)).Update(timer, combo, type, absoluteTime, duration, error);
        }

        public bool TryGetPlayerJudge(int player, [MaybeNullWhen(false)] out Judge judge) => _judges.TryGetValue(player, out judge);

        public class Judge(int player) : ObservableObjectBase, IClear
        {
            public int Player { get; } = player;
            public int Combo { get; private set => SetValue(ref field, value); }
            public JudgeType Type { get; private set => SetValue(ref field, value); }
            public double LastOccurred { get; private set => SetValue(ref field, value); }
            public double Limit { get; private set => SetValue(ref field, value); }

            public void Clear()
            {
                Combo = 0;
                Type = 0;
                LastOccurred = 0;
                Limit = 0;
            }

            public void Update(IBmsTimer timer, int combo, JudgeType type, double absoluteTime, double duration, double error)
            {
                if (absoluteTime >= LastOccurred)
                {
                    Combo = combo;
                    Type = type;
                    LastOccurred = absoluteTime;
                    Limit = Math.Max(Limit, absoluteTime + duration);
                    timer.SetJudgeTimer(absoluteTime, Player, error);
                }
            }
        }
    }
}
