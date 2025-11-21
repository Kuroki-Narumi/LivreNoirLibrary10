using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsTimer : IClear
    {
        private readonly Dictionary<TimerId, long> _timers = [];

        public void Clear()
        {
            _timers.Clear();
        }

        public void Set(TimerId id, long absoluteTick) => _timers[id] = absoluteTick;
        public bool Remove(TimerId id) => _timers.Remove(id);

        public long Get(TimerId id, long absolutTick)
        {
            if (_timers.TryGetValue(id, out var startTick))
            {
                return absolutTick - startTick;
            }
            return -1;
        }

        public bool TryGet(TimerId id, long absoluteTick, out long relativeTick)
        {
            relativeTick = _timers.TryGetValue(id, out var startTick) ? absoluteTick - startTick : -1;
            return relativeTick is >= 0;
        }

        public static int GetFrameIndex(long relativeTick, in TextureData texData)
        {
            var period = texData.LoopPeriod;
            var maxPattern = texData.DivX * texData.DivY;
            return period is > 0 ? (int)(relativeTick * maxPattern / period) : 0;
        }

        public int GetFrameIndex(TimerId timerId, long absoluteTick, in TextureData texData)
        {
            var period = texData.LoopPeriod;
            var maxPattern = texData.DivX * texData.DivY;
            return period is > 0 && TryGet(timerId, absoluteTick, out var relativeTick) ? (int)(relativeTick * maxPattern / period) : 0;
        }
    }
}
