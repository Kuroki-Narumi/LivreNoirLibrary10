using System.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class PlayList : Container<PlayListData>, IRiffChunk<PlayList>
    {
        public override string Chid => ChunkIds.PlayList;

        public static PlayList LoadContents(BinaryReader s, uint length)
        {
            PlayList data = [];
            data.LoadData(s);
            return data;
        }
    }
}
