using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Wave;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BgmTimeline : IAudioTimeline<string>
    {
        public const int Tag_KeySound = 0;
        public const int Tag_BgmSound = 1;

        private readonly HashSet<string> _keys = [];
        private readonly Dictionary<string, Item> _items = [];

        public int KeyCount => _keys.Count;

        public void Clear()
        {
            foreach (var (_, list) in _items)
            {
                list.Clear();
            }
            _keys.Clear();
        }

        public void Add(string key, SoundInfo info)
        {
            _keys.Add(key);
            _items.GetOrAdd(key, k => new(k)).Add(info);
        }

        public IEnumerator<IAudioList<string>> GetEnumerator()
        {
            foreach (var (_, list) in _items)
            {
                if (list.IsValid)
                {
                    yield return list;
                }
            }
        }

        private class Item(string key) : IAudioList<string>
        {
            private readonly List<SoundInfo> _list = [];
            private int _index;

            public string Key { get; } = key;
            public bool IsValid => _list.Count is > 0;

            public TimelineItem FirstItem => ToTI(_list[0]);
            public TimelineItem LastItem => ToTI(_list[^1]);

            public void Clear()
            {
                _list.Clear();
                Rewind();
            }

            public void Add(SoundInfo info)
            {
                _list.Add(info);
            }

            public void Rewind()
            {
                _index = 0;
            }

            public bool MoveNext(double untilExclusive, out TimelineItem current)
            {
                var list = _list;
                var index = _index;
                SoundInfo info;
                if (index < list.Count && (info = list[index]).Time < untilExclusive)
                {
                    current = ToTI(info);
                    _index = index + 1;
                    return true;
                }
                current = default;
                return false;
            }

            private static TimelineItem ToTI(SoundInfo info) => new(info.Time, info.Length, info.IsKey ? Tag_KeySound : Tag_BgmSound);
        }
    }
}
