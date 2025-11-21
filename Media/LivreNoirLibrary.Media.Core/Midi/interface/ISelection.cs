using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ISelection
    {
        int Count { get; }
        void Add(Rational position, IObject obj);
        bool Remove(Rational position, IObject obj);
        IEnumerable<(Rational, IObject)> EachItem();
        Rational GetFirstBeat();
    }
}
