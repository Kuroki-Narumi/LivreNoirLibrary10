using System;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowData(BmsData root) : BaseData
    {
        public override BmsData Root { get; } = root;

        public override int Base { get => Root.Base; set => Root.Base = value; }
        public override int MaxDefIndex => Root.MaxDefIndex;
    }
}
