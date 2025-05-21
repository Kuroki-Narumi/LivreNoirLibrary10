using System;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public abstract class ContainerDataWithChid : ContainerData
    {
        private string _chid = ChunkIds.Data;

        public string Chid { get => _chid; set => _chid = value.Shared(); }
    }
}
