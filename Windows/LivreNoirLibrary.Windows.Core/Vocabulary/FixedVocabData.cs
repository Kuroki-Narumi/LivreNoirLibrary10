using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LivreNoirLibrary.Windows.Vocabulary
{
    public class FixedVocabData(string? value, string? keyTip = null) : IVocabData
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Value { get; } = value;
        public string? KeyTip { get; } = keyTip;

        public string WithLeader => $"{Value}{VocabData.Leader}";
    }
}
