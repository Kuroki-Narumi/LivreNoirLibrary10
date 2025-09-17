using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Relation : IEnumerable<Relation.Channel>
    {
        private readonly Channel[] _data;

        public readonly float Length;
        public readonly int Channels;
        public ref Channel this[int channel] => ref _data[channel];

        public Relation(Analysis data1, Analysis data2)
        {
            var ch = data1.Channels;
            Channels = ch;
            _data = new Channel[ch];
            var  len = (float)(data2.Length - data1.Length) / data1.Length;
            Length = len * len;
            for (int c = 0; c < ch; c++)
            {
                _data[c] = new(data1[c], data2[c]);
            }
        }

        public static bool IsLengthSimilar(long len1, long len2)
        {
            var len = (float)(len1 - len2) / len1;
            len *= len;
            return len < 0.2;
        }

        public IEnumerator<Channel> GetEnumerator()
        {
            foreach (var channel in _data)
            {
                yield return channel;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => $"{{Length={Length}, {string.Join(", ", _data.Select((c, i) => $"Ch{i}:{c}"))}}}";
    }
}
