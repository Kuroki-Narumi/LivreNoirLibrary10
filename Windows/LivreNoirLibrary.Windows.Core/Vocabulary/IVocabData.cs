using System;
using System.ComponentModel;

namespace LivreNoirLibrary.Windows
{
    public interface IVocabData : INotifyPropertyChanged
    {
        public string? Value { get; }
    }
}
