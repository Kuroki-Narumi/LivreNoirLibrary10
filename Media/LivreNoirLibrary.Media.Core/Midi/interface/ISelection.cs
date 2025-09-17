using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ISelection
    {
        public int Count { get; }
        public void Add(Rational position, IObject obj);
        public bool Remove(Rational position, IObject obj);
        public IEnumerable<(Rational, IObject)> EachItem();
        public Rational GetFirstBeat();
    }
}
