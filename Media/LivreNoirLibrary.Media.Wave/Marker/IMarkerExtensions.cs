using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public static class IMarkerExtensions
    {
        public static Marker[] GetMarkerArray<T>(this T obj) where T : IMarkerContainer => [.. obj.Markers];
        public static void SetMarkerArray<T>(this T obj, Marker[] array) where T : IMarkerContainer => obj.Markers.Load(array);

        public static bool TryGetNearestMarker<T>(this T obj, long position, out Marker marker) where T : IMarkerContainer => obj.Markers.TryGetNearest(position, out marker);
        public static bool TryGetNextMarker<T>(this T obj, long position, out Marker marker) where T : IMarkerContainer => obj.Markers.TryGet(position, SearchMode.Next, out marker);
        public static bool TryGetPreviousMarker<T>(this T obj, long position, out Marker marker) where T : IMarkerContainer => obj.Markers.TryGet(position, SearchMode.Previous, out marker);

        public static long GetMarkerLength<T>(this T obj, in Marker marker) where T : IMarkerContainer => obj.Markers.GetLength(marker, obj.Length);

        public static bool MoveMarkerToMinimum<T>(this T obj, long maxDif = 44)
             where T : IMarkerContainer, IWaveBuffer
        {
            if (maxDif <= 0) { return false; }
            var (poss, values) = obj.Markers.GetLists();
            var count = poss.Count - 1;
            var data = obj.Data;
            var chPos = (stackalloc long[obj.Channels]);
            var ch = obj.Channels;
            var flag = false;
            var limit = obj.SampleLength;
            for (var i = 0; i <= count; i++)
            {
                if (values[i] is not Constants.IgnoreMarkerName)
                {
                    var pos = poss[i];
                    if (pos >= limit)
                    {
                        break;
                    }
                    var leftLimit = Math.Max(pos - maxDif, i is > 0 ? poss[i - 1] + 1 : 0);
                    var rightLimit = Math.Min(pos + maxDif, (i < count ? poss[i + 1] : limit) - 1);
                    for (var c = 0; c < ch; c++)
                    {
                        var minValue = 1f;
                        var minPos = pos;
                        for (var p = pos; p >= leftLimit; p--)
                        {
                            var value = data[(int)pos * ch + c];
                            value *= value;
                            if (value < minValue)
                            {
                                minValue = value;
                                minPos = pos;
                            }
                        }
                        for (var p = pos + 1; p <= rightLimit; p++)
                        {
                            var value = data[(int)pos * ch + c];
                            value *= value;
                            if (value < minValue)
                            {
                                minValue = value;
                                minPos = pos;
                            }
                        }
                        chPos[c] = minPos;
                    }
                    var min = chPos.Min();
                    if (poss[i] != min)
                    {
                        poss[i] = min;
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public static (long NewLeft, long NewRight) ShiftMarker<T>(this T obj, long start, long amount, bool singleMove)
             where T : IMarkerContainer => obj.Markers.Shift(start, amount, obj.Length, singleMove);

        public  static long GetSliceCount<T>(this T obj) where T : IMarkerContainer => obj.Markers.GetValidCount();

        public static IEnumerable<MarkerInfo> EachSlice<T>(this T obj, bool skipIgnoreName = true)
            where T : IMarkerContainer => obj.Markers.EnumerateWithLength(obj.Length, skipIgnoreName);
    }
}
