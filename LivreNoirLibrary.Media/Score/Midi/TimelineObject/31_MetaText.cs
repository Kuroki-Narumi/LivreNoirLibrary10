using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class MetaText(MetaType type, string text) : IMetaObject, ICloneable<MetaText>, IDumpable<MetaText>
    {
        private MetaType _type = type;
        private string _text = text;

        ObjectType IObject.ObjectType => ObjectType.MetaText;
        public string ObjectName => _type.ToString();
        public string ContentString => _text;
        public MetaType Type { get => _type; set => _type = value; }
        public string Text { get => _text; set => _text = value; }

        public MetaText(RawData.MetaText source) : this(source.Type, source.Text) { }

        public static MetaText Load(BinaryReader reader)
        {
            var type = (MetaType)reader.ReadByte();
            var text = reader.ReadString();
            return new(type, text);
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((byte)_type);
            writer.Write(_text);
        }

        public bool Equals(IObject other) => other is MetaText m && m._type == _type && m._text == _text;

        public MetaText Clone() => new(_type, _text);
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawData.RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new RawData.MetaText(_type, _text));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
