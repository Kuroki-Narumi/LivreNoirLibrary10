using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsTimer : IClear
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

        public static int GetFrameIndex(double relativeTime, in TextureData texData)
        {
            var period = texData.LoopPeriod;
            var maxPattern = texData.DivX * texData.DivY;
            return period is > 0 ? (int)(relativeTime * maxPattern / period) : 0;
        }

        public int GetFrameIndex(TimerId timerId, double time, in TextureData texData)
        {
            var period = texData.LoopPeriod;
            var maxPattern = texData.DivX * texData.DivY;
            return period is > 0 && TryGet(timerId, time, out var relativeTime) ? (int)(relativeTime * maxPattern / (period + 0.0001)) : 0;
        }

        public static TimerId Lane2TimerId(int lane) => (TimerId)(1000 + lane * 10);

        public static bool TryGetButtonTimer(Channel channel, out TimerId id)
        {
            if (channel.TryGetLane(out var lane) && lane is > 0)
            {
                id = Lane2TimerId(lane);
                return true;
            }
            id = 0;
            return false;
        }
    }
}
