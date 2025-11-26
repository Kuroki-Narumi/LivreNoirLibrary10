using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Numerics;
using System;
using System.Windows.Media.Media3D;

namespace LivreNoirLibrary.Benchmark
{
    public class RectTest
    {
        const int Count = 1000000;

        readonly Item[] _items = new Item[Count];
        internal readonly bool[] _results = new bool[Count];

        private struct Item
        {
            public int x;
            public int y;
            public int w;
            public int h;
            public int width;
            public int height;
        }

        [GlobalSetup]
        public void Setup()
        {
            var random = new XorShift(123456789);
            const int MaxValue = 100000;
            for (var i = 0; i < Count; i++)
            {
                _items[i] = new()
                {
                    x = random.Next(-MaxValue, MaxValue),
                    y = random.Next(-MaxValue, MaxValue),
                    w = random.Next(MaxValue),
                    h = random.Next(MaxValue),
                    width = random.Next(MaxValue),
                    height = random.Next(MaxValue),
                };
            }
        }

        [Benchmark]
        public void If()
        {
            for (var i = 0; i < Count; i++)
            {
                ref Item item = ref _items[i];
                if (item.x is < 0)
                {
                    item.w += item.x;
                    item.x = 0;
                }
                if (item.y is < 0)
                {
                    item.h += item.y;
                    item.y = 0;
                }
                item.w = Math.Min(item.w, item.width - item.x);
                item.h = Math.Min(item.h, item.height - item.y);
                if (item.w is > 0 && item.h is > 0)
                {
                    _results[i] = true;
                }
                else
                {
                    item.x = item.y = item.w = item.h = 0;
                    _results[i] = false;
                }
            }
        }

        [Benchmark]
        public void Max()
        {
            for (var i = 0; i < Count; i++)
            {
                ref Item item = ref _items[i];
                var x = Math.Max(item.x, 0);
                var y = Math.Max(item.y, 0);
                item.w = Math.Min(item.x + item.w, item.width) - x;
                item.h = Math.Min(item.y + item.h, item.height) - y;
                if (item.w is > 0 && item.h is > 0)
                {
                    item.x = x;
                    item.y = y;
                    _results[i] = true;
                }
                else
                {
                    item.x = item.y = item.w = item.h = 0;
                    _results[i] = false;
                }
            }
        }

        [Benchmark]
        public void NoBranch()
        {
            for (var i = 0; i < Count; i++)
            {
                ref Item item = ref _items[i];
                var x1 = Math.Max(item.x, 0);
                var y1 = Math.Max(item.y, 0);
                var x2 = Math.Min(item.x + item.w, item.width);
                var y2 = Math.Min(item.y + item.h, item.height);
                var isValid = x1 < x2 && y1 < y2;
                item.x = isValid ? x1 : 0;
                item.y = isValid ? y1 : 0;
                item.w = isValid ? x2 - x1 : 0;
                item.h = isValid ? y2 - y1 : 0;
                _results[i] = isValid;
            }
        }
    }
}
