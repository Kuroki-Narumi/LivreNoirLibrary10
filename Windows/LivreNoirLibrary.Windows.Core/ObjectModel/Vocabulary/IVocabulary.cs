using System;

namespace LivreNoirLibrary.Windows
{
    public interface IVocabulary<TSelf>
        where TSelf : IVocabulary<TSelf>
    {
        abstract static TSelf Current { get; set; }
        abstract static TSelf Default { get; set; }
        LanguageData? CurrentLanguage { get; set; }
    }
}
