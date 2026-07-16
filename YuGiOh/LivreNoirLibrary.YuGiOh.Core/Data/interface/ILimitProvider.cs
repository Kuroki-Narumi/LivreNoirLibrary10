using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ILimitProvider
    {
        bool TryGet(int id, out int count);
    }
}
