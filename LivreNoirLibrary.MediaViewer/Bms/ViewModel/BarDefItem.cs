using System;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BarDefItem(int number) : ObservableObjectBase
    {
        private readonly int _number = number;
        [ObservableProperty(Related = [nameof(ValueText)])]
        private Rational _value;
        [ObservableProperty(Related = [nameof(DefaultValueText)])]
        private Rational _defaultValue = Constants.DefaultBarLength;

        public int Number => _number;
        public string NumberText => _number.GetBarText();
        public string ValueText => _value.IsPositiveThanZero() ? GetText(_value) : "";
        public string DefaultValueText => GetText(_defaultValue);

        public static string GetText(in Rational value) => value.ToString();

        public void Clear()
        {
            Value = 0;
            DefaultValue = Constants.DefaultBarLength;
        }
    }
}
