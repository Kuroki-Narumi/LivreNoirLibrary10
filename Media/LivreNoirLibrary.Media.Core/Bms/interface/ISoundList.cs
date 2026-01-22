using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ISoundList : ICount
    {
        double FirstTime { get; }
        double LastTime { get; }
        IEnumerable<(int WavId, List<SoundTimingInfo>)> EnumerateSoundList();
    }

    public readonly record struct SoundTimingInfo(double Time, double Length, bool IsBgm)
    {
        public SoundTimingInfo(double time, bool isBgm) : this(time, -1, isBgm) { }
        public SoundTimingInfo SetLength(double endTime) => new(Time, endTime - Time, IsBgm);
    }
}
