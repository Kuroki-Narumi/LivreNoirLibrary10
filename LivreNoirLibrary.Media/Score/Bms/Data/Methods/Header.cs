using System;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BaseData
    {
        public void SetDefaultHeaders()
        {
            Title = "(untitled)";
            Player = Constants.DefaultPlayer;
            Bpm = Constants.DefaultBpm;
            PlayLevel = Constants.DefaultLevel;
            Difficulty = Constants.DefaultDifficulty;
            Rank = Constants.DefaultRank;
            Total = Constants.DefaultTotal;
            StageFile = Constants.DefaultStageFile;
            Banner = Constants.DefaultBanner;
            BackBmp = Constants.DefaultBackBmp;
        }

        public PlayerType Player
        {
            get => Headers.GetEnum(HeaderType.Player, Constants.DefaultPlayer);
            set
            {
                if (value is > 0)
                {
                    Headers.Set(HeaderType.Player, (int)value);
                }
                else
                {
                    Headers.Remove(HeaderType.Player);
                }
            }
        }

        public string? Genre
        {
            get => Headers.GetInherited(HeaderType.Genre);
            set => Headers.Set(HeaderType.Genre, value);
        }

        public string? Title
        {
            get => Headers.GetInherited(HeaderType.Title);
            set => Headers.Set(HeaderType.Title, value);
        }

        public string? SubTitle
        {
            get => Headers.GetInherited(HeaderType.SubTitle);
            set => Headers.Set(HeaderType.SubTitle, value);
        }

        public string? Artist
        {
            get => Headers.GetInherited(HeaderType.Artist);
            set => Headers.Set(HeaderType.Artist, value);
        }

        public string? SubArtist
        {
            get => Headers.GetInherited(HeaderType.SubArtist);
            set => Headers.Set(HeaderType.SubArtist, value);
        }

        public decimal Bpm
        {
            get => Headers.GetNumber(HeaderType.Bpm, Constants.DefaultBpm);
            set
            {
                if (value is <= 0)
                {
                    Headers.Remove(HeaderType.Bpm);
                }
                else
                {
                    Headers.Set(HeaderType.Bpm, value);
                }
            }
        }

        public int PlayLevel
        {
            get => Headers.GetNumber(HeaderType.PlayLevel, Constants.DefaultLevel);
            set => Headers.Set(HeaderType.PlayLevel, value);
        }

        public string? Difficulty
        {
            get => Headers.GetString(HeaderType.Difficulty, Constants.DefaultDifficulty);
            set => Headers.Set(HeaderType.Difficulty, value);
        }

        public Rank Rank
        {
            get => Headers.GetEnum(HeaderType.Rank, Constants.DefaultRank);
            set
            {
                if (value is >= 0)
                {
                    Headers.Set(HeaderType.Rank, (int)value);
                }
                else
                {
                    Headers.Remove(HeaderType.Rank);
                }
            }
        }

        public double Total
        {
            get => Headers.GetNumber(HeaderType.Total, 0.0);
            set
            {
                if (value is >= 0)
                {
                    Headers.Set(HeaderType.Total, value);
                }
                else
                {
                    Headers.Remove(HeaderType.Total);
                }
            }
        }

        public string? StageFile
        {
            get => Headers.GetInherited(HeaderType.StageFile);
            set => Headers.Set(HeaderType.StageFile, value);
        }

        public string? Banner
        {
            get => Headers.GetInherited(HeaderType.Banner);
            set => Headers.Set(HeaderType.Banner, value);
        }

        public string? BackBmp
        {
            get => Headers.GetInherited(HeaderType.BackBmp);
            set => Headers.Set(HeaderType.BackBmp, value);
        }

        public string? Preview
        {
            get => Headers.GetString(HeaderType.Preview, Constants.DefaultPreview);
            set => Headers.Set(HeaderType.Preview, value);
        }

        public int LnObj
        {
            get => Headers.GetInherited(HeaderType.LnObj) is string s ? s.ParseToInt(Base) : 0;
            set
            {
                if (value is > 0)
                {
                    Headers.Set(HeaderType.LnObj, value.ToBased(Base, 2));
                }
                else
                {
                    Headers.Remove(HeaderType.LnObj);
                }
            }
        }

        public LongNoteMode LnMode
        {
            get => Headers.GetEnum(HeaderType.LnMode, Constants.DefaultLnMode);
            set
            {
                if (value is > 0)
                {
                    Headers.Set(HeaderType.LnMode, (int)value);
                }
                else
                {
                    Headers.Remove(HeaderType.LnMode);
                }
            }
        }

        public double ExRank
        {
            get => Headers.GetNumber(HeaderType.DefExRank, Constants.DefaultExRank);
            set
            {
                if (value is > 0)
                {
                    Headers.Set(HeaderType.DefExRank, value);
                }
                else
                {
                    Headers.Remove(HeaderType.DefExRank);
                }
            }
        }

        public string? Comment
        {
            get => Headers.GetInherited(HeaderType.Comment);
            set => Headers.Set(HeaderType.Comment, value);
        }

        public abstract int Base { get; set; }
        public abstract int MaxDefIndex { get; }
    }
}
