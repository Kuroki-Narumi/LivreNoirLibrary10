using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public readonly struct Operator_int : IPositionOperator<int>, IComparer<int, int>
    {
        public int Compare(int x, int y) => x.CompareTo(y);
        public bool IsXCloserThanY(int x, int y, int z) => y - z + x - z is > 0;

        public static int Zero { get; } = 0;
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;

        public void Write(BinaryWriter writer, int value) => writer.Write(value);
        public int Read(BinaryReader reader) => reader.ReadInt32();
    }

    public readonly struct Operator_long : IPositionOperator<long>, IComparer<long, long>
    {
        public int Compare(long x, long y) => x.CompareTo(y);
        public bool IsXCloserThanY(long x, long y, long z) => y - z + x - z is > 0;

        public static long Zero { get; } = 0;
        public long Add(long x, long y) => x + y;
        public long Subtract(long x, long y) => x - y;

        public void Write(BinaryWriter writer, long value) => writer.Write(value);
        public long Read(BinaryReader reader) => reader.ReadInt64();
    }

    public readonly struct Operator_ulong : IPositionOperator<ulong>, IComparer<ulong, ulong>
    {
        public int Compare(ulong x, ulong y) => x.CompareTo(y);
        public bool IsXCloserThanY(ulong x, ulong y, ulong z) => y - z + x - z is > 0;

        public static ulong Zero { get; } = 0;
        public ulong Add(ulong x, ulong y) => x + y;
        public ulong Subtract(ulong x, ulong y) => x - y;

        public void Write(BinaryWriter writer, ulong value) => writer.Write(value);
        public ulong Read(BinaryReader reader) => reader.ReadUInt64();
    }

    public readonly struct Operator_double : IPositionOperator<double>, IComparer<double, double>
    {
        public int Compare(double x, double y) => x.CompareTo(y);
        public bool IsXCloserThanY(double x, double y, double z) => y - z + x - z is > 0;

        public static double Zero { get; } = 0;
        public double Add(double x, double y) => x + y;
        public double Subtract(double x, double y) => x - y;

        public void Write(BinaryWriter writer, double value) => writer.Write(value);
        public double Read(BinaryReader reader) => reader.ReadDouble();
    }

    public readonly struct Operator_decimal : IPositionOperator<decimal>, IComparer<decimal, decimal>
    {
        public int Compare(decimal x, decimal y) => x.CompareTo(y);
        public bool IsXCloserThanY(decimal x, decimal y, decimal z) => y - z + x - z is > 0;

        public static decimal Zero { get; } = 0;
        public decimal Add(decimal x, decimal y) => x + y;
        public decimal Subtract(decimal x, decimal y) => x - y;

        public void Write(BinaryWriter writer, decimal value) => writer.Write(value);
        public decimal Read(BinaryReader reader) => reader.ReadDecimal();
    }

    public readonly struct Operator_Rational : IPositionOperator<Rational>, IComparer<Rational, Rational>
    {
        public int Compare(Rational x, Rational y) => x.CompareTo(y);
        public bool IsXCloserThanY(Rational x, Rational y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }

        public static Rational Zero { get; } = 0;
        public Rational Add(Rational x, Rational y) => x + y;
        public Rational Subtract(Rational x, Rational y) => x - y;

        public void Write(BinaryWriter writer, Rational value) => writer.Write(value);
        public Rational Read(BinaryReader reader) => reader.ReadRational();
    }

}
