using System.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class Cue : Container<CueData>, IRiffChunk<Cue>
    {
        public override string Chid => ChunkIds.Cue;

        public static Cue LoadContents(BinaryReader s, uint length)
        {
            Cue data = [];
            data.LoadData(s);
            return data;
        }

        public void Sort() => Sort((a, b) => (int)((long)a.SampleOffset - b.SampleOffset));

        public void SetAt(uint offset, string chid = ChunkIds.Data)
        {
            Add(new()
            {
                Id = FindId(),
                Position = offset,
                SampleOffset = offset,
                Chid = chid,
            });
        }

        public void SetAt(uint offset, int id, string chid = ChunkIds.Data)
        {
            var c = Get(id);
            if (c is null)
            {
                Add(new()
                {
                    Id = id,
                    Position = offset,
                    SampleOffset = offset,
                    Chid = chid,
                });
            }
            else
            {
                c.Position = c.SampleOffset = offset;
            }
        }
    }
}
