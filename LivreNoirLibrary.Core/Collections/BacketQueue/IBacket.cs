using System;

namespace LivreNoirLibrary.Collections
{
    public interface IBacket<TIn, TSelf>
        where TIn : allows ref struct
        where TSelf : IBacket<TIn, TSelf>
    {
        public static abstract TSelf Create(TIn input);
        public void SetData(TIn input);
        public void ClearData();
    }
}
