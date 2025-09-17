using LivreNoirLibrary.Media.BM3;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Midi
{

    public ref struct PackedNoteExtendState(PackOptions options, 
                                            IScore targetData,
                                            ITimeline targetTimeline,
                                            Dictionary<int, int> sideChainMap,
                                            Rational startOffset)
    {
        public readonly IScore TargetData = targetData;
        public readonly ITimeline TargetTimeline = targetTimeline;
        public readonly Dictionary<int, int> SideChainMap = sideChainMap;
        public readonly int Interval = options.Interval;
        public readonly Rational PortamentoLength = options.PortamentoLength;
        public readonly bool CutTail = options.CutTail;
        public readonly Rational TailMargin = options.TailMargin;
        public Rational Offset = startOffset;
        public int LastBarLength;
        public int LastTempo;
        public int LastNoteNumber;
        public readonly Dictionary<int, int> LastControl = [];
    }
}
