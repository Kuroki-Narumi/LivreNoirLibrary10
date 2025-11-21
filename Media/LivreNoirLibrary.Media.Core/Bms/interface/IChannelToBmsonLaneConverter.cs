using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IChannelToBmsonLaneConverter
    {
        bool TryConvert(Channel channel, out int lane);
    }
}
