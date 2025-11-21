using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Text;
using System.Windows.Input;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    [JsonConverter(typeof(LaneInfoJsonConverter))]
    public class LaneInfo : ObservableObjectBase, INamedObject, ICloneable<LaneInfo>, IEnumerable<LaneInfo>
    {
        public const int DefaultLaneWidth = 2;
        public const int DefaultMetaLaneWidth = 3;

        public static LaneInfo Separator { get; } = new() { Channel = 0 };

        public static LaneInfo Bgm_Default { get; } = new()
        {
            Name = "B",
            Width = DefaultLaneWidth,
            BackColor = Colors.Back_Bgm,
            NoteColor = Colors.Note_Bgm,
        };

        public string Name { get; set => SetValue(ref field, value.Shared()); } = "";
        public Channel Channel { get; set => SetValue(ref field, value, [nameof(Lane), nameof(IsSeparator)]); }
        public int Lane { get => Channel.TryGetLane(out var lane) ? lane : int.MaxValue; set => Channel = value.TryGetChannel(out var channel) ? channel : 0; }
        public bool IsSeparator => Channel is 0;
        public int Width { get; set => SetValue(ref field, value); }
        public Color BackColor { get; set => SetValue(ref field, value); }
        public Color NoteColor { get; set => SetValue(ref field, value); }
        public Color LongColor { get; set => SetValue(ref field, value); }
        public Key Key { get; set => SetValue(ref field, value); }

        public void CopyFrom(LaneInfo source)
        {
            Name = source.Name;
            Channel = source.Channel;
            Width = source.Width;
            BackColor = source.BackColor;
            NoteColor = source.NoteColor;
            LongColor = source.LongColor;
            Key = source.Key;
        }

        public LaneInfo Clone()
        {
            LaneInfo clone = new();
            clone.CopyFrom(this);
            return clone;
        }

        public static LaneInfo CreateLane(string name, Channel channel, string? colorName = null, int width = DefaultLaneWidth)
        {
            colorName ??= name;
            var t = typeof(Colors);
            var c1 = t.GetProperty($"Back_{colorName}")?.GetValue(null) is Color c ? c : default;
            var c2 = t.GetProperty($"Note_{colorName}")?.GetValue(null) is Color d ? d : default;
            return new()
            {
                Name = name,
                Channel = channel,
                Width = width,
                BackColor = c1,
                NoteColor = c2,
            };
        }

        public static LaneInfo CreateKeyLane(string name, Channel channel, string colorName, Key key, int width = DefaultLaneWidth)
        {
            var t = typeof(Colors);
            var c1 = t.GetProperty($"Back_{colorName}")?.GetValue(null) is Color c ? c : default;
            var c2 = t.GetProperty($"Note_{colorName}")?.GetValue(null) is Color d ? d : default;
            var c3 = t.GetProperty($"Long_{colorName}")?.GetValue(null) is Color e ? e : default;
            return new()
            {
                Name = name,
                Channel = channel,
                Width = width,
                BackColor = c1,
                NoteColor = c2,
                LongColor = c3,
                Key = key,
            };
        }

        public IEnumerator<LaneInfo> GetEnumerator()
        {
            yield return this;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
