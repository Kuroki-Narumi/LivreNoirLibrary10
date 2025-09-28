using System;
using Dr = System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public sealed class BgaElement : ScreenElementBase
    {
        private string _directory = "";
        private readonly TimeCounter _timeCounter = new();
        private readonly BgaTimingList _timingList = new();
        private readonly MediaBufferCollection _cache = new();

        public bool RewindMissLayer { get; set; }
        public long MissLayerDisplayTime { get; set; }
        public long LastMissedTime { get; set; }

        public void Setup(BgaSetting settings)
        {
            _cache.RequiredRect = settings.Rect;
            RewindMissLayer = settings.RewindMissLayer;
            MissLayerDisplayTime = (long)(settings.MissLayerDisplayTime * TimeSpan.TicksPerSecond);
        }

        public void Load(IBmsData data, string directory)
        {
            var counter = _timeCounter;
            counter.Load(data);
            if (directory != _directory)
            {
                _directory = directory;
                _cache.Clear();
            }
            _timingList.Load(data, counter, directory);
        }

        public override void Render(DrawingContext drawingContext, long currentTick)
        {
            var timings = _timingList;
            var cache = _cache;
            drawingContext.DrawRectangle(Brushes.Black, null, cache.RequiredRect.ToRect());
            var rewind = RewindMissLayer;
            var lastMiss = LastMissedTime;
            var showMissLayer = lastMiss is >= 0 && (currentTick - lastMiss >= MissLayerDisplayTime);
            foreach (var channel in BmsUtils.BgaChannelList)
            {
                if (timings.TryGetValue(channel, currentTick, out var startTick, out var path))
                {
                    WriteableBitmap? bitmap = null;
                    Rect rect = default;
                    if (channel.IsMissLayer())
                    {
                        if (showMissLayer)
                        {
                            (bitmap, rect) = cache.GetBitmap(path, currentTick - (rewind ? lastMiss : startTick));
                        }
                    }
                    else
                    {
                        (bitmap, rect) = cache.GetBitmap(path, currentTick - startTick);
                    }
                    if (bitmap is not null)
                    {
                        drawingContext.DrawImage(bitmap, rect);
                    }
                }
            }
        }
    }
}
