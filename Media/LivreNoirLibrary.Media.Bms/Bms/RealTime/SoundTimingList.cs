using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class SoundTimingList : IClear, ISoundList
    {
        private readonly SortedDictionary<int, List<SoundTimingInfo>> _list = [];
        private double _first = long.MaxValue;
        private double _last = 0;

        public int Count => _list.Count;
        public double FirstTime => _last is 0 ? 0 : _first;
        public double LastTime => _last;

        public void Clear()
        {
            _list.Clear();
            _first = double.MaxValue;
            _last = 0;
        }

        public SortedDictionary<int, List<SoundTimingInfo>>.Enumerator GetEnumerator() => _list.GetEnumerator();

        IEnumerable<(int WavId, List<SoundTimingInfo>)> ISoundList.EnumerateSoundList()
        {
            foreach (var (id, list) in _list)
            {
                yield return (id, list);
            }
        }

        public void Load(IBmsViewModel source, Predicate<Note>? selector = null, double length = 0)
        {
            Clear();
            selector ??= BmsExtensions.IsPlayableSound;
            foreach (var (pos, notes) in source.CurrentTimeline.EnumerateList())
            {
                var time = source.Position2Time(pos);
                if (length is not 0 && time >= length)
                {
                    break;
                }
                foreach (var note in notes.AsSpan())
                {
                    if (selector(note))
                    {
                        Add(time, note);
                    }
                }
            }
            SetEnd(length);
        }

        public void Load(IBmsViewModel source, Selection selection, Predicate<Note>? selector = null, double length = 0)
        {
            Clear();
            selector ??= BmsExtensions.IsPlayableSound;
            foreach (var (pos, note) in selection)
            {
                var time = source.Beat2Time(pos);
                if (length is not 0 && time >= length)
                {
                    break;
                }
                if (selector(note))
                {
                    Add(time, note);
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

        private void Add(double time, Note note)
        {
            var id = note.Value;
            if (id is > 0)
            {
                Add((int)id, time, note.IsBgm());
            }
        }

        private void Add(int id, double time, bool isBgm)
        {
            if (time < _first)
            {
                _first = time;
            }
            if (time > _last)
            {
                _last = time;
            }
            var list = _list.GetOrAdd(id);
            if (list.Count is > 0)
            {
                list[^1] = list[^1].SetLength(time);
            }
            list.Add(new(time, isBgm));
        }

        private void SetEnd(double time)
        {
            if (time > _last)
            {
                foreach (var (_, list) in _list)
                {
                    list[^1] = list[^1].SetLength(time);
                }
                _last = time;
            }
        }
    }
}
