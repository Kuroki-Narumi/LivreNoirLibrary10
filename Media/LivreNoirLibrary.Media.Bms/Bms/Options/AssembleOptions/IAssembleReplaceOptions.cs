using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IAssembleReplaceOptions
    {
        public AssembleReplaceMode ReplaceMode { get; }
        public double ReplaceMargin { get; }
    }
}
