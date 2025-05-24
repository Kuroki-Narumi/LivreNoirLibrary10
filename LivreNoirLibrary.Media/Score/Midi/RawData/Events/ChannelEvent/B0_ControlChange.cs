using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class ControlChange(int channel, CCType type, int value, int ext = ControlChange.ExtDisabled) : ChannelEvent(channel)
    {
        public const int ExtDisabled = 0xFFFF;

        public override StatusType Type => StatusType.ControlChange;

        private CCType _type = type;
        private byte _value = (byte)value;
        private int _ext = ext;

        public CCType Number { get => _type; set => _type = value; }
        public int Value { get => _value; set => _value = (byte)value; }
        public int Ext { get => _ext; set => _ext = value; }

        public static ControlChange Load(int channel, BinaryReader reader)
        {
            var type = (CCType)reader.ReadByte();
            var value = reader.ReadByte();
            var ext = NextSize is 3 ? reader.ReadByte() : ExtDisabled;
            NextSize = type is CCType.OmniModeOff ? 3 : 2;
            return new(channel, type, value, ext);
        }

        protected override void DumpContents(BinaryWriter s)
        {
            s.Write((byte)_type);
            s.Write(_value);
            if (_ext is not ExtDisabled)
            {
                s.Write((byte)_ext);
            }
        }

        private static int NextSize { get; set; } = 2;
        internal static void InitSize() => NextSize = 2;

        public static bool IsMSB(CCType type) => (int)type is < 32;
        public static bool IsLSB(CCType type) => (int)type is >= 32 and < 64;

        public static CCType GetLSB(CCType type) => type + 32;

        public static bool NeedsDataMSB(int rpn) => (RPN)rpn is RPN.ChannelFineTune or RPN.ModulationDepthRange;

        public override string ToString() => $"{nameof(ControlChange)}{{{nameof(Channel)}={_channel}, {nameof(Number)}={_type}, {nameof(Value)}={_value}, {nameof(Ext)}={_ext}}}";
    }
}
