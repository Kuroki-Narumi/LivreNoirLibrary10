using System;
using LivreNoirLibrary.Media.Bms.Play;

namespace LivreNoirLibrary.Media.Bms
{
    public static class BmsTimerExtensions
    {
        extension (IBmsTimer obj)
        {
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
                return period is > 0 && obj.TryGet(timerId, time, out var relativeTime) ? (int)(relativeTime * maxPattern / (period + 0.0001)) : 0;
            }
        }
    }
}
