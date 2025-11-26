using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IFlowOptions
    {
        public bool IsFlowEnabled { get; }
        public RandomProvideMode RandomMode { get; }
        public int RandomSeed { get; }
    }
}
