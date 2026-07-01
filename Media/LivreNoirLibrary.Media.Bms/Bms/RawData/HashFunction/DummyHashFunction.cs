using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class DummyHashFunction : IHashFunction
    {
        public static DummyHashFunction Instance { get; } = new();

        public byte[] Hash => [];

        private DummyHashFunction() { }

        public void Initialize()
        {
        }

        public void Update(byte[] buffer)
        {
        }

        public void UpdateFinal(byte[] buffer)
        {
        }
    }
}
