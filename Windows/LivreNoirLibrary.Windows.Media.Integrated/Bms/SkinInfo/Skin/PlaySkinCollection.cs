using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class PlaySkinCollection : IClear
    {
        private readonly Dictionary<int, ObservableList<PlaySkin>> _skins = [];
        public IEnumerable<PlaySkin>? this[int keyCount] => _skins.GetValueOrDefault(keyCount);

        public void Clear()
        {
            foreach (var (_, list) in _skins)
            {
                list.Clear();
            }
        }

        public void Add(PlaySkin skin)
        {
            _skins.GetOrAdd(0).Add(skin);
            foreach (var key in skin.KeyCount)
            {
                _skins.GetOrAdd(key).Add(skin);
            }
        }

        public IEnumerator<(int KeyCount, IEnumerable<PlaySkin> Skin)> GetEnumerator()
        {
            foreach (var (key, list) in _skins)
            {
                yield return (key, list);
            }
        }
    }
}
