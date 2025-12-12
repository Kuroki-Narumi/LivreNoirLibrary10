using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class BmsTimer : IBmsTimer
    {
        private readonly Dictionary<TimerId, double> _timers = [];

        public void Clear()
        {
            _timers.Clear();
        }

        public void Set(TimerId id, double time) => _timers[id] = time;
        public bool Remove(TimerId id) => _timers.Remove(id);

        public double Get(TimerId id, double time)
        {
            if (_timers.TryGetValue(id, out var start))
            {
                return time - start;
            }
            return -1;
        }

        public bool TryGet(TimerId id, double time, out double relativeTime)
        {
            relativeTime = _timers.TryGetValue(id, out var start) ? time - start : -1;
            return relativeTime is >= 0;
        }
    }
}
