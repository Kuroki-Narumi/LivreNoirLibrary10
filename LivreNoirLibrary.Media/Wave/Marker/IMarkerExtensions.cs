using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public static class IMarkerExtensions
    {
        public static Marker[] GetMarkerArray<T>(this T obj) where T : IMarker => [.. obj.Markers];
        public static void SetMarkerArray<T>(this T obj, Marker[] array) where T : IMarker => obj.Markers.Load(array);

        public static bool TryGetNearestMarker<T>(this T obj, long position, out Marker marker) where T : IMarker => obj.Markers.TryGetNearest(position, out marker);
        public static bool TryGetNextMarker<T>(this T obj, long position, out Marker marker) where T : IMarker => obj.Markers.TryGet(position, SearchMode.Next, out marker);
        public static bool TryGetPreviousMarker<T>(this T obj, long position, out Marker marker) where T : IMarker => obj.Markers.TryGet(position, SearchMode.Previous, out marker);

        public static long GetMarkerLength<T>(this T obj, in Marker marker) where T : IMarker => obj.Markers.GetLength(marker, obj.SampleLength);

        public static bool MoveMarkerToMinimum<T>(this T obj, long maxDif = 44)
             where T : IMarker, IWaveBuffer
        {
            if (maxDif <= 0) { return false; }
            var (poss, values) = obj.Markers.GetLists();
            var count = poss.Count - 1;
            var data = obj.Data;
            var chPos = (stackalloc long[obj.Channels]);
            var ch = obj.Channels;
            var flag = false;
            var limit = (obj as IWaveBuffer).SampleLength;
            for (var i = 0; i <= count; i++)
            {
                if (values[i] is not Marker.IgnoreName)
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

        public static (long NewLeft, long NewRight) ShiftMarker<T>(this T obj, long position, long amount, bool singleMove)
             where T : IMarker
        {
            var poss = obj.Markers.GetPosList();
            var count = poss.Count;
            var index = poss.FindNearestIndex(position);
            var leftLimit = index is > 0 ? poss[index - 1] + 1 : 0;
            var rightLimit = (singleMove && index < count - 1 ? poss[index + 1] : obj.SampleLength) - 1;

            var left = poss[index];
            if (singleMove)
            {
                poss[index] = left = Math.Clamp(left + amount, leftLimit, rightLimit);
            }
            else
            {
                if (left + amount < leftLimit)
                {
                    amount = leftLimit - left;
                }
                var right = poss[^1];
                if (right + amount > rightLimit)
                {
                    amount = rightLimit - right;
                }
                left += amount;
                for (var i = index; i < count; i++)
                {
                    poss[i] += amount;
                }
            }
            return (left, rightLimit + 1);
        }

        public  static long GetSliceCount<T>(this T obj) where T : IMarker => obj.Markers.GetValidCount();

        public static IEnumerable<MarkerInfo> EachSlice<T>(this T obj, bool skipIgnoreName = true) where T : IMarker => obj.Markers.EachMarkerWithLength(obj.SampleLength, skipIgnoreName);
    }
}
