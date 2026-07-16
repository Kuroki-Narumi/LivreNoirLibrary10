using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardPackProvider
    {
        bool TryGet(string pid, [MaybeNullWhen(false)] out CardPack pack);
    }
}
