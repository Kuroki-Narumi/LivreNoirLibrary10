using System;
using System.Collections.Generic;
using System.Text;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDecimalValueNote : INote
    {
        public decimal Value { get; set; }
    }
}
