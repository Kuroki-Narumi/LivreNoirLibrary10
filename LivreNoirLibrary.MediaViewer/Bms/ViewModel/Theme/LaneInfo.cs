using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    [JsonConverter(typeof(LaneInfoJsonConverter))]
    public partial class LaneInfo : ObservableObjectBase, INamedObject, IEnumerable<LaneInfo>
    {
        public const int SeparatorLane = int.MaxValue;

        public const int DefaultLaneWidth = 2;
        public const int DefaultMetaLaneWidth = 3;
        public const int SeparatorWidth = 6;

        [ObservableProperty]
        private string _name;
        [ObservableProperty(Related = [nameof(Channel), nameof(IsSeparator)])]
        private int _lane;
        [ObservableProperty]
        private int _width;
        [ObservableProperty]
        private Color _backColor;
        [ObservableProperty]
        private Color _noteColor;
        [ObservableProperty]
        private Color _longColor;
        [ObservableProperty(Related = [nameof(KeyText)])]
        private Key _key;

        public Channel Channel
        {
            get => IsSeparator ? Channel.None : BmsUtils.GetMetaChannel(_lane);
            set => Lane = value.GetLane();
        }
        public bool IsSeparator => _lane is SeparatorLane;

        public string KeyText => KeyInput.GetKeyName(_key);

        private static string CoerceName(string value) => value.Shared();

        public LaneInfo()
        {
            _name = "";
            _width = DefaultLaneWidth;
        }

        public LaneInfo(string name, int lane, int width, Color back, Color note, Color @long, Key key)
        {
            _name = name.Shared();
            _lane = lane;
            _width = width;
            _backColor = back;
            _noteColor = note;
            _longColor = @long;
            _key = key;
        }

        public LaneInfo(LaneInfo source) : this(source._name, source._lane, source._width, source._backColor, source._noteColor, source._longColor, source._key) { }

        public LaneInfo Clone() => new(this);

        public void Load(LaneInfo? source)
        {
            if (source is not null)
            {
                Name = source._name;
                Lane = source._lane;
                Width = source._width;
                BackColor = source._backColor;
                NoteColor = source._noteColor;
                LongColor = source._longColor;
                Key = source._key;
            }
        }

        public static LaneInfo Separator { get; } = new("", SeparatorLane, SeparatorWidth, default, default, default, Key.None);
        public static LaneInfo Bgm { get; } = GetStatic("B", 0, "Bgm");

        public static LaneInfo GetStatic(string name, int lane, string colorName, int width = DefaultLaneWidth)
        {
            var t = typeof(Colors);
            var c1 = t.GetProperty($"Back_{colorName}")?.GetValue(null) is Color c ? c : default;
            var c2 = t.GetProperty($"Note_{colorName}")?.GetValue(null) is Color d ? d : default;
            var c3 = t.GetProperty($"Long_{colorName}")?.GetValue(null) is Color e ? e : default;
            return new(name, lane, width, c1, c2, c3, Key.None);
        }
        
        public static LaneInfo GetStatic(string name, int lane, string colorName, Key key, int width = DefaultLaneWidth)
        {
            var info = GetStatic(name, lane, colorName, width);
            info._key = key;
            return info;
        }

        public static LaneInfo GetStatic(string name, Channel channel, string colorName, int width = DefaultLaneWidth)
            => GetStatic(name, channel.GetLane(), colorName, width);

        public IEnumerator<LaneInfo> GetEnumerator()
        {
            yield return this;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
