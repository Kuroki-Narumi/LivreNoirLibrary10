using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class DummyData : BaseData
    {
        private static readonly BmsData _root = new();

        public override BmsData Root => _root;
    }
}
