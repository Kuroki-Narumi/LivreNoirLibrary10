using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        extension(IBmsData data)
        {
            public bool TryGetDoubleHeader(HeaderType type, out double value)
            {
                if (data.Headers.TryGetNumber(type, out value))
                {
                    return true;
                }
                if (data.Parent is { } parent)
                {
                    return parent.TryGetDoubleHeader(type, out value);
                }
                value = default;
                return false;
            }

            public bool TryGetIntHeader(HeaderType type, out int value)
            {
                if (data.Headers.TryGetNumber(type, out var dValue))
                {
                    value = (int)dValue;
                    return true;
                }
                if (data.Parent is { } parent)
                {
                    return parent.TryGetIntHeader(type, out value);
                }
                value = default;
                return false;
            }

            public bool TryGetEnumHeader<T>(HeaderType type, out T value)
                where T : struct, Enum
            {
                if (data.Headers.TryGetEnum(type, out value))
                {
                    return true;
                }
                if (data.Parent is { } parent)
                {
                    return parent.TryGetEnumHeader(type, out value);
                }
                value = default;
                return false;
            }

            public bool TryGetTextHeader(HeaderType type, [MaybeNullWhen(false)] out string value)
            {
                if (data.Headers.TryGetText(type, out value))
                {
                    return true;
                }
                if (data.Parent is { } parent)
                {
                    return parent.TryGetTextHeader(type, out value);
                }
                value = default;
                return false;
            }

            public double GetDoubleHeader(HeaderType type, double ifNone = default) => data.TryGetDoubleHeader(type, out var value) ? value : ifNone;
            public int GetIntHeader(HeaderType type, int ifNone = default) => data.TryGetIntHeader(type, out var value) ? value : ifNone;
            public T GetEnumHeader<T>(HeaderType type, T ifNone = default) where T : struct, Enum => data.TryGetEnumHeader(type, out T value) ? value : ifNone;
            [return: NotNullIfNotNull(nameof(ifNone))]
            public string? GetTextHeader(HeaderType type, string? ifNone = null) => data.TryGetTextHeader(type, out var value) ? value : ifNone;

            public void SetHeader(HeaderType type, double value)
            {
                if (data.Parent is { } parent && parent.TryGetDoubleHeader(type, out var current) && current == value)
                {
                    data.Headers.Remove(type);
                }
                else
                {
                    data.Headers.Set(type, value);
                }
            }

            public void SetHeader<T>(HeaderType type, T value)
                where T : struct, Enum
            {
                if (data.Parent is { } parent && parent.TryGetEnumHeader(type, out T current) && current.Equals(value))
                {
                    data.Headers.Remove(type);
                }
                else
                {
                    data.Headers.Set(type, value);
                }
            }

            public void SetHeader(HeaderType type, string? value)
            {
                if (value is null || (data.Parent is { } parent && parent.TryGetTextHeader(type, out var current) && current == value))
                {
                    data.Headers.Remove(type);
                }
                else
                {
                    data.Headers.Set(type, value);
                }
            }

            public PlayerType Player { get => data.GetEnumHeader(HeaderType.Player, Constants.DefaultPlayer); set => data.SetHeader(HeaderType.Player, value); }
            public string? Genre { get => data.GetTextHeader(HeaderType.Genre); set => data.SetHeader(HeaderType.Genre, value); }
            public string? Title { get => data.GetTextHeader(HeaderType.Title); set => data.SetHeader(HeaderType.Title, value); }
            public string? SubTitle { get => data.GetTextHeader(HeaderType.SubTitle); set => data.SetHeader(HeaderType.SubTitle, value); }
            public string? Artist { get => data.GetTextHeader(HeaderType.Artist); set => data.SetHeader(HeaderType.Artist, value); }
            public string? SubArtist { get => data.GetTextHeader(HeaderType.SubArtist); set => data.SetHeader(HeaderType.SubArtist, value); }
            public double Bpm { get => data.GetDoubleHeader(HeaderType.Bpm, Constants.DefaultBpm); set => data.SetHeader(HeaderType.Bpm, value); }
            public string PlayLevel { get => data.GetTextHeader(HeaderType.PlayLevel, Constants.DefaultPlayLevel); set => data.SetHeader(HeaderType.PlayLevel, value); }
            public string Difficulty { get => data.GetTextHeader(HeaderType.Difficulty, Constants.DefaultDifficulty); set => data.SetHeader(HeaderType.Difficulty, value); }
            public Rank Rank { get => data.GetEnumHeader(HeaderType.Rank, Constants.DefaultRank); set => data.SetHeader(HeaderType.Rank, value); }
            public double Total { get => data.GetDoubleHeader(HeaderType.Total, 0); set => data.SetHeader(HeaderType.Total, value); }
            public string? StageFile { get => data.GetTextHeader(HeaderType.StageFile); set => data.SetHeader(HeaderType.StageFile, value); }
            public string? Banner { get => data.GetTextHeader(HeaderType.Banner); set => data.SetHeader(HeaderType.Banner, value); }
            public string? BackBmp { get => data.GetTextHeader(HeaderType.BackBmp); set => data.SetHeader(HeaderType.BackBmp, value); }
            public string? Preview { get => data.GetTextHeader(HeaderType.Preview); set => data.SetHeader(HeaderType.Preview, value); }
            public LongNoteMode LnMode { get => data.GetEnumHeader(HeaderType.LnMode, Constants.DefaultLnMode); set => data.SetHeader(HeaderType.LnMode, value); }
            public double ExRank { get => data.GetDoubleHeader(HeaderType.DefExRank, Constants.DefaultExRank); set => data.SetHeader(HeaderType.DefExRank, value); }
            public string? Comment { get => data.GetTextHeader(HeaderType.Comment); set => data.SetHeader(HeaderType.Comment, value); }
        }

    }
}
