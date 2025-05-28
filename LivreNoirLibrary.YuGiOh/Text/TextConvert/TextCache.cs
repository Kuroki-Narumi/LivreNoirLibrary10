using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LivreNoirLibrary.YuGiOh.ViewModel;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class TextConvert
    {
        private static readonly Dictionary<Card, TextCache> _text_cache = [];
        private static readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

        static TextConvert()
        {
            CreateTextCache();
        }

        public static async void CreateTextCache()
        {
            await Task.Run(() =>
            {
                foreach (var card in CardPool.Instance.Cards)
                {
                    GetTextCache(card);
                }
            });
        }

        private static TextCache GetTextCache(Card card)
        {
            _lock.EnterReadLock();
            try
            {
                if (!_text_cache.TryGetValue(card, out var s))
                {
                    s = new(card);
                    _lock.EnterWriteLock();
                    try
                    {
                        _text_cache.Add(card, s);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
                return s;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public static void RemoveTextCache(Card card)
        {
            _lock.EnterWriteLock();
            try
            {
                _text_cache.Remove(card);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private class TextCache
        {
            public TextCacheItem Original { get; }
            public TextCacheItem Lower { get; }

            public TextCache(Card card)
            {
                var (name, cName) = StrConv(card.Name);
                var (ruby, cRuby) = StrConv(card.Ruby);
                var (enName, cEnName) = StrConv(card.EnName);
                var text = Wide2Half(card.Text);
                var pText = Wide2Half(card.PendulumText);

                Original = new(name, cName, ruby, cRuby, enName, cEnName, text, pText);
                Lower = new(name.ToLower(), cName.ToLower(), ruby.ToLower(), cRuby.ToLower(), enName.ToLower(), cEnName.ToLower(), text.ToLower(), pText.ToLower());
            }
        }

        private record TextCacheItem(string Name, string ConvertedName, string Ruby, string ConvertedRuby, string EnName, string ConvertedEnName, string Text, string PendulumText);
    }
}
