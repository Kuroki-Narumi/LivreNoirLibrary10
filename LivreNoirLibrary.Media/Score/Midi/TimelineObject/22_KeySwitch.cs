using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class KeySwitch : IObject, ICloneable<KeySwitch>, IDumpable<KeySwitch>
    {
        public const byte Velocity = 100;

        private byte _gid;
        private byte _nn;
        private KeySwitchMode _mode;

        ObjectType IObject.ObjectType => ObjectType.KeySwitch;

        public string ObjectName => nameof(KeySwitch);
        public string ContentString => $"NN:{_nn} Mode:{_mode}";
        public int GroupId { get => _gid; set => _gid = (byte)value; }
        public int Number { get => _nn; set => _nn = (byte)Math.Clamp(value, 0, 127); }
        public KeySwitchMode Mode { get => _mode; set => _mode = value; }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_gid);
            writer.Write(_nn);
            writer.Write((byte)_mode);
        }

        public static KeySwitch Load(BinaryReader reader)
        {
            var gid = reader.ReadByte();
            var nn = reader.ReadByte();
            var mode = (KeySwitchMode)reader.ReadByte();
            return new() { _gid = gid, _nn = nn, _mode = mode };
        }

        public KeySwitch Clone() => new() { _gid = _gid, _nn = _nn, _mode = _mode };
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational position, long ticksPerWholeNote)
        {
            NoteOn on;
            NoteOff off;
            switch (_mode)
            {
                case KeySwitchMode.HoldOn:
                    on = new(channel, _nn, Velocity);
                    timeline.Add(tick - 1, on);
                    break;
                case KeySwitchMode.HoldOff:
                    off = new(channel, _nn);
                    timeline.Add(tick, off);
                    break;
                default:
                    on = new(channel, _nn, Velocity);
                    off = new(channel, _nn);
                    timeline.Add(tick - 1, on);
                    timeline.Add(tick + 1, off);
                    break;
            }
        }

        public int CompareTo(KeySwitch other)
        {
            var c = _gid - other._gid;
            if (c is not 0)
            {
                return c;
            }
            c = _nn - other._nn;
            if (c is not 0)
            {
                return c;
            }
            return (int)_mode - (int)other._mode;
        }

        public int CompareTo(IObject? other)
        {
            if (other is KeySwitch ks)
            {
                return CompareTo(ks);
            }
            return IObject.CompareBase(this, other);
        }
    }
}
