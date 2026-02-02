using System;
using System.Collections.Generic;
using System.Threading;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class IndexesOptionsBase : ObservableObjectBase
    {
        public const string DefaultIndexText = "ex) 01-0Z 15 19-1C";

        protected readonly RangeSet<int> _indexes = [];

        public RangeSet<int> Indexes
        {
            get => _indexes;
            set
            {
                _indexes.OverwriteFrom(value);
                SendPropertyChanged();
            }
        }

        public string GetIndexText(int radix) => _indexes.GetListText(radix);

        public bool TrySetIndex(string? text, int radix)
        {
            using var o = ObjectPool.Rent<RangeSet<int>>();
            var cache = o.Value;
            if (BasedNumber.TryParseRangeSet(text, cache, radix))
            {
                Indexes = cache;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
