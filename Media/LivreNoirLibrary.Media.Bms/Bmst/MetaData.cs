using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LivreNoirLibrary.Media.Bmst
{
    public class MetaData: ObservableObjectBase
    {
        public const double DefaultBpm = 120;
        public const double DefaultLevel = 1;
        public const int DefaultDifficulty = 3;
        public const JudgeRank DefaultJudgeRank = JudgeRank.Normal;
        public const LongNoteMode DefaultLnMode = LongNoteMode.Auto;
        public const double DefaultJudgeWidth = 1;
        public const double DefaultVolume = 1;
        public const BgaOverflow DefaultBgaOverflow = BgaOverflow.Auto;
        public const double DefaultAutoResol = 1 / 32.0;

        public string? Genre { get; set => SetValue(ref field, value);  }
        public string? Title { get; set => SetValue(ref field, value);  }
        public string? SubTitle { get; set => SetValue(ref field, value);  }
        public string? Artist { get; set => SetValue(ref field, value);  }
        public string? SubArtist { get; set => SetValue(ref field, value);  }
        public double Bpm { get; set => SetValue(ref field, value); } = DefaultBpm;
        public string? LevelExpression { get; set => SetValue(ref field, value); } = DefaultLevel.ToString();
        public double Level { get => double.TryParse(LevelExpression, out var value) ? value : double.PositiveInfinity; set => LevelExpression = value.ToString(); }
        public string? DifficultyExpression { get; set => SetValue(ref field, value); } = DefaultDifficulty.ToString();
        public double Difficulty { get => double.TryParse(DifficultyExpression, out var value) ? value : double.PositiveInfinity; set => DifficultyExpression = value.ToString(); }
        public JudgeRank JudgeRank { get; set => SetValue(ref field, value); } = DefaultJudgeRank;
        public double JudgeWidth { get; set => SetValue(ref field, value); } = DefaultJudgeWidth;
        public double Recover { get; set => SetValue(ref field, value); }
        public LongNoteMode LnMode { get; set => SetValue(ref field, value); } = DefaultLnMode;
        public double Volume { get; set => SetValue(ref field, value); } = DefaultVolume;
        public (int Width, int Height) BgaSize { get; set => SetValue(ref field, value); }
        public BgaOverflow BgaOverflow { get; set => SetValue(ref field, value); } = DefaultBgaOverflow;
        public double AutoResol { get; set => SetValue(ref field, value); } = DefaultAutoResol;
        public string? Jacket { get; set => SetValue(ref field, value); }
        public string? Banner { get; set => SetValue(ref field, value); }
        public string? Loading { get; set => SetValue(ref field, value); }
        public string? Ready { get; set => SetValue(ref field, value); }
        public string? Preview { get; set => SetValue(ref field, value); }

        public static double ValidateBpm(double value)
        {
            if (!double.IsFinite(value) || value <= 0)
            {
                return DefaultBpm;
            }
            return value;
        }
    }
}
