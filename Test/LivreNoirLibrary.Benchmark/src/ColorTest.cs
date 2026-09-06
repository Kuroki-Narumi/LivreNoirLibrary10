using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Benchmark
{
    public unsafe class ColorTest
    {
        public const float InvertFactor = 1f / 255f;
        private static readonly float[] _scRgbTable = CreateScRgbTable();
        private static readonly float[] _linearTable = CreateLinearTable();
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

        static float[] CreateLinearTable()
        {
            var table = new float[256];
            for (var i = 0; i < 256; i++)
            {
                table[i] = i * InvertFactor;
            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RgbToScRgbByStack(byte value)
        {
            ref var first = ref MemoryMarshal.GetArrayDataReference(_scRgbTable);
            return Unsafe.Add(ref first, value);
        }

        public static float ToFloat(byte value) => value * InvertFactor;
        public static float ToFloatByTable(byte value)
        {
            ref var first = ref MemoryMarshal.GetArrayDataReference(_linearTable);
            return Unsafe.Add(ref first, value);
        }

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
            var index = Math.Clamp((int)(value * RgbScale + 0.5f), 0, RgbTableSize - 1);
            return _rgbTable[index];
        }

        public static byte ScRgbToRgb2(float value)
        {
            var index = Math.Clamp((int)(value * RgbScale + 0.5f), 0, RgbTableSize - 1);
            ref var first = ref MemoryMarshal.GetArrayDataReference(_rgbTable);
            return Unsafe.Add(ref first, index);
        }

        public static byte GetByte(float value) => (byte)Math.Clamp(value * 255f, 0, 255);

        public static float ConvertByIf(byte value, int index) => index % 4 is 3 ? GetFloat(value): RgbToScRgb(value);
        public static float ConvertByIf_Stack(byte value, int index) => index % 4 is 3 ? GetFloat(value): RgbToScRgbByStack(value);
        public static byte ConvertBackByIf(float value, int index) => index % 4 is 3 ? GetByte(value) : ScRgbToRgb(value);

        static readonly float[] _bgra_table = CreateBgraTable();

        static float[] CreateBgraTable()
        {
            var count = Vector<float>.Count;
            var source = _scRgbTable.AsSpan();
            float[] result = new float[256 * count];
            for (var c = 0; c < count; c++)
            {
                var offset = 256 * c;
                if (c % 4 is 3)
                {
                    for (var i = 0; i < 256; i++)
                    {
                        result[offset + i] = GetFloat((byte)i);
                    }
                }
                else
                {
                    source.CopyTo(result.AsSpan(offset));
                }
            }
            return result;
        }

        static readonly Func<float, byte>[] _scRgbFuncs = CreateScRgbFuncs();

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

        public static float ConvertByTable(byte value, int index)
        {
            ref var first = ref MemoryMarshal.GetArrayDataReference(_bgra_table);
            return Unsafe.Add(ref first, value + index * 256);
        }

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

        //[Benchmark]
        public void ToFloat_Calc()
        {
            ref var value = ref MemoryMarshal.GetArrayDataReference(_base);
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ToFloat(Unsafe.Add(ref value, i));
            }
        }

        //[Benchmark]
        public void ToFloat_Table()
        {
            ref var value = ref MemoryMarshal.GetArrayDataReference(_base);
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ToFloatByTable(Unsafe.Add(ref value, i));
            }
        }

        //[Benchmark]
        public void Convert_If()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ConvertByIf(_base[i], i % count);
            }
        }

        //[Benchmark]
        public void Convert_If_Stack()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ConvertByIf_Stack(_base[i], i % count);
            }
        }

        //[Benchmark]
        public void Convert_Table()
        {
            var count = Vector<float>.Count;
            for (var i = 0; i < Count; i++)
            {
                _convert[i] = ConvertByTable(_base[i], i % count);
            }
        }

        [Benchmark]
        public void ConvertBack_Direct()
        {
            ref var value = ref MemoryMarshal.GetArrayDataReference(_convert);
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgbImpl(Unsafe.Add(ref value, i));
            }
        }

        [Benchmark]
        public void ConvertBack_Table()
        {
            ref var value = ref MemoryMarshal.GetArrayDataReference(_convert);
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgb(Unsafe.Add(ref value, i));
            }
        }

        [Benchmark]
        public void ConvertBack_Table2()
        {
            ref var value = ref MemoryMarshal.GetArrayDataReference(_convert);
            for (var i = 0; i < Count; i++)
            {
                _convertBack[i] = ScRgbToRgb2(Unsafe.Add(ref value, i));
            }
        }

        public static void Validate()
        {
            var c1 = new ColorTest();
            c1.Setup();
            c1.Convert_If();
            var c2 = new ColorTest();
            c1._base.CopyTo(c2._base);
            c2.Convert_Table();
            Console.WriteLine(c1._convert.SequenceEqual(c2._convert));

            c1.ConvertBack_Direct();
            c2.ConvertBack_Table();
            Console.WriteLine(c1._convertBack.SequenceEqual(c2._convertBack));
        }
    }
}
