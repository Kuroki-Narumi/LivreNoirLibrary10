using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track
    {
        public Rational GetFirstMetaPosition(MetaType type)
        {
            if (_timeline.Find((_, obj) => obj is IMetaObject m && m.Type == type, out var pos, out _))
            {
                return pos;
            }
            return new(-1);
        }

        public void SetMetaText(MetaType type, string? value)
        {
            bool Check(Rational _, IObject obj) => obj is MetaText m && m.Type == type;
            if (string.IsNullOrEmpty(value))
            {
                _timeline.RemoveIf(Check);
            }
            else
            {
                if (_timeline.Find(Check, out _, out var obj))
                {
                    (obj as MetaText)!.Text = value;
                }
                else
                {
                    _timeline.Add(Rational.Zero, new MetaText(type, value));
                }
            }
        }

        public void SetMetaText(Rational position, MetaType type, string? value)
        {
            bool Check(IObject obj) => obj is MetaText m && m.Type == type;
            if (string.IsNullOrEmpty(value))
            {
                _timeline.RemoveIf(position, Check);
            }
            else
            {
                if (_timeline.Find(position, Check, out var obj))
                {
                    (obj as MetaText)!.Text = value;
                }
                else
                {
                    _timeline.Add(position, new MetaText(type, value));
                }
            }
        }
    }
}
