using System;
using System.Text.Json.Serialization;
using System.Windows.Input;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public sealed class KeyLaneInfoBundle : LaneInfoBundle
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter<ChartType>))]
        public ChartType Type { get; set => SetValue(ref field, value); }
        [JsonPropertyName("player")]
        public PlayerCount Player { get; set => SetValue(ref field, value); }
        [JsonPropertyName("mode_hint")]
        public string? ModeHint { get; set => SetValue(ref field, value); }
        [JsonPropertyName("scratch")]
        public LaneInfo? ScratchLane { get; set => SetValue(ref field, value, [nameof(HasScratchLane)]); }
        [JsonIgnore]
        public bool HasScratchLane
        {
            get => ScratchLane is not null;
            set
            {
                if (value)
                {
                    ScratchLane ??= LaneInfo.CreateKeyLane("SCR", Channel.Beat_1P_Scratch, "Red", Key.A);
                }
            }
        }

        public KeyLaneInfoBundle() { }
        public KeyLaneInfoBundle(string name, ChartType type, PlayerCount player, string modeHint, ReadOnlySpan<LaneInfo> lanes, LaneInfo? scratch = null) : base(name, lanes)
        {
            Type = type;
            Player = player;
            ModeHint = modeHint;
            ScratchLane = scratch;
        }

        public void CopyFrom(KeyLaneInfoBundle source)
        {
            Type = source.Type;
            Player = source.Player;
            ModeHint = source.ModeHint;
            ScratchLane = source.ScratchLane?.Clone();
            base.CopyFrom(source);
        }

        public static KeyLaneInfoBundle Beat_5k { get; } = new("Beat-5k", ChartType.Beat, PlayerCount.Single, "beat-5k",
        [
            LaneInfo.CreateKeyLane("1", Channel.Beat_1P_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("2", Channel.Beat_1P_2, "Blue", Key.S),
            LaneInfo.CreateKeyLane("3", Channel.Beat_1P_3, "White", Key.X),
            LaneInfo.CreateKeyLane("4", Channel.Beat_1P_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("5", Channel.Beat_1P_5, "White", Key.C),
            LaneInfo.CreateKeyLane("SCR", Channel.Beat_1P_Scratch, "Red", Key.V),
        ]);

        public static KeyLaneInfoBundle Beat_10k { get; } = new("Beat-10k", ChartType.Beat, PlayerCount.Double, "beat-10k",
        [
            LaneInfo.CreateKeyLane("1-1", Channel.Beat_1P_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("1-2", Channel.Beat_1P_2, "Blue", Key.S),
            LaneInfo.CreateKeyLane("1-3", Channel.Beat_1P_3, "White", Key.X),
            LaneInfo.CreateKeyLane("1-4", Channel.Beat_1P_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("1-5", Channel.Beat_1P_5, "White", Key.C),
            LaneInfo.CreateKeyLane("1-SC", Channel.Beat_1P_Scratch, "Red", Key.V),
            LaneInfo.Separator,
            LaneInfo.CreateKeyLane("2-1", Channel.Beat_2P_1, "White", Key.OemComma),
            LaneInfo.CreateKeyLane("2-2", Channel.Beat_2P_2, "Blue", Key.L),
            LaneInfo.CreateKeyLane("2-3", Channel.Beat_2P_3, "White", Key.OemPeriod),
            LaneInfo.CreateKeyLane("2-4", Channel.Beat_2P_4, "Blue", Key.OemPlus),
            LaneInfo.CreateKeyLane("2-5", Channel.Beat_2P_5, "White", Key.OemQuestion),
            LaneInfo.CreateKeyLane("2-SC", Channel.Beat_2P_Scratch, "Red", Key.OemBackslash),
        ]);

        public static KeyLaneInfoBundle Beat_7k { get; } = new("Beat-7k", ChartType.Beat, PlayerCount.Single, "beat_7k",
        [
            LaneInfo.CreateKeyLane("1", Channel.Beat_1P_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("2", Channel.Beat_1P_2, "Blue", Key.S),
            LaneInfo.CreateKeyLane("3", Channel.Beat_1P_3, "White", Key.X),
            LaneInfo.CreateKeyLane("4", Channel.Beat_1P_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("5", Channel.Beat_1P_5, "White", Key.C),
            LaneInfo.CreateKeyLane("6", Channel.Beat_1P_6, "Blue", Key.F),
            LaneInfo.CreateKeyLane("7", Channel.Beat_1P_7, "White", Key.V),
        ], LaneInfo.CreateKeyLane("SCR", Channel.Beat_1P_Scratch, "Red", Key.A));

        public static KeyLaneInfoBundle Beat_14k { get; } = new("Beat-14k", ChartType.Beat, PlayerCount.Double, "beat-14k",
        [
            LaneInfo.CreateKeyLane("1-SC", Channel.Beat_1P_Scratch, "Red", Key.A),
            LaneInfo.CreateKeyLane("1-1", Channel.Beat_1P_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("1-2", Channel.Beat_1P_2, "Blue", Key.S),
            LaneInfo.CreateKeyLane("1-3", Channel.Beat_1P_3, "White", Key.X),
            LaneInfo.CreateKeyLane("1-4", Channel.Beat_1P_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("1-5", Channel.Beat_1P_5, "White", Key.C),
            LaneInfo.CreateKeyLane("1-6", Channel.Beat_1P_6, "Blue", Key.F),
            LaneInfo.CreateKeyLane("1-7", Channel.Beat_1P_7, "White", Key.V),
            LaneInfo.Separator,
            LaneInfo.CreateKeyLane("2-1", Channel.Beat_2P_1, "White", Key.OemComma),
            LaneInfo.CreateKeyLane("2-2", Channel.Beat_2P_2, "Blue", Key.L),
            LaneInfo.CreateKeyLane("2-3", Channel.Beat_2P_3, "White", Key.OemPeriod),
            LaneInfo.CreateKeyLane("2-4", Channel.Beat_2P_4, "Blue", Key.OemPlus),
            LaneInfo.CreateKeyLane("2-5", Channel.Beat_2P_5, "White", Key.OemQuestion),
            LaneInfo.CreateKeyLane("2-6", Channel.Beat_2P_6, "Blue", Key.OemSemicolon),
            LaneInfo.CreateKeyLane("2-7", Channel.Beat_2P_7, "White", Key.OemBackslash),
            LaneInfo.CreateKeyLane("2-SC", Channel.Beat_2P_Scratch, "Red", Key.OemCloseBrackets),
        ]);

        public static KeyLaneInfoBundle Popn_9k { get; } = new("Popn-9k", ChartType.Popn, PlayerCount.Single, "popn-9k",
        [
            LaneInfo.CreateKeyLane("LW", Channel.Popn_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("LY", Channel.Popn_2, "Yellow", Key.S),
            LaneInfo.CreateKeyLane("LG", Channel.Popn_3, "Green", Key.X),
            LaneInfo.CreateKeyLane("LB", Channel.Popn_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("CR", Channel.Popn_5, "Red", Key.C),
            LaneInfo.CreateKeyLane("RB", Channel.Popn_6, "Blue", Key.F),
            LaneInfo.CreateKeyLane("RG", Channel.Popn_7, "Green", Key.V),
            LaneInfo.CreateKeyLane("RY", Channel.Popn_8, "Yellow", Key.G),
            LaneInfo.CreateKeyLane("RW", Channel.Popn_9, "White", Key.B),
        ]);

        public static KeyLaneInfoBundle Popn_18k { get; } = new("Popn-18k", ChartType.Popn, PlayerCount.Double, "popn-18k",
        [
            LaneInfo.CreateKeyLane("1-LW", Channel.Popn_1P_1, "White", Key.Z),
            LaneInfo.CreateKeyLane("1-LY", Channel.Popn_1P_2, "Yellow", Key.S),
            LaneInfo.CreateKeyLane("1-LG", Channel.Popn_1P_3, "Green", Key.X),
            LaneInfo.CreateKeyLane("1-LB", Channel.Popn_1P_4, "Blue", Key.D),
            LaneInfo.CreateKeyLane("1-CR", Channel.Popn_1P_5, "Red", Key.C),
            LaneInfo.CreateKeyLane("1-RB", Channel.Popn_1P_6, "Blue", Key.F),
            LaneInfo.CreateKeyLane("1-RG", Channel.Popn_1P_7, "Green", Key.V),
            LaneInfo.CreateKeyLane("1-RY", Channel.Popn_1P_8, "Yellow", Key.G),
            LaneInfo.CreateKeyLane("1-RW", Channel.Popn_1P_9, "White", Key.B),
            LaneInfo.Separator,
            LaneInfo.CreateKeyLane("2-LW", Channel.Popn_2P_1, "White", Key.M),
            LaneInfo.CreateKeyLane("2-LY", Channel.Popn_2P_2, "Yellow", Key.K),
            LaneInfo.CreateKeyLane("2-LG", Channel.Popn_2P_3, "Green", Key.OemComma),
            LaneInfo.CreateKeyLane("2-LB", Channel.Popn_2P_4, "Blue", Key.L),
            LaneInfo.CreateKeyLane("2-CR", Channel.Popn_2P_5, "Red", Key.OemPeriod),
            LaneInfo.CreateKeyLane("2-RB", Channel.Popn_2P_6, "Blue", Key.OemPlus),
            LaneInfo.CreateKeyLane("2-RG", Channel.Popn_2P_7, "Green", Key.OemQuestion),
            LaneInfo.CreateKeyLane("2-RY", Channel.Popn_2P_8, "Yellow", Key.OemSemicolon),
            LaneInfo.CreateKeyLane("2-RW", Channel.Popn_2P_9, "White", Key.OemBackslash),
        ]);

        public static KeyLaneInfoBundle Generic_24k { get; } = CreateGeneric(2, PlayerCount.Single, "keyboard-24k");
        public static KeyLaneInfoBundle Generic_48k { get; } = CreateGeneric(4, PlayerCount.Double, "keyboard-24k-double");

        private static KeyLaneInfoBundle CreateGeneric(int setCount, PlayerCount player, string modeHint)
        {
            KeyLaneInfoBundle result = new($"Keyboard-{setCount * 12}k", ChartType.Keyboard, player, modeHint, []);
            var list = result.Lanes;

            ReadOnlySpan<bool> isWhite = [true, false, true, false, true, true, false, true, false, true, false, true];
            for (var i = 0; i < setCount; i++)
            {
                if (i is not 0)
                {
                    list.Add(LaneInfo.Separator);
                }
                for (var j = 0; j < isWhite.Length; j++)
                {
                    var lane = i * 12 + j;
                    list.Add(LaneInfo.CreateKeyLane(lane.ToString(), (short)lane + Channel.Visible_Start, isWhite[j] ? "White" : "Blue", 0));
                }
            }

            ReadOnlySpan<Key> keys = [Key.Z, Key.S, Key.X, Key.D, Key.C, Key.V, Key.G, Key.B, Key.H, Key.N, Key.J, Key.M];
            for (var i = 0; i < keys.Length; i++)
            {
                list[i].Key = keys[i];
            }
            return result;
        }
    }
}
