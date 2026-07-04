using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows
{
    public interface IVocabulary<TSelf>
        where TSelf : IVocabulary<TSelf>
    {
        abstract static ICollection<LanguageData> Languages { get; }
        abstract static TSelf Current { get; }
        abstract static TSelf Default { get; }

        LanguageData? CurrentLanguage { get; set; }
    }
}
