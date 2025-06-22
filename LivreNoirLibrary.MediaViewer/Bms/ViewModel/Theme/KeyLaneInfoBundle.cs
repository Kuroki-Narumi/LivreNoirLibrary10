using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Input;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using static LivreNoirLibrary.Media.Bms.KeyIndexes;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class KeyLaneInfoBundle : LaneInfoBundle
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter<ChartType>))]
        [ObservableProperty]
        private ChartType _type = ChartType.Unknown;
        [JsonPropertyName("scratch")]
        [ObservableProperty(Related = [nameof(HasScratchLane)])]
        private LaneInfo? _scratchLane;

        [JsonIgnore]
        public bool HasScratchLane
        {
            get => _scratchLane is not null;
            set
            {
                if (value)
                {
                    if (_scratchLane is null)
                    {
                        ScratchLane = LaneInfo.GetStatic("SCR", Beat_1P_Scratch, "Red", Key.A);
                    }
                }
                else if (_scratchLane is not null)
                {
                    ScratchLane = null;
                }
            }
        }

        public KeyLaneInfoBundle() { }

        public KeyLaneInfoBundle(string name, ChartType type, IEnumerable<LaneInfo> lanes) : base(name, lanes)
        {
            _type = type;
        }

        public KeyLaneInfoBundle(KeyLaneInfoBundle source) => Load(source);

        public override void Load(LaneInfoBundle source)
        {
            if (source is KeyLaneInfoBundle k)
            {
                Type = k._type;
                ScratchLane = k._scratchLane?.Clone();
            }
            base.Load(source);
        }

        public static KeyLaneInfoBundle Beat_5k { get; } = new("Beat-5k", ChartType.Beat,
        [
            LaneInfo.GetStatic("1", Beat_1P_1, "White", Key.Z),
            LaneInfo.GetStatic("2", Beat_1P_2, "Blue", Key.S),
            LaneInfo.GetStatic("3", Beat_1P_3, "White", Key.X),
            LaneInfo.GetStatic("4", Beat_1P_4, "Blue", Key.D),
            LaneInfo.GetStatic("5", Beat_1P_5, "White", Key.C),
            LaneInfo.GetStatic("SCR", Beat_1P_Scratch, "Red", Key.V),
        ]);

        public static KeyLaneInfoBundle Beat_10k { get; } = new("Beat-10k", ChartType.Beat,
        [
            LaneInfo.GetStatic("1-1", Beat_1P_1, "White", Key.Z),
            LaneInfo.GetStatic("1-2", Beat_1P_2, "Blue", Key.S),
            LaneInfo.GetStatic("1-3", Beat_1P_3, "White", Key.X),
            LaneInfo.GetStatic("1-4", Beat_1P_4, "Blue", Key.D),
            LaneInfo.GetStatic("1-5", Beat_1P_5, "White", Key.C),
            LaneInfo.GetStatic("1-SC", Beat_1P_Scratch, "Red", Key.V),
            LaneInfo.Separator,
            LaneInfo.GetStatic("2-1", Beat_2P_1, "White", Key.OemComma),
            LaneInfo.GetStatic("2-2", Beat_2P_2, "Blue", Key.L),
            LaneInfo.GetStatic("2-3", Beat_2P_3, "White", Key.OemPeriod),
            LaneInfo.GetStatic("2-4", Beat_2P_4, "Blue", Key.OemPlus),
            LaneInfo.GetStatic("2-5", Beat_2P_5, "White", Key.OemQuestion),
            LaneInfo.GetStatic("2-SC", Beat_2P_Scratch, "Red", Key.OemBackslash),
        ]);

        public static KeyLaneInfoBundle Beat_7k { get; } = new("Beat-7k", ChartType.Beat,
        [
            LaneInfo.GetStatic("1", Beat_1P_1, "White", Key.Z),
            LaneInfo.GetStatic("2", Beat_1P_2, "Blue", Key.S),
            LaneInfo.GetStatic("3", Beat_1P_3, "White", Key.X),
            LaneInfo.GetStatic("4", Beat_1P_4, "Blue", Key.D),
            LaneInfo.GetStatic("5", Beat_1P_5, "White", Key.C),
            LaneInfo.GetStatic("6", Beat_1P_6, "Blue", Key.F),
            LaneInfo.GetStatic("7", Beat_1P_7, "White", Key.V),
        ])
        {
            HasScratchLane = true,
        };

        public static KeyLaneInfoBundle Beat_14k { get; } = new("Beat-14k", ChartType.Beat,
        [
            LaneInfo.GetStatic("1-SC", Beat_1P_Scratch, "Red", Key.A),
            LaneInfo.GetStatic("1-1", Beat_1P_1, "White", Key.Z),
            LaneInfo.GetStatic("1-2", Beat_1P_2, "Blue", Key.S),
            LaneInfo.GetStatic("1-3", Beat_1P_3, "White", Key.X),
            LaneInfo.GetStatic("1-4", Beat_1P_4, "Blue", Key.D),
            LaneInfo.GetStatic("1-5", Beat_1P_5, "White", Key.C),
            LaneInfo.GetStatic("1-6", Beat_1P_6, "Blue", Key.F),
            LaneInfo.GetStatic("1-7", Beat_1P_7, "White", Key.V),
            LaneInfo.Separator,
            LaneInfo.GetStatic("2-1", Beat_2P_1, "White", Key.OemComma),
            LaneInfo.GetStatic("2-2", Beat_2P_2, "Blue", Key.L),
            LaneInfo.GetStatic("2-3", Beat_2P_3, "White", Key.OemPeriod),
            LaneInfo.GetStatic("2-4", Beat_2P_4, "Blue", Key.OemPlus),
            LaneInfo.GetStatic("2-5", Beat_2P_5, "White", Key.OemQuestion),
            LaneInfo.GetStatic("2-6", Beat_2P_6, "Blue", Key.OemSemicolon),
            LaneInfo.GetStatic("2-7", Beat_2P_7, "White", Key.OemBackslash),
            LaneInfo.GetStatic("2-SC", Beat_2P_Scratch, "Red", Key.OemCloseBrackets),
        ]);

        public static KeyLaneInfoBundle Pop_9k { get; } = new("Popn-9k", ChartType.Popn,
        [
            LaneInfo.GetStatic("LW", Pop_1, "White", Key.Z),
            LaneInfo.GetStatic("LY", Pop_2, "Yellow", Key.S),
            LaneInfo.GetStatic("LG", Pop_3, "Green", Key.X),
            LaneInfo.GetStatic("LB", Pop_4, "Blue", Key.D),
            LaneInfo.GetStatic("CR", Pop_5, "Red", Key.C),
            LaneInfo.GetStatic("RB", Pop_6, "Blue", Key.F),
            LaneInfo.GetStatic("RG", Pop_7, "Green", Key.V),
            LaneInfo.GetStatic("RY", Pop_8, "Yellow", Key.G),
            LaneInfo.GetStatic("RW", Pop_9, "White", Key.B),
        ]);

        public static KeyLaneInfoBundle Pop_18k { get; } = new("Popn-18k", ChartType.Popn,
        [
            LaneInfo.GetStatic("1-LW", Pop_1P_1, "White", Key.Z),
            LaneInfo.GetStatic("1-LY", Pop_1P_2, "Yellow", Key.S),
            LaneInfo.GetStatic("1-LG", Pop_1P_3, "Green", Key.X),
            LaneInfo.GetStatic("1-LB", Pop_1P_4, "Blue", Key.D),
            LaneInfo.GetStatic("1-CR", Pop_1P_5, "Red", Key.C),
            LaneInfo.GetStatic("1-RB", Pop_1P_6, "Blue", Key.F),
            LaneInfo.GetStatic("1-RG", Pop_1P_7, "Green", Key.V),
            LaneInfo.GetStatic("1-RY", Pop_1P_8, "Yellow", Key.G),
            LaneInfo.GetStatic("1-RW", Pop_1P_9, "White", Key.B),
            LaneInfo.Separator,
            LaneInfo.GetStatic("2-LW", Pop_2P_1, "White", Key.M),
            LaneInfo.GetStatic("2-LY", Pop_2P_2, "Yellow", Key.K),
            LaneInfo.GetStatic("2-LG", Pop_2P_3, "Green", Key.OemComma),
            LaneInfo.GetStatic("2-LB", Pop_2P_4, "Blue", Key.L),
            LaneInfo.GetStatic("2-CR", Pop_2P_5, "Red", Key.OemPeriod),
            LaneInfo.GetStatic("2-RB", Pop_2P_6, "Blue", Key.OemPlus),
            LaneInfo.GetStatic("2-RG", Pop_2P_7, "Green", Key.OemQuestion),
            LaneInfo.GetStatic("2-RY", Pop_2P_8, "Yellow", Key.OemSemicolon),
            LaneInfo.GetStatic("2-RW", Pop_2P_9, "White", Key.OemBackslash),
        ]);

        public static KeyLaneInfoBundle Generic_24k { get; } = CreateGeneric(2);
        public static KeyLaneInfoBundle Generic_48k { get; } = CreateGeneric(4);

        private static KeyLaneInfoBundle CreateGeneric(int setCount)
        {
            KeyLaneInfoBundle result = new($"Generic-{setCount * 12}k", ChartType.Generic, []);
            var list = result.Lanes;
            for (int i = 0; i < setCount; i++)
            {
                if (i is not 0)
                {
                    list.Add(LaneInfo.Separator);
                }
                var j = i * 12;
                list.Add(LaneInfo.GetStatic((j +  1).ToString(), j +  1, "White"));
                list.Add(LaneInfo.GetStatic((j +  2).ToString(), j +  2, "Blue"));
                list.Add(LaneInfo.GetStatic((j +  3).ToString(), j +  3, "White"));
                list.Add(LaneInfo.GetStatic((j +  4).ToString(), j +  4, "Blue"));
                list.Add(LaneInfo.GetStatic((j +  5).ToString(), j +  5, "White"));
                list.Add(LaneInfo.GetStatic((j +  6).ToString(), j +  6, "White"));
                list.Add(LaneInfo.GetStatic((j +  7).ToString(), j +  7, "Blue"));
                list.Add(LaneInfo.GetStatic((j +  8).ToString(), j +  8, "White"));
                list.Add(LaneInfo.GetStatic((j +  9).ToString(), j +  9, "Blue"));
                list.Add(LaneInfo.GetStatic((j + 10).ToString(), j + 10, "White"));
                list.Add(LaneInfo.GetStatic((j + 11).ToString(), j + 11, "Blue"));
                list.Add(LaneInfo.GetStatic((j + 12).ToString(), j + 12, "White"));
            }
            return result;
        }
    }
}
