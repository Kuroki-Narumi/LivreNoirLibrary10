using System;

namespace LivreNoirLibrary.Media
{
    public interface IOptions<T>
        where T : IOptions<T>
    {
        void Load(T source);
    }
}
