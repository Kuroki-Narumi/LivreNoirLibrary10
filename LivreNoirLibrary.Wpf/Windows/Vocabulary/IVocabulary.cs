using System;

namespace LivreNoirLibrary.Windows
{
    public interface IVocabulary<TSelf>
        where TSelf : IVocabulary<TSelf>
    {
        public static abstract TSelf Current { get; set; }
        public static abstract TSelf Default { get; set; }
        public LanguageData? CurrentLanguage { get; set; }
    }
}
