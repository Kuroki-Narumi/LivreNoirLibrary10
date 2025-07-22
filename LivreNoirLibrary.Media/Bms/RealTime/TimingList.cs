using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimingList : SortedDictionary<int, List<TimingListItem>>
    {
        private long _first = long.MaxValue;
        private long _last = 0;

        public long FirstTick => _last is 0 ? 0 : _first;
        public long LastTick => _last;

        public static TimingList Create(BaseData data, TimeCounter counter, Predicate<Note>? selector = null, long length = 0)
        {
            selector ??= n => n.IsPlayableSound();
            return CreateCore(length, list =>
            {
                foreach (var (pos, notes) in data.Timeline.EachList())
                {
                    var tick = counter.Beat2Ticks(data.GetBeat(pos));
                    if (length is not 0 && tick >= length)
                    {
                        break;
                    }
                    foreach (var note in CollectionsMarshal.AsSpan(notes))
                    {
                        if (selector(note))
                        {
                            list.Add(tick, note);
                        }
                    }
                }
            });
        }

        public static TimingList Create(Selection selection, TimeCounter counter, Predicate<Note>? selector = null, long length = 0)
        {
            selector ??= n => n.IsPlayableSound();
            return CreateCore(length, list =>
            {
                foreach (var (_, beat, note) in selection.EachItem())
                {
                    var tick = counter.Beat2Ticks(beat);
                    if (length is not 0 && tick >= length)
                    {
                        break;
                    }
                    if (selector(note))
                    {
                        list.Add(tick, note);
                    }
                }
            });
        }

        public static TimingList Create(BaseData data, Predicate<Note>? selector = null, long length = 0)
        {
            TimeCounter counter = new(data);
            return Create(data, counter, selector, length);
        }

        public static TimingList Create(BaseData data, Selection selection, Predicate<Note>? selector = null, long length = 0)
        {
            TimeCounter counter = new(data);
            return Create(selection, counter, selector, length);
        }

        private static TimingList CreateCore(long length, Action<TimingList> addProcess)
        {
            TimingList list = [];
            addProcess(list);
            list.SetEnd(length);
            return list;
        }

        public new void Clear()
        {
            base.Clear();
            _first = long.MaxValue;
            _last = long.MaxValue;
        }

        public void Add<T>(long ticks, T note) where T : INote
        {
            var id = note.Id;
            if (id is > 0)
            {
                AddCore(id, ticks, new(ticks, id, note.Lane is > 0));
            }
        }

        public void Add(int id, long ticks, bool autoKey = false) => AddCore(id, ticks, new(ticks, id, autoKey));

        private void AddCore(int id, long ticks, in TimingListItem item)
        {
            if (ticks < _first)
            {
                _first = ticks;
            }
            if (ticks > _last)
            {
                _last = ticks;
            }
            var list = this.GetOrAdd(id);
            if (list.Count is > 0)
            {
                list[^1] = list[^1].SetLength(ticks, false);
            }
            list.Add(item);
        }

        public void SetEnd(long ticks)
        {
            if (ticks > _last)
            {
                foreach (var (_, list) in this)
                {
                    list[^1] = list[^1].SetLength(ticks, true);
                }
                _last = ticks;
            }
        }
    }

    public readonly struct TimingListItem : IComparable, IComparable<TimingListItem>
    {
        public readonly long Position;
        public readonly long Length;
        public readonly int Id;
        public readonly bool AutoKey;
        public readonly bool IsLast;

        private TimingListItem(long position, long length, int id, bool autoKey, bool isLast)
        {
            Position = position; 
            Length = length; 
            Id = id;
            AutoKey = autoKey;
            IsLast = isLast;
        }

        public TimingListItem(long position, int id, bool autoKey) : this(position, -1, id, autoKey, false) { }
        public TimingListItem SetLength(long endPosition, bool isLast) => new(Position, endPosition - Position, Id, AutoKey, isLast);

        public int CompareTo(TimingListItem other) => Position.CompareTo(other.Position);
        public int CompareTo(object? obj) => obj is TimingListItem other ? CompareTo(other) : 1;
    }
}
