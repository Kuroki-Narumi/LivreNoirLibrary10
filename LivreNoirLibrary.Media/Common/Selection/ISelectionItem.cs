using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface ISelectionItem
    {
        public Rational ActualPosition { get; }
        public void ReplaceToClone();
    }
}
