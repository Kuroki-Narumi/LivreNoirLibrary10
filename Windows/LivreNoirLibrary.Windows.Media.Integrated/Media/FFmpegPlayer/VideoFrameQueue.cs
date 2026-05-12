using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.Media
{
    public class VideoFrameQueue(int capacity = 4) : BacketQueue<VideoFrameInfo, VideoFrameBacket>(capacity, false)
    {
    }
}
