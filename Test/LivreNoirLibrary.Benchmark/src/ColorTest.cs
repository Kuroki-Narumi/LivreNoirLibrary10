using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Collections;
using System.Numerics;

namespace LivreNoirLibrary.Benchmark
{
    public unsafe class ColorTest
    {
        public const float InvertFactor = 1f / 255f;
        private static readonly float[] _scRgbTable = CreateScRgbTable();
        const int RgbTableSize = 65536;
        const float RgbScale = 65535;
        private static readonly byte[] _rgbTable = CreateRgbTable();

        static float[] CreateScRgbTable()
        {
            var table = new float[256];
            // 低輝度領域
            for (var i = 1; i <= 10; i++)
            {
                table[i] = i * InvertFactor / 12.92f;
            }
            // 高輝度領域
            for (var i = 11; i <= 254; i++)
            {
                table[i] = MathF.Pow((i * InvertFactor + 0.055f) / 1.055f, 2.4f);
            }
            table[255] = 1;
            return table;
        }

        public static float RgbToScRgbImpl(byte value) => value switch
        {
            0 => 0,
            <= 10 => value * InvertFactor / 12.92f,
            <= 254 => MathF.Pow((value * InvertFactor + 0.055f) / 1.055f, 2.4f),
            255 => 255,
        };

        public static float RgbToScRgb(byte value) => _scRgbTable[value];
        public static float GetFloat(byte value) => value * InvertFactor;

        static byte[] CreateRgbTable()
        {
            var table = new byte[RgbTableSize];
            for (var i = 0; i < RgbTableSize; i++)
            {
                table[i] = ScRgbToRgbImpl(i / RgbScale);
            }
            return table;
        }

        public static byte ScRgbToRgbImpl(float value) => value switch
        {
            not > 0 => 0,
            <= 0.0031308f => (byte)((value * 12.92f * 255.0f) + 0.5f),
            < 1 => (byte)(((MathF.Pow(value, 1.0f / 2.4f) * 1.055f - 0.055f) * 255.0f) + 0.5f),
            _ => 255,
        };

        public static byte ScRgbToRgb(float value)
        {
            var index = Math.Clamp((int)(value * RgbScale), 0, RgbTableSize - 1);
            return _rgbTable[index];
        }

        public static byte ScRgbToRgb2(float value)
        {
            var index = _scRgbTable.BinarySearch(Math.Min(value, 1));
            return (byte)(index is >= 0 ? index : ~index);
        }

        public static byte GetByte(float value) => (byte)Math.Clamp(value * 255f, 0, 255);

        public static float ConvertByIf(byte value, int index) => index % 4 is 3 ? GetFloat(value): RgbToScRgb(value);
        public static byte ConvertBackByIf(float value, int index) => index % 4 is 3 ? GetByte(value) : ScRgbToRgb(value);

        static readonly Func<byte, float>[] _rgbFuncs = CreateRgbFuncs();
        static readonly Func<float, byte>[] _scRgbFuncs = CreateScRgbFuncs();

        static Func<byte, float>[] CreateRgbFuncs()
        {
            var count = Vector<float>.Count;
            var ary = new Func<byte, float>[count];
            for (var i = 0; i <  count; i++)
            {
                ary[i] = i % 4 is 3 ? GetFloat : RgbToScRgb;
            }
            return ary;
        }

        static Func<float, byte>[] CreateScRgbFuncs()
        {
            var count = Vector<float>.Count;
            var ary = new Func<float, byte>[count];
            for (var i = 0; i <  count; i++)
            {
                ary[i] = i % 4 is 3 ? GetByte : ScRgbToRgb;
            }
            return ary;
        }

        public static float ConvertByTable(byte value, int index) => _rgbFuncs[index](value);
        public static byte ConvertBackByTable(float value, int index) => _scRgbFuncs[index](value);

        public const int Count = 1000;
        private readonly byte[] _base = new byte[Count];
        private readonly float[] _convert = new float[Count];
        private readonly byte[] _convertBack = new byte[Count];

        [GlobalSetup]
        public void Setup()
        {
            var rand = Random.Shared;
            rand.NextBytes(_base);
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = rand.NextSingle();
            }
        }

        /*
        [Benchmark]
        public void Convert_Calculation()
        {
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = RgbToScRgbImpl(_base[i]);
            }
        }

        [Benchmark]
        public void Convert_Table()
        {
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = _scRgbTable[_base[i]];
            }
        }

        [Benchmark]
        public void ConvertBack_Calculation()
        {
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgbImpl(_convert[i]);
            }
        }

        [Benchmark]
        public void ConvertBack_Table()
        {
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgb(_convert[i]);
            }
        }

        [Benchmark]
        public void ConvertBack_BinarySearch()
        {
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgb2(_convert[i]);
            }
        }
        */

        [Benchmark]
        public void Convert_If()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ConvertByIf(_base[i], i % count);
            }
        }

        [Benchmark]
        public void Convert_Table()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ConvertByTable(_base[i], i % count);
            }
        }

        [Benchmark]
        public void ConvertBack_If()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ConvertBackByIf(_convert[i], i % count);
            }
        }

        [Benchmark]
        public void ConvertBack_Table()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ConvertBackByTable(_convert[i], i % count);
            }
        }
    }
}
