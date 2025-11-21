using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ICount : IClear
    {
        int Count { get; }
    }
}
