using System;

namespace LivreNoirLibrary.Media
{
    public interface IOptions<T>
        where T : IOptions<T>
    {
        public void Load(T source);
    }
}
