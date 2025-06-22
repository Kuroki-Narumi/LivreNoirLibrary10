using System;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class DefListItem(DefType type, int index) : SelectableObject
    {
        [ObservableProperty(SetterScope = Scope.Private)]
        private DefType _type = type;
        private readonly int _index = index;
        [ObservableProperty(Related = [nameof(IndexText)])]
        private int _radix = Constants.Base_Default;
        [ObservableProperty]
        private string? _value;
        [ObservableProperty]
        private string? _defaultValue;

        public int Index => _index;
        public string IndexText => $"#{BmsUtils.ToBased(_index, _radix)}".Shared();

        public void Clear()
        {
            Value = null;
            DefaultValue = null;
        }

        public DefListItem Clone()
        {
            return new(Type, _index) { _radix = _radix, _value = _value, _defaultValue = _defaultValue };
        }

        public void Update(DefListItem source)
        {
            Radix = source._radix;
            Type = source._type;
            Value = source._value;
            DefaultValue = source._defaultValue;
        }
    }
}
