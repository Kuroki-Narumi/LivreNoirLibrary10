using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class Header(string type, string value) : HeaderBase(type, value), IDumpable<Header>
    {
        public void SetKey(string key)
        {
            Key = key;
        }

        public static Header Load(BinaryReader reader)
        {
            var (key, value) = LoadKV(reader);
            return new(key, value);
        }
    }
}
