using System;

namespace LivreNoirLibrary.Collections
{
    public interface IBacket<TIn, TSelf>
        where TIn : allows ref struct
        where TSelf : IBacket<TIn, TSelf>
    {
        abstract static TSelf Create(in TIn input);
        void SetData(in TIn input);
        void ClearData();
    }
}
