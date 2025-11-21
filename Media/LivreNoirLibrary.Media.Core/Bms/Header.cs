using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class Header(string key, string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;

        public void Deconstruct(out string key, out string value)
        {
            key = Key;
            value = Value;
        }
    }
}
