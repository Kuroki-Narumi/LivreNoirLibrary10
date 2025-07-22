using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class DummyData : BaseData
    {
        private static readonly BmsData _root = new();

        public override BmsData Root => _root;
        public override int Base { get => _root.Base; set { } }
        public override int MaxDefIndex => _root.MaxDefIndex;
    }
}
