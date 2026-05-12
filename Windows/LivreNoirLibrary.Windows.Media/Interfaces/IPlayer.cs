using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.Media
{
    public interface IPlayer
    {
        public bool IsPlayable { get; }
        public bool IsPlaying { get; set; }
    }

    public static class IPlayerExtensions
    {
        extension(IPlayer player)
        {
            public void Play()
            {
                if (player.IsPlayable && !player.IsPlaying)
                {
                    player.IsPlaying = true;
                }
            }

            public void Stop()
            {
                player.IsPlaying = false;
            }
        }
    }
}
