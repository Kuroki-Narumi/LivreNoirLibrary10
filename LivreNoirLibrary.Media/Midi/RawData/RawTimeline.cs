using System;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public class RawTimeline : LongMultiTimeline<Event>
    {
        public void ChangeResolution(long numerator, long denominator)
        {
            var c = _pos_list.Count;
            for (var i = 0; i < c; i++)
            {
                _pos_list[i] = _pos_list[i] * numerator / denominator;
            }
        }
    }
}
