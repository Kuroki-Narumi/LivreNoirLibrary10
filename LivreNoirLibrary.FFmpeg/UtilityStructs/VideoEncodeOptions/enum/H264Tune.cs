using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public enum H264Tune
    {
        None = 0,
        film, 
        animation, 
        grain, 
        stillimage,
        psnr, 
        ssim,
        fastdecode,
        zerolatency
    }
}
