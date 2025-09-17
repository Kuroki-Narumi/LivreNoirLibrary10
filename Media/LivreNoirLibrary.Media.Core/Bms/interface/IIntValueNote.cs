using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IIntValueNote : INote
    {
        public int Value { get; set; }
    }
}
