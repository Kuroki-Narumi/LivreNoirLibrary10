using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class SoundTimingList
    {
        private readonly SortedDictionary<int, List<TimingItem>> _list = [];
        private long _first = long.MaxValue;
        private long _last = 0;

        public int Count => _list.Count;
        public long FirstTick => _last is 0 ? 0 : _first;
        public long LastTick => _last;

        public void Clear()
        {
            _list.Clear();
            _first = long.MaxValue;
            _last = long.MaxValue;
        }

        public SortedDictionary<int, List<TimingItem>>.Enumerator GetEnumerator() => _list.GetEnumerator();

        public void Load(IBmsData data, TimeCounter counter, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            Clear();
            selector ??= n => n.IsPlayableSound(false, out _);
            foreach (var (pos, notes) in data.Timeline.EachList())
            {
                var tick = counter.Beat2Tick(data.GetAbsolutePosition(pos));
                if (length is not 0 && tick >= length)
                {
                    break;
                }
                foreach (var note in CollectionsMarshal.AsSpan(notes))
                {
                    if (note is ISoundNote s && selector(s))
                    {
                        Add(tick, s);
                    }
                }
            }
            SetEnd(length);
        }

        public void Load(Selection selection, TimeCounter counter, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            Clear();
            selector ??= n => n.IsPlayableSound(false, out _);
            foreach (var (_, beat, note) in selection)
            {
                var tick = counter.Beat2Tick(beat);
                if (length is not 0 && tick >= length)
                {
                    break;
                }
                if (note is ISoundNote s && selector(s))
                {
                    Add(tick, s);
                }
            }
            SetEnd(length);
        }

        public void Load(IBmsData data, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            TimeCounter counter = new(data);
            Load(data, counter, selector, length);
        }

        public void Load(IBmsData data, Selection selection, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            TimeCounter counter = new(data);
            Load(selection, counter, selector, length);
        }

        public static SoundTimingList Create(IBmsData data, TimeCounter counter, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            return CreateCore(list => list.Load(data, counter, selector, length));
        }

        public static SoundTimingList Create(Selection selection, TimeCounter counter, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            return CreateCore(list => list.Load(selection, counter, selector, length));
        }

        public static SoundTimingList Create(IBmsData data, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            return CreateCore(list => list.Load(data, selector, length));
        }

        public static SoundTimingList Create(IBmsData data, Selection selection, Predicate<ISoundNote>? selector = null, long length = 0)
        {
            return CreateCore(list => list.Load(data, selection, selector, length));
        }

        private static SoundTimingList CreateCore(Action<SoundTimingList> addProcess)
        {
            SoundTimingList list = new();
            addProcess(list);
            return list;
        }

        public void Add(long ticks, ISoundNote note)
        {
            var id = note.Value;
            if (id is > 0)
            {
                Add(id, ticks, note.Lane is > 0);
            }
        }

        public void Add(int id, long ticks, bool autoKey = false)
        {
            if (ticks < _first)
            {
                _first = ticks;
            }
            if (ticks > _last)
            {
                _last = ticks;
            }
            var list = _list.GetOrAdd(id);
            if (list.Count is > 0)
            {
                list[^1] = list[^1].SetLength(ticks, false);
            }
            list.Add(new(ticks, id, autoKey));
        }

        public void SetEnd(long ticks)
        {
            if (ticks > _last)
            {
                foreach (var (_, list) in _list)
                {
                    list[^1] = list[^1].SetLength(ticks, true);
                }
                _last = ticks;
            }
        }

        public readonly struct TimingItem : IComparable, IComparable<TimingItem>
        {
            public readonly long Position;
            public readonly long Length;
            public readonly int Id;
            public readonly bool AutoKey;
            public readonly bool IsLast;

            private TimingItem(long position, long length, int id, bool autoKey, bool isLast)
            {
                Position = position;
                Length = length;
                Id = id;
                AutoKey = autoKey;
                IsLast = isLast;
            }

            public TimingItem(long position, int id, bool autoKey) : this(position, -1, id, autoKey, false) { }
            public TimingItem SetLength(long endPosition, bool isLast) => new(Position, endPosition - Position, Id, AutoKey, isLast);

            public int CompareTo(TimingItem other) => Position.CompareTo(other.Position);
            public int CompareTo(object? obj) => obj is TimingItem other ? CompareTo(other) : 1;
        }
    }
}
