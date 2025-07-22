using System;
using System.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;

namespace LivreNoirLibrary.Media.Midi
{
    public class TonalityTimeline : RationalTimeline<Tonality>, IDumpable<TonalityTimeline>
    {
        public const string Chid = "LNMdTn";

        public static TonalityTimeline Load(BinaryReader reader)
        {
            TonalityTimeline timeline = new();
            timeline.ProcessLoad(reader);
            return timeline;
        }

        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, Tonality.Load, Chid);
        public void Dump(BinaryWriter writer) => ProcessDump(writer, Dumpable.Dump, Chid);

        public void ExtendToEvent(RawTimeline timeline, long ticksPerWholeNote)
        {
            foreach (var (pos, value) in this)
            {
                var tick = IObject.GetTick(pos, ticksPerWholeNote);
                timeline.Add(tick, new RawData.Tonality(value));
            }
        }

        public Tonality Get(Rational position) => Get(position, default);
    }
}
