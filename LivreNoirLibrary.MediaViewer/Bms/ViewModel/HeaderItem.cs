using System;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class HeaderItem(Header source) : ObservableObjectBase
    {
        private readonly Header _source = source;

        public string Key
        {
            get => _source.Key;
            set
            {
                if (value != _source.Key)
                {
                    _source.SetKey(value);
                    SendPropertyChanged();
                    SendPropertyChanged(nameof(KeyValue));
                }
            }
        }

        public string Value
        {
            get => _source.Value;
            set
            {
                if (value != _source.Value)
                {
                    _source.Value = value;
                    SendPropertyChanged();
                    SendPropertyChanged(nameof(KeyValue));
                }
            }
        }

        public string KeyValue => $"#{Key} {Value}";

        public void SetValues(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
