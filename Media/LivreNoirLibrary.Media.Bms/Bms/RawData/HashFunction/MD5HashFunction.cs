using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class MD5HashFunction : IHashFunction
    {
        private readonly MD5 _m = MD5.Create();

        public byte[] Hash => _m.Hash!;

        public void Initialize()
        {
            _m.Initialize();
        }

        public void Update(byte[] buffer)
        {
            _m.TransformBlock(buffer, 0, buffer.Length, null, 0);
        }

        public void UpdateFinal(byte[] buffer)
        {
            _m.TransformFinalBlock(buffer, 0, buffer.Length);
        }
    }
}
