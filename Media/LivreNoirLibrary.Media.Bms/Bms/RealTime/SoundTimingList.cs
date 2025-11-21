using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class SoundTimingList : IClear
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

        public void Load(IBmsViewModel source, Predicate<Note>? selector = null, long length = 0)
        {
            Clear();
            var counter = source.TimeCounter;
            selector ??= BmsExtensions.IsPlayableSound;
            foreach (var (pos, notes) in source.CurrentData.Timeline.EnumerateList())
            {
                var tick = counter.Beat2Tick(source.GetAbsolutePosition(pos));
                if (length is not 0 && tick >= length)
                {
                    break;
                }
                foreach (var note in notes.AsSpan())
                {
                    if (selector(note))
                    {
                        Add(tick, note);
                    }
                }
            }
            SetEnd(length);
        }

        public void Load(IBmsViewModel source, Selection selection, Predicate<Note>? selector = null, long length = 0)
        {
            Clear();
            selector ??= BmsExtensions.IsPlayableSound;
            var counter = source.TimeCounter;
            foreach (var (pos, note) in selection)
            {
                var tick = counter.Beat2Tick(pos);
                if (length is not 0 && tick >= length)
                {
                    break;
                }
                if (selector(note))
                {
                    Add(tick, note);
                }
            }
            SetEnd(length);
        }

        public static SoundTimingList Create(IBmsViewModel source, Predicate<Note>? selector = null, long length = 0)
        {
            SoundTimingList result = new();
            result.Load(source, selector, length);
            return result;
        }

        public static SoundTimingList Create(IBmsViewModel source, Selection selection, Predicate<Note>? selector = null, long length = 0)
        {
            SoundTimingList result = new();
            result.Load(source, selection, selector, length);
            return result;
        }

        public void Add(long ticks, Note note)
        {
            var id = note.Value;
            if (id is > 0)
            {
                Add((int)id, ticks, note.IsKey());
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
