using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class BulkEditOptions : ObservableObjectBase
    {
        public bool Selection { get; set => SetValue(ref field, value); }

        public Rational RangeLeft { get; set => SetValue(ref field, value); }
        public Rational RangeRight { get; set => SetValue(ref field, value); }
        public bool RangeExclusive { get; set => SetValue(ref field, value); }

        public bool Target_Meta { get; set => SetValue(ref field, value); }
        public bool Target_SysEx { get; set => SetValue(ref field, value); }
        public bool Target_CC { get; set => SetValue(ref field, value); }
        public bool Target_Note { get; set => SetValue(ref field, value); }

        private static readonly List<int> _numbers_cache = [];
        internal readonly SortedSet<int> _numbers = [];
        public IEnumerable<int> Numbers
        {
            get => _numbers;
            set
            {
                _numbers.Clear();
                _numbers.UnionWith(value);
                SendPropertyChanged();
            }
        }

        public Rational PositionQuantize { get; set => SetValue(ref field, value); }
        public ValueOperationMode PositionOperationMode { get; set => SetValue(ref field, value); }
        public Rational PositionOperationValue { get; set => SetValue(ref field, value); }

        public bool Legato { get; set => SetValue(ref field, value); }
        public bool LengthQuantize_Auto { get; set => SetValue(ref field, value); } = true;
        public Rational LengthQuantize { get; set => SetValue(ref field, value); }
        public ValueOperationMode LengthOperationMode { get; set => SetValue(ref field, value); }
        public Rational LengthOperationValue { get; set => SetValue(ref field, value); }

        public int VelQuantize { get; set => SetValue(ref field, value); }
        public ValueOperationMode VelOperationMode { get; set => SetValue(ref field, value); }
        public Rational VelOperationValue { get; set => SetValue(ref field, value); }

        public ValueOperationMode NumberOperationMode { get; set => SetValue(ref field, value); }
        public Rational NumberOperationValue { get; set => SetValue(ref field, value); }

        public bool RemoveDuplicates { get; set => SetValue(ref field, value); }

        public string GetNumbersText() => BasedNumber.GetListText(Numbers, 10);
        public bool TrySetNumbers(string? text)
        {
            if (BasedNumber.TryParseListText(text, _numbers_cache, 10, 127))
            {
                Numbers = _numbers_cache;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
