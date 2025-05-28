using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class Token : Card
    {
        public string ConvertedName { get; internal set; }
        public bool IsInitialized { get; internal set; }
        public SortedCardList Generators { get; } = [];
        public SortedCardList Referers { get; } = [];

        public Token(int id, string name)
        {
            _id = id;
            var (_, cname) = TextConvert.StrConv(name);
            Name = name;
            ConvertedName = cname;
        }
    }

    public class TokenCollection : List<Token>
    {
        public SortedCardList Generic { get; } = [];
        public SortedCardList Except { get; } = [];

        private readonly Dictionary<string, int> _indexes = [];

        public Token GetByName(string name)
        {
            if (!_indexes.TryGetValue(name, out var index))
            {
                index = Count;
                Token token = new(index + 1, name);
                _indexes.Add(name, index);
                Add(token);
            }
            return this[index];
        }
    }
}
