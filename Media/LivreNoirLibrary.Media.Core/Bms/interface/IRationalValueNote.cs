using System;
using System.Collections.Generic;
using System.Text;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IRationalValueNote : INote
    {
        public Rational Value { get; set; }
    }
}
