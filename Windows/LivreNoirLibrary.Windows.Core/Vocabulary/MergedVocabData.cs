using LivreNoirLibrary.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace LivreNoirLibrary.Windows
{
    public class MergedVocabData : ObservableObjectBase, IVocabData
    {
        private readonly IVocabData? _separator;
        private readonly IVocabData[] _sources;
        private string? _value;

        public string Value => _value ??= EnsureValue();
        public string WithLeader => $"{Value}{VocabData.Leader}";

        public MergedVocabData(IVocabData[] sources, IVocabData? separator = null)
        {
            _separator = separator;
            _sources = sources;
            _value = EnsureValue();
            separator?.PropertyChanged += OnPropertyChanged;
            foreach (var source in sources)
            {
                source.PropertyChanged += OnPropertyChanged;
            }
        }

        private string EnsureValue() => string.Join(_separator?.Value, _sources.Select(static v => v.Value));

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(VocabData.Value))
            {
                _value = null;
                this.NotifyPropertyChanged(nameof(Value));
                this.NotifyPropertyChanged(nameof(WithLeader));
            }
        }

        public override string ToString() => Value;

        public static implicit operator string(MergedVocabData value) => value.Value;
    }
}
