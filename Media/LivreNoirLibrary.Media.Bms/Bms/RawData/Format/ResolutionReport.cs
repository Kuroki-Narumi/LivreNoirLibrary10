using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class ResolutionReport : ISafeEnumerable<(int, Channel, long)>, IClear
    {
        private readonly SortedDictionary<ReportKey, long> _data = [];

        public long MaxResolution { get; private set; }

        public void Clear() => _data.Clear();

        public void Add(int number, Channel channel, long resolution)
        {
            var key = new ReportKey(number, channel);
            _data[key] = Math.Max(resolution, _data.GetValueOrDefault(key));
        }

        public IEnumerator<(int, Channel, long)> GetEnumerator()
        {
            foreach (var (key, value) in _data)
            {
                yield return (key.Number, key.Channel, value);
            }
        }

        private readonly struct ReportKey(int number, Channel channel) : IComparable<ReportKey>
        {
            public readonly int Number = number;
            public readonly Channel Channel = channel;

            public int CompareTo(ReportKey other)
            {
                var c = Number.CompareTo(other.Number);
                if (c is not 0)
                {
                    return c;
                }
                return Channel - other.Channel;
            }
        }
    }
}
