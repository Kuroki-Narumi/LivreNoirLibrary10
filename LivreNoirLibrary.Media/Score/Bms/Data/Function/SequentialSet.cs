using System;
using System.Runtime.InteropServices;
using System.Threading;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        public Selection CreateSequentialSelection(SequentialSetOptions options) => CreateSequentialSelection(options, null, CancellationToken.None);
        public Selection CreateSequentialSelection(SequentialSetOptions options, ProgressReporter? reporter, CancellationToken c)
        {
            var lane = options.Lane;
            var list = options.IdList;
            var interval = options.Interval;
            var root = options.RootDirectory;

            Selection selection = [];
            var pos = GetBeat(options.Position);
            void Add(int index)
            {
                selection.Add(GetPosition(pos), pos, new(NoteType.Normal, lane, index));
            }

            var max = list.Count;
            var i = 0;
            reporter?.Report("Sequential Set", "start process...");
            c.ThrowIfCancellationRequested();
            if (BmsUtils.IsSoundLane(lane) && options.IntervalAuto)
            {
                interval = options.Resolution;
                TimeCounter counter = new(this);
                var head = pos;
                var second = counter.Beat2Second(pos);
                foreach (var index in CollectionsMarshal.AsSpan(list))
                {
                    c.ThrowIfCancellationRequested();
                    if (index is > 0 && TryGetWavePath(index, root, out _, out var path) && AudioUtils.TryGetWaveStream(path, out var audio))
                    {
                        var beat = counter.Second2Beat(second);
                        pos = (beat - head).Round(interval) + head;
                        Add(index);
                        second += audio.Duration;
                    }
                    reporter?.ReportFraction(i++, max);
                }
            }
            else
            {
                foreach (var index in CollectionsMarshal.AsSpan(list))
                {
                    if (index is > 0)
                    {
                        Add(index);
                    }
                    pos += interval;
                    reporter?.ReportFraction(i++, max);
                }
            }
            return selection;
        }
    }
}
