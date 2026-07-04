using LivreNoirLibrary.Windows.YuGiOh.Vocabulary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public partial class Vocab : VocabBase, IVocabulary<Vocab>
    {
        public static ICollection<LanguageData> Languages { get; } = new ObservableCollection<LanguageData>();
        public static Vocab Current { get; } = new();
        public static Vocab Default { get; } = CreateDefault();

        static Vocab()
        {
            SetupInstance<Vocab>();
        }

        private static Vocab CreateDefault()
        {
            Vocab vocab = new();
            vocab.LoadDefault();
            return vocab;
        }

        public CardInfoVocab CInfo { get; } = new();
        public CardTypeVocab CType { get; } = new();
        public AttributeVocab Attr { get; } = new();
        public MonsterTypeVocab MType { get; } = new();
        public AbilityVocab Abi { get; } = new();

        public void LoadDefault()
        {
            CInfo.LoadDefault();
            CType.LoadDefault();
            Attr.LoadDefault();
            MType.LoadDefault();
            Abi.LoadDefault();
        }
    }
}
