using System;
using System.Collections.Generic;
using System.Threading;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class BulkEditOptions : ObservableObjectBase
    {
        private static readonly Lock _numbers_lock = new();
        private static readonly SortedSet<int> _numbers_cache = [];

        [ObservableProperty]
        private bool _selection;
        [ObservableProperty]
        private bool _target_Meta;
        [ObservableProperty]
        private bool _target_SysEx;
        [ObservableProperty]
        private bool _target_CC;
        [ObservableProperty]
        private bool _target_Note;

        [ObservableProperty]
        private Rational _rangeLeft = Rational.Zero;
        [ObservableProperty]
        private Rational _rangeRight = Rational.Zero;
        [ObservableProperty]
        private bool _rangeExclusive = false;

        private readonly SortedSet<int> _numbers = [];

        [ObservableProperty]
        private Rational _positionQuantize = Rational.Zero;
        [ObservableProperty]
        private ValueOperationMode _positionOperationMode;
        [ObservableProperty]
        private Rational _positionOperationValue;

        [ObservableProperty]
        private int _velQuantize = 0;
        [ObservableProperty]
        private ValueOperationMode _velOperationMode;
        [ObservableProperty]
        private Rational _velOperationValue;

        [ObservableProperty]
        private bool _legato;
        [ObservableProperty]
        private bool _lengthQuantize_Auto = true;
        [ObservableProperty]
        private Rational _lengthQuantize = Rational.Zero;
        [ObservableProperty]
        private ValueOperationMode _lengthOperationMode;
        [ObservableProperty]
        private Rational _lengthOperationValue;

        [ObservableProperty]
        private ValueOperationMode _numberOperationMode;
        [ObservableProperty]
        private Rational _numberOperationValue;

        [ObservableProperty]
        private bool _removeDuplicates;

        public SortedSet<int> Numbers
        {
            get => _numbers;
            set
            {
                _numbers.Clear();
                _numbers.UnionWith(value);
                SendPropertyChanged();
            }
        }

        public string GetNumbersText() => BasedIndex.GetIndexListText(_numbers, 10);

        public bool TrySetNumbers(string? text)
        {
            lock (_numbers_lock)
            {
                if (BasedIndex.TryParseIndexText(text, _numbers_cache, 10, 127))
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

        public void Load(BulkEditOptions other)
        {
            Selection = other._selection;
            RangeLeft = other._rangeLeft;
            RangeRight = other._rangeRight;
            RangeExclusive = other._rangeExclusive;

            Target_Meta = other._target_Meta;
            Target_SysEx = other._target_SysEx;
            Target_CC = other._target_CC;
            Target_Note = other._target_Note;
            Numbers = other._numbers;

            PositionQuantize = other._positionQuantize;
            PositionOperationMode = other._positionOperationMode;
            PositionOperationValue = other._positionOperationValue;
            LengthQuantize_Auto = other._lengthQuantize_Auto;
            LengthQuantize = other._lengthQuantize;
            LengthOperationMode = other._lengthOperationMode;
            LengthOperationValue = other._lengthOperationValue;
            VelQuantize = other._velQuantize;
            VelOperationMode = other._velOperationMode;
            VelOperationValue = other._velOperationValue;
            NumberOperationMode = other._numberOperationMode;
            NumberOperationValue = other._numberOperationValue;

            RemoveDuplicates = other._removeDuplicates;
        }
    }
}
