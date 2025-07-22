using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class SelectionItem(Rational position, IObject obj) : ISelectionItem
    {
        public Rational Position { get; } = position;
        Rational ISelectionItem.ActualPosition => Position;
        public IObject Object { get; private set; } = obj;

        public void ReplaceToClone()
        {
            if (Object is not NoteGroup)
            {
                Object = Object.Clone();
            }
        }

        public void Deconstruct(out Rational positoin, out IObject obj)
        {
            positoin = Position;
            obj = Object;
        }
    }
}
