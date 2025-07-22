using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public readonly struct KeySwitchOption(byte value) : IDumpable<KeySwitchOption>, IEquatable<KeySwitchOption>
    {
        public const byte IdFilter = 0x0F;
        public const byte ModeFilter = 0xF0;

        private readonly byte _value = value;

        public int GroupId => _value & IdFilter;
        public KeySwitchMode Mode => (KeySwitchMode)(_value & ModeFilter);

        public KeySwitchOption(KeySwitchMode mode, int group) : this((byte)((int)mode | group)) { }

        public void Dump(BinaryWriter stream) => stream.Write(_value);
        public static KeySwitchOption Load(BinaryReader stream) => new(stream.ReadByte());

        public bool Equals(KeySwitchOption other) => _value == other._value;
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is KeySwitchOption other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(KeySwitchOption left, KeySwitchOption right) => left.Equals(right);
        public static bool operator !=(KeySwitchOption left, KeySwitchOption right) => !left.Equals(right);

        public static implicit operator byte(KeySwitchOption value) => value._value;
        public static implicit operator KeySwitchOption(byte value) => new(value);

        public override string ToString() => $"KeySwitch{{Mode:{Mode} Group:{GroupId}}}";
    }
}
