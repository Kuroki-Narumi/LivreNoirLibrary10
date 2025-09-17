using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IChannelNote : INote
    {
        public Channel Channel { get; set; }
    }
}
