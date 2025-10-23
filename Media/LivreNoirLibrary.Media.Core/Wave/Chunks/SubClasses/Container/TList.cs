using System.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class TList : Container<TListData>, IRiffChunk<TList>
    {
        public override string Chid => ChunkIds.TList;

        public static TList LoadContents(BinaryReader s, ref uint length)
        {
            TList data = [];
            data.LoadData(s);
            return data;
        }
    }
}
