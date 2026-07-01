using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IHashFunction
    {
        byte[] Hash { get; }

        void Initialize();
        void Update(byte[] buffer);
        void UpdateFinal(byte[] buffer);
    }
}
