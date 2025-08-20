using System;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowData(BmsData root) : BaseData
    {
        public override BmsData Root { get; } = root;
    }
}
