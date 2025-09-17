using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Wave.Chunks;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public static class MarkerExtension
    {
        public static MarkerCollection GetCueMarkers<T>(this T meta)
            where T : IWaveMetaData
        {
            MarkerCollection result = [];
            GetCueMarkers(meta, result);
            return result;
        }

        public static void GetCueMarkers<T>(this T meta, MarkerCollection target)
            where T : IWaveMetaData
        {
            target.Clear();
            if (meta.TryGetChunk<Cue>(out var cues))
            {
                meta.TryGetList(ChunkIds.Associated, out var adtl);
                foreach (var cue in cues)
                {
                    var id = cue.Id;
                    var position = cue.SampleOffset;
                    var name = adtl?.GetText(ChunkIds.Label, id);
                    target.Set(position, name);
                }
            }
        }

        public static void SetCueMarkers<T>(this T meta, MarkerCollection source, long totalSamples)
            where T : IWaveMetaData
        {
            var cues = meta.GetOrCreateChunk<Cue>(() => []);
            var adtl = meta.GetOrCreateAdtlList();
            // clearnup
            HashSet<int> ids = [.. cues.Select(c => c.Id)];
            cues.Clear();
            var adList = adtl.SubChunks;
            for (var i = 0; i < adList.Count;)
            {
                var item = adList[i];
                if (item.Chid is ChunkIds.Label or ChunkIds.LTxt &&
                    item is IIdChunk idc && ids.Contains(idc.Id))
                {
                    adList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            // apply
            var purpose = "beat".Shared();
            foreach (var (i, name, pos, length) in source.EachMarkerWithLength(totalSamples, false))
            {
                CueData data = new(pos) { Id = i };
                cues.Add(data);
                if (!string.IsNullOrEmpty(name))
                {
                    adtl.Add(ChunkIds.Label, i, name);
                }
                adList.Add(new LTxt() { Purpose = purpose, SampleLength = (uint)length });
            }
            if (cues.Count is 0)
            {
                meta.RemoveChunk<Cue>();
            }
            if (adtl.Count is 0)
            {
                meta.RemoveAdtlList();
            }
        }
    }
}
