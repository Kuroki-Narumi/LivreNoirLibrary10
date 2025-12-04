using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IBmsViewModel vm)
        {
            public bool IsHeaderDefined(HeaderType type, bool containsCurrent = true)
                => !string.IsNullOrEmpty(vm.GetInheritedValue((data, out value) => data.MainHeaders.TryGetValue(type, out value!), "", containsCurrent));

            public double GetDoubleHeader(HeaderType type, double ifNone, bool containsCurrent = true)
                => vm.GetInheritedValue((data, out value) => data.MainHeaders.TryGetDouble(type, out value), ifNone, containsCurrent);

            public int GetIntHeader(HeaderType type, int ifNone, bool containsCurrent = true)
                => (int)GetDoubleHeader(vm, type, ifNone, containsCurrent);

            public T GetEnumHeader<T>(HeaderType type, T ifNone, bool containsCurrent = true)
                where T : struct, Enum
                => vm.GetInheritedValue((data, out value) => data.MainHeaders.TryGetEnum(type, out value), ifNone, containsCurrent);

            [return: NotNullIfNotNull(nameof(ifNone))]
            public string? GetTextHeader(HeaderType type, string? ifNone = null, bool containsCurrent = true)
                => vm.GetInheritedValue((data, out value) => data.MainHeaders.TryGetValue(type, out value!), ifNone, containsCurrent);

            public void RemoveHeader(HeaderType type)
            {
                if (vm.CurrentData.MainHeaders.Remove(type))
                {
                    vm.OnModified();
                }
            }

            public void SetHeader(HeaderType type, double value)
            {
                var headers = vm.CurrentData.MainHeaders;
                if (!headers.TryGetDouble(type, out var current) || current != value)
                {
                    headers.Set(type, value);
                    vm.OnModified();
                }
            }

            public void SetHeader<T>(HeaderType type, T value)
                where T : struct, Enum => SetHeader(vm, type, (value as IConvertible).ToDouble(null));

            public void SetHeader(HeaderType type, string? value)
            {
                if (value is null)
                {
                    RemoveHeader(vm, type);
                    return;
                }
                var headers = vm.CurrentData.MainHeaders;
                if (!headers.TryGetValue(type, out var current) || current != value)
                {
                    headers.Set(type, value);
                    vm.OnModified();
                }
            }

            public PlayerType Player { get => vm.GetEnumHeader(HeaderType.Player, BmsConstants.DefaultPlayer); set => vm.SetHeader(HeaderType.Player, value); }
            public string? Genre { get => vm.GetTextHeader(HeaderType.Genre); set => vm.SetHeader(HeaderType.Genre, value); }
            public string? Title { get => vm.GetTextHeader(HeaderType.Title); set => vm.SetHeader(HeaderType.Title, value); }
            public string? SubTitle { get => vm.GetTextHeader(HeaderType.SubTitle); set => vm.SetHeader(HeaderType.SubTitle, value); }
            public string? Artist { get => vm.GetTextHeader(HeaderType.Artist); set => vm.SetHeader(HeaderType.Artist, value); }
            public string? SubArtist { get => vm.GetTextHeader(HeaderType.SubArtist); set => vm.SetHeader(HeaderType.SubArtist, value); }
            public double Bpm { get => vm.GetDoubleHeader(HeaderType.Bpm, BmsConstants.DefaultBpm); set => vm.SetHeader(HeaderType.Bpm, value); }
            public int PlayLevel { get => vm.GetIntHeader(HeaderType.PlayLevel, BmsConstants.DefaultPlayLevel); set => vm.SetHeader(HeaderType.PlayLevel, value); }
            public int Difficulty { get => vm.GetIntHeader(HeaderType.Difficulty, BmsConstants.DefaultDifficulty); set => vm.SetHeader(HeaderType.Difficulty, value); }
            public Rank Rank { get => vm.GetEnumHeader(HeaderType.Rank, BmsConstants.DefaultRank); set => vm.SetHeader(HeaderType.Rank, value); }
            public double Total { get => vm.GetDoubleHeader(HeaderType.Total, 0); set => vm.SetHeader(HeaderType.Total, value); }
            public string? StageFile { get => vm.GetTextHeader(HeaderType.StageFile); set => vm.SetHeader(HeaderType.StageFile, value); }
            public string? Banner { get => vm.GetTextHeader(HeaderType.Banner); set => vm.SetHeader(HeaderType.Banner, value); }
            public string? BackBmp { get => vm.GetTextHeader(HeaderType.BackBmp); set => vm.SetHeader(HeaderType.BackBmp, value); }
            public string? Preview { get => vm.GetTextHeader(HeaderType.Preview); set => vm.SetHeader(HeaderType.Preview, value); }
            public LongNoteMode LnMode { get => vm.GetEnumHeader(HeaderType.LnMode, BmsConstants.DefaultLnMode); set => vm.SetHeader(HeaderType.LnMode, value); }
            public double ExRank { get => vm.GetDoubleHeader(HeaderType.DefExRank, BmsConstants.DefaultExRank); set => vm.SetHeader(HeaderType.DefExRank, value); }
            public string? Comment { get => vm.GetTextHeader(HeaderType.Comment); set => vm.SetHeader(HeaderType.Comment, value); }

            public int LnObj
            {
                get => vm.Root.LnObj;
                set
                {
                    if (vm.Root.LnObj != value)
                    {
                        vm.Root.LnObj = value;
                        vm.OnModified();
                    }
                }
            }
        }
    }
}
