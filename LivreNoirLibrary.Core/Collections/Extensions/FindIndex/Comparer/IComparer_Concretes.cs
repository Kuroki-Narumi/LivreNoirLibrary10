using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public readonly struct Comparer_byte_byte : IComparer<byte, byte>
    {
        public bool Equals(byte x, byte y) => x == y;
        public bool LessThan(byte x, byte y) => x < y;
        public bool IsCloser(byte x, byte y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_sbyte : IComparer<byte, sbyte>
    {
        public bool Equals(byte x, sbyte y) => x == y;
        public bool LessThan(byte x, sbyte y) => x < y;
        public bool IsCloser(byte x, byte y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_short : IComparer<byte, short>
    {
        public bool Equals(byte x, short y) => x == y;
        public bool LessThan(byte x, short y) => x < y;
        public bool IsCloser(byte x, byte y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_ushort : IComparer<byte, ushort>
    {
        public bool Equals(byte x, ushort y) => x == y;
        public bool LessThan(byte x, ushort y) => x < y;
        public bool IsCloser(byte x, byte y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_int : IComparer<byte, int>
    {
        public bool Equals(byte x, int y) => x == y;
        public bool LessThan(byte x, int y) => x < y;
        public bool IsCloser(byte x, byte y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_uint : IComparer<byte, uint>
    {
        public bool Equals(byte x, uint y) => x == y;
        public bool LessThan(byte x, uint y) => x < y;
        public bool IsCloser(byte x, byte y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_long : IComparer<byte, long>
    {
        public bool Equals(byte x, long y) => x == y;
        public bool LessThan(byte x, long y) => x < y;
        public bool IsCloser(byte x, byte y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_byte_ulong : IComparer<byte, ulong>
    {
        public bool Equals(byte x, ulong y) => x == y;
        public bool LessThan(byte x, ulong y) => x < y;
        public bool IsCloser(byte x, byte y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_byte_float : IComparer<byte, float>
    {
        public bool Equals(byte x, float y) => x == y;
        public bool LessThan(byte x, float y) => x < y;
        public bool IsCloser(byte x, byte y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_byte_double : IComparer<byte, double>
    {
        public bool Equals(byte x, double y) => x == y;
        public bool LessThan(byte x, double y) => x < y;
        public bool IsCloser(byte x, byte y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_byte_decimal : IComparer<byte, decimal>
    {
        public bool Equals(byte x, decimal y) => x == y;
        public bool LessThan(byte x, decimal y) => x < y;
        public bool IsCloser(byte x, byte y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_byte_Rational : IComparer<byte, Rational>
    {
        public bool Equals(byte x, Rational y) => x == y;
        public bool LessThan(byte x, Rational y) => x < y;
        public bool IsCloser(byte x, byte y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_sbyte_byte : IComparer<sbyte, byte>
    {
        public bool Equals(sbyte x, byte y) => x == y;
        public bool LessThan(sbyte x, byte y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_sbyte : IComparer<sbyte, sbyte>
    {
        public bool Equals(sbyte x, sbyte y) => x == y;
        public bool LessThan(sbyte x, sbyte y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_short : IComparer<sbyte, short>
    {
        public bool Equals(sbyte x, short y) => x == y;
        public bool LessThan(sbyte x, short y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_ushort : IComparer<sbyte, ushort>
    {
        public bool Equals(sbyte x, ushort y) => x == y;
        public bool LessThan(sbyte x, ushort y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_int : IComparer<sbyte, int>
    {
        public bool Equals(sbyte x, int y) => x == y;
        public bool LessThan(sbyte x, int y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_uint : IComparer<sbyte, uint>
    {
        public bool Equals(sbyte x, uint y) => x == y;
        public bool LessThan(sbyte x, uint y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_long : IComparer<sbyte, long>
    {
        public bool Equals(sbyte x, long y) => x == y;
        public bool LessThan(sbyte x, long y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_sbyte_ulong : IComparer<sbyte, ulong>
    {
        public bool Equals(sbyte x, ulong y) => x is >= 0 && (ulong)x == y;
        public bool LessThan(sbyte x, ulong y) => x is < 0 || (ulong)x < y;
        public bool IsCloser(sbyte x, sbyte y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_sbyte_float : IComparer<sbyte, float>
    {
        public bool Equals(sbyte x, float y) => x == y;
        public bool LessThan(sbyte x, float y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_sbyte_double : IComparer<sbyte, double>
    {
        public bool Equals(sbyte x, double y) => x == y;
        public bool LessThan(sbyte x, double y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_sbyte_decimal : IComparer<sbyte, decimal>
    {
        public bool Equals(sbyte x, decimal y) => x == y;
        public bool LessThan(sbyte x, decimal y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_sbyte_Rational : IComparer<sbyte, Rational>
    {
        public bool Equals(sbyte x, Rational y) => x == y;
        public bool LessThan(sbyte x, Rational y) => x < y;
        public bool IsCloser(sbyte x, sbyte y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_short_byte : IComparer<short, byte>
    {
        public bool Equals(short x, byte y) => x == y;
        public bool LessThan(short x, byte y) => x < y;
        public bool IsCloser(short x, short y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_sbyte : IComparer<short, sbyte>
    {
        public bool Equals(short x, sbyte y) => x == y;
        public bool LessThan(short x, sbyte y) => x < y;
        public bool IsCloser(short x, short y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_short : IComparer<short, short>
    {
        public bool Equals(short x, short y) => x == y;
        public bool LessThan(short x, short y) => x < y;
        public bool IsCloser(short x, short y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_ushort : IComparer<short, ushort>
    {
        public bool Equals(short x, ushort y) => x == y;
        public bool LessThan(short x, ushort y) => x < y;
        public bool IsCloser(short x, short y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_int : IComparer<short, int>
    {
        public bool Equals(short x, int y) => x == y;
        public bool LessThan(short x, int y) => x < y;
        public bool IsCloser(short x, short y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_uint : IComparer<short, uint>
    {
        public bool Equals(short x, uint y) => x == y;
        public bool LessThan(short x, uint y) => x < y;
        public bool IsCloser(short x, short y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_long : IComparer<short, long>
    {
        public bool Equals(short x, long y) => x == y;
        public bool LessThan(short x, long y) => x < y;
        public bool IsCloser(short x, short y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_short_ulong : IComparer<short, ulong>
    {
        public bool Equals(short x, ulong y) => x is >= 0 && (ulong)x == y;
        public bool LessThan(short x, ulong y) => x is < 0 || (ulong)x < y;
        public bool IsCloser(short x, short y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_short_float : IComparer<short, float>
    {
        public bool Equals(short x, float y) => x == y;
        public bool LessThan(short x, float y) => x < y;
        public bool IsCloser(short x, short y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_short_double : IComparer<short, double>
    {
        public bool Equals(short x, double y) => x == y;
        public bool LessThan(short x, double y) => x < y;
        public bool IsCloser(short x, short y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_short_decimal : IComparer<short, decimal>
    {
        public bool Equals(short x, decimal y) => x == y;
        public bool LessThan(short x, decimal y) => x < y;
        public bool IsCloser(short x, short y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_short_Rational : IComparer<short, Rational>
    {
        public bool Equals(short x, Rational y) => x == y;
        public bool LessThan(short x, Rational y) => x < y;
        public bool IsCloser(short x, short y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ushort_byte : IComparer<ushort, byte>
    {
        public bool Equals(ushort x, byte y) => x == y;
        public bool LessThan(ushort x, byte y) => x < y;
        public bool IsCloser(ushort x, ushort y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_sbyte : IComparer<ushort, sbyte>
    {
        public bool Equals(ushort x, sbyte y) => x == y;
        public bool LessThan(ushort x, sbyte y) => x < y;
        public bool IsCloser(ushort x, ushort y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_short : IComparer<ushort, short>
    {
        public bool Equals(ushort x, short y) => x == y;
        public bool LessThan(ushort x, short y) => x < y;
        public bool IsCloser(ushort x, ushort y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_ushort : IComparer<ushort, ushort>
    {
        public bool Equals(ushort x, ushort y) => x == y;
        public bool LessThan(ushort x, ushort y) => x < y;
        public bool IsCloser(ushort x, ushort y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_int : IComparer<ushort, int>
    {
        public bool Equals(ushort x, int y) => x == y;
        public bool LessThan(ushort x, int y) => x < y;
        public bool IsCloser(ushort x, ushort y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_uint : IComparer<ushort, uint>
    {
        public bool Equals(ushort x, uint y) => x == y;
        public bool LessThan(ushort x, uint y) => x < y;
        public bool IsCloser(ushort x, ushort y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_long : IComparer<ushort, long>
    {
        public bool Equals(ushort x, long y) => x == y;
        public bool LessThan(ushort x, long y) => x < y;
        public bool IsCloser(ushort x, ushort y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ushort_ulong : IComparer<ushort, ulong>
    {
        public bool Equals(ushort x, ulong y) => x == y;
        public bool LessThan(ushort x, ulong y) => x < y;
        public bool IsCloser(ushort x, ushort y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_ushort_float : IComparer<ushort, float>
    {
        public bool Equals(ushort x, float y) => x == y;
        public bool LessThan(ushort x, float y) => x < y;
        public bool IsCloser(ushort x, ushort y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ushort_double : IComparer<ushort, double>
    {
        public bool Equals(ushort x, double y) => x == y;
        public bool LessThan(ushort x, double y) => x < y;
        public bool IsCloser(ushort x, ushort y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ushort_decimal : IComparer<ushort, decimal>
    {
        public bool Equals(ushort x, decimal y) => x == y;
        public bool LessThan(ushort x, decimal y) => x < y;
        public bool IsCloser(ushort x, ushort y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ushort_Rational : IComparer<ushort, Rational>
    {
        public bool Equals(ushort x, Rational y) => x == y;
        public bool LessThan(ushort x, Rational y) => x < y;
        public bool IsCloser(ushort x, ushort y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_int_byte : IComparer<int, byte>
    {
        public bool Equals(int x, byte y) => x == y;
        public bool LessThan(int x, byte y) => x < y;
        public bool IsCloser(int x, int y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_sbyte : IComparer<int, sbyte>
    {
        public bool Equals(int x, sbyte y) => x == y;
        public bool LessThan(int x, sbyte y) => x < y;
        public bool IsCloser(int x, int y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_short : IComparer<int, short>
    {
        public bool Equals(int x, short y) => x == y;
        public bool LessThan(int x, short y) => x < y;
        public bool IsCloser(int x, int y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_ushort : IComparer<int, ushort>
    {
        public bool Equals(int x, ushort y) => x == y;
        public bool LessThan(int x, ushort y) => x < y;
        public bool IsCloser(int x, int y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_int : IComparer<int, int>
    {
        public bool Equals(int x, int y) => x == y;
        public bool LessThan(int x, int y) => x < y;
        public bool IsCloser(int x, int y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_uint : IComparer<int, uint>
    {
        public bool Equals(int x, uint y) => x == y;
        public bool LessThan(int x, uint y) => x < y;
        public bool IsCloser(int x, int y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_long : IComparer<int, long>
    {
        public bool Equals(int x, long y) => x == y;
        public bool LessThan(int x, long y) => x < y;
        public bool IsCloser(int x, int y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_int_ulong : IComparer<int, ulong>
    {
        public bool Equals(int x, ulong y) => x is >= 0 && (ulong)x == y;
        public bool LessThan(int x, ulong y) => x is < 0 || (ulong)x < y;
        public bool IsCloser(int x, int y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_int_float : IComparer<int, float>
    {
        public bool Equals(int x, float y) => x == y;
        public bool LessThan(int x, float y) => x < y;
        public bool IsCloser(int x, int y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_int_double : IComparer<int, double>
    {
        public bool Equals(int x, double y) => x == y;
        public bool LessThan(int x, double y) => x < y;
        public bool IsCloser(int x, int y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_int_decimal : IComparer<int, decimal>
    {
        public bool Equals(int x, decimal y) => x == y;
        public bool LessThan(int x, decimal y) => x < y;
        public bool IsCloser(int x, int y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_int_Rational : IComparer<int, Rational>
    {
        public bool Equals(int x, Rational y) => x == y;
        public bool LessThan(int x, Rational y) => x < y;
        public bool IsCloser(int x, int y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_uint_byte : IComparer<uint, byte>
    {
        public bool Equals(uint x, byte y) => x == y;
        public bool LessThan(uint x, byte y) => x < y;
        public bool IsCloser(uint x, uint y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_sbyte : IComparer<uint, sbyte>
    {
        public bool Equals(uint x, sbyte y) => x == y;
        public bool LessThan(uint x, sbyte y) => x < y;
        public bool IsCloser(uint x, uint y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_short : IComparer<uint, short>
    {
        public bool Equals(uint x, short y) => x == y;
        public bool LessThan(uint x, short y) => x < y;
        public bool IsCloser(uint x, uint y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_ushort : IComparer<uint, ushort>
    {
        public bool Equals(uint x, ushort y) => x == y;
        public bool LessThan(uint x, ushort y) => x < y;
        public bool IsCloser(uint x, uint y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_int : IComparer<uint, int>
    {
        public bool Equals(uint x, int y) => x == y;
        public bool LessThan(uint x, int y) => x < y;
        public bool IsCloser(uint x, uint y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_uint : IComparer<uint, uint>
    {
        public bool Equals(uint x, uint y) => x == y;
        public bool LessThan(uint x, uint y) => x < y;
        public bool IsCloser(uint x, uint y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_long : IComparer<uint, long>
    {
        public bool Equals(uint x, long y) => x == y;
        public bool LessThan(uint x, long y) => x < y;
        public bool IsCloser(uint x, uint y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_uint_ulong : IComparer<uint, ulong>
    {
        public bool Equals(uint x, ulong y) => x == y;
        public bool LessThan(uint x, ulong y) => x < y;
        public bool IsCloser(uint x, uint y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_uint_float : IComparer<uint, float>
    {
        public bool Equals(uint x, float y) => x == y;
        public bool LessThan(uint x, float y) => x < y;
        public bool IsCloser(uint x, uint y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_uint_double : IComparer<uint, double>
    {
        public bool Equals(uint x, double y) => x == y;
        public bool LessThan(uint x, double y) => x < y;
        public bool IsCloser(uint x, uint y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_uint_decimal : IComparer<uint, decimal>
    {
        public bool Equals(uint x, decimal y) => x == y;
        public bool LessThan(uint x, decimal y) => x < y;
        public bool IsCloser(uint x, uint y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_uint_Rational : IComparer<uint, Rational>
    {
        public bool Equals(uint x, Rational y) => x == y;
        public bool LessThan(uint x, Rational y) => x < y;
        public bool IsCloser(uint x, uint y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_long_byte : IComparer<long, byte>
    {
        public bool Equals(long x, byte y) => x == y;
        public bool LessThan(long x, byte y) => x < y;
        public bool IsCloser(long x, long y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_sbyte : IComparer<long, sbyte>
    {
        public bool Equals(long x, sbyte y) => x == y;
        public bool LessThan(long x, sbyte y) => x < y;
        public bool IsCloser(long x, long y, sbyte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_short : IComparer<long, short>
    {
        public bool Equals(long x, short y) => x == y;
        public bool LessThan(long x, short y) => x < y;
        public bool IsCloser(long x, long y, short z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_ushort : IComparer<long, ushort>
    {
        public bool Equals(long x, ushort y) => x == y;
        public bool LessThan(long x, ushort y) => x < y;
        public bool IsCloser(long x, long y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_int : IComparer<long, int>
    {
        public bool Equals(long x, int y) => x == y;
        public bool LessThan(long x, int y) => x < y;
        public bool IsCloser(long x, long y, int z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_uint : IComparer<long, uint>
    {
        public bool Equals(long x, uint y) => x == y;
        public bool LessThan(long x, uint y) => x < y;
        public bool IsCloser(long x, long y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_long : IComparer<long, long>
    {
        public bool Equals(long x, long y) => x == y;
        public bool LessThan(long x, long y) => x < y;
        public bool IsCloser(long x, long y, long z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_long_ulong : IComparer<long, ulong>
    {
        public bool Equals(long x, ulong y) => x is >= 0 && (ulong)x == y;
        public bool LessThan(long x, ulong y) => x is < 0 || (ulong)x < y;
        public bool IsCloser(long x, long y, ulong z) => (ulong)y - z + (ulong)x - z is > 0;
    }

    public readonly struct Comparer_long_float : IComparer<long, float>
    {
        public bool Equals(long x, float y) => x == y;
        public bool LessThan(long x, float y) => x < y;
        public bool IsCloser(long x, long y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_long_double : IComparer<long, double>
    {
        public bool Equals(long x, double y) => x == y;
        public bool LessThan(long x, double y) => x < y;
        public bool IsCloser(long x, long y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_long_decimal : IComparer<long, decimal>
    {
        public bool Equals(long x, decimal y) => x == y;
        public bool LessThan(long x, decimal y) => x < y;
        public bool IsCloser(long x, long y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_long_Rational : IComparer<long, Rational>
    {
        public bool Equals(long x, Rational y) => x == y;
        public bool LessThan(long x, Rational y) => x < y;
        public bool IsCloser(long x, long y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_byte : IComparer<ulong, byte>
    {
        public bool Equals(ulong x, byte y) => x == y;
        public bool LessThan(ulong x, byte y) => x < y;
        public bool IsCloser(ulong x, ulong y, byte z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ulong_sbyte : IComparer<ulong, sbyte>
    {
        public bool Equals(ulong x, sbyte y) => y is >= 0 && x == (ulong)y;
        public bool LessThan(ulong x, sbyte y) => y is not < 0 && x < (ulong)y;
        public bool IsCloser(ulong x, ulong y, sbyte z) { var zz = (ulong)z; return y - zz + x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_short : IComparer<ulong, short>
    {
        public bool Equals(ulong x, short y) => y is >= 0 && x == (ulong)y;
        public bool LessThan(ulong x, short y) => y is not < 0 && x < (ulong)y;
        public bool IsCloser(ulong x, ulong y, short z) { var zz = (ulong)z; return y - zz + x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_ushort : IComparer<ulong, ushort>
    {
        public bool Equals(ulong x, ushort y) => x == y;
        public bool LessThan(ulong x, ushort y) => x < y;
        public bool IsCloser(ulong x, ulong y, ushort z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ulong_int : IComparer<ulong, int>
    {
        public bool Equals(ulong x, int y) => y is >= 0 && x == (ulong)y;
        public bool LessThan(ulong x, int y) => y is not < 0 && x < (ulong)y;
        public bool IsCloser(ulong x, ulong y, int z) { var zz = (ulong)z; return y - zz + x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_uint : IComparer<ulong, uint>
    {
        public bool Equals(ulong x, uint y) => x == y;
        public bool LessThan(ulong x, uint y) => x < y;
        public bool IsCloser(ulong x, ulong y, uint z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ulong_long : IComparer<ulong, long>
    {
        public bool Equals(ulong x, long y) => y is >= 0 && x == (ulong)y;
        public bool LessThan(ulong x, long y) => y is not < 0 && x < (ulong)y;
        public bool IsCloser(ulong x, ulong y, long z) { var zz = (ulong)z; return y - zz + x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_ulong : IComparer<ulong, ulong>
    {
        public bool Equals(ulong x, ulong y) => x == y;
        public bool LessThan(ulong x, ulong y) => x < y;
        public bool IsCloser(ulong x, ulong y, ulong z) => y - z + x - z is > 0;
    }

    public readonly struct Comparer_ulong_float : IComparer<ulong, float>
    {
        public bool Equals(ulong x, float y) => x == y;
        public bool LessThan(ulong x, float y) => x < y;
        public bool IsCloser(ulong x, ulong y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_double : IComparer<ulong, double>
    {
        public bool Equals(ulong x, double y) => x == y;
        public bool LessThan(ulong x, double y) => x < y;
        public bool IsCloser(ulong x, ulong y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_decimal : IComparer<ulong, decimal>
    {
        public bool Equals(ulong x, decimal y) => x == y;
        public bool LessThan(ulong x, decimal y) => x < y;
        public bool IsCloser(ulong x, ulong y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_ulong_Rational : IComparer<ulong, Rational>
    {
        public bool Equals(ulong x, Rational y) => x == y;
        public bool LessThan(ulong x, Rational y) => x < y;
        public bool IsCloser(ulong x, ulong y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_byte : IComparer<float, byte>
    {
        public bool Equals(float x, byte y) => x == y;
        public bool LessThan(float x, byte y) => x < y;
        public bool IsCloser(float x, float y, byte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_sbyte : IComparer<float, sbyte>
    {
        public bool Equals(float x, sbyte y) => x == y;
        public bool LessThan(float x, sbyte y) => x < y;
        public bool IsCloser(float x, float y, sbyte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_short : IComparer<float, short>
    {
        public bool Equals(float x, short y) => x == y;
        public bool LessThan(float x, short y) => x < y;
        public bool IsCloser(float x, float y, short z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_ushort : IComparer<float, ushort>
    {
        public bool Equals(float x, ushort y) => x == y;
        public bool LessThan(float x, ushort y) => x < y;
        public bool IsCloser(float x, float y, ushort z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_int : IComparer<float, int>
    {
        public bool Equals(float x, int y) => x == y;
        public bool LessThan(float x, int y) => x < y;
        public bool IsCloser(float x, float y, int z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_uint : IComparer<float, uint>
    {
        public bool Equals(float x, uint y) => x == y;
        public bool LessThan(float x, uint y) => x < y;
        public bool IsCloser(float x, float y, uint z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_long : IComparer<float, long>
    {
        public bool Equals(float x, long y) => x == y;
        public bool LessThan(float x, long y) => x < y;
        public bool IsCloser(float x, float y, long z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_ulong : IComparer<float, ulong>
    {
        public bool Equals(float x, ulong y) => x == y;
        public bool LessThan(float x, ulong y) => x < y;
        public bool IsCloser(float x, float y, ulong z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_float : IComparer<float, float>
    {
        public bool Equals(float x, float y) => x == y;
        public bool LessThan(float x, float y) => x < y;
        public bool IsCloser(float x, float y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_double : IComparer<float, double>
    {
        public bool Equals(float x, double y) => x == y;
        public bool LessThan(float x, double y) => x < y;
        public bool IsCloser(float x, float y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_decimal : IComparer<float, decimal>
    {
        public bool Equals(float x, decimal y) => (double)x == (double)y;
        public bool LessThan(float x, decimal y) => (double)x < (double)y;
        public bool IsCloser(float x, float y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_float_Rational : IComparer<float, Rational>
    {
        public bool Equals(float x, Rational y) => x == y;
        public bool LessThan(float x, Rational y) => x < y;
        public bool IsCloser(float x, float y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_byte : IComparer<double, byte>
    {
        public bool Equals(double x, byte y) => x == y;
        public bool LessThan(double x, byte y) => x < y;
        public bool IsCloser(double x, double y, byte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_sbyte : IComparer<double, sbyte>
    {
        public bool Equals(double x, sbyte y) => x == y;
        public bool LessThan(double x, sbyte y) => x < y;
        public bool IsCloser(double x, double y, sbyte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_short : IComparer<double, short>
    {
        public bool Equals(double x, short y) => x == y;
        public bool LessThan(double x, short y) => x < y;
        public bool IsCloser(double x, double y, short z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_ushort : IComparer<double, ushort>
    {
        public bool Equals(double x, ushort y) => x == y;
        public bool LessThan(double x, ushort y) => x < y;
        public bool IsCloser(double x, double y, ushort z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_int : IComparer<double, int>
    {
        public bool Equals(double x, int y) => x == y;
        public bool LessThan(double x, int y) => x < y;
        public bool IsCloser(double x, double y, int z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_uint : IComparer<double, uint>
    {
        public bool Equals(double x, uint y) => x == y;
        public bool LessThan(double x, uint y) => x < y;
        public bool IsCloser(double x, double y, uint z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_long : IComparer<double, long>
    {
        public bool Equals(double x, long y) => x == y;
        public bool LessThan(double x, long y) => x < y;
        public bool IsCloser(double x, double y, long z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_ulong : IComparer<double, ulong>
    {
        public bool Equals(double x, ulong y) => x == y;
        public bool LessThan(double x, ulong y) => x < y;
        public bool IsCloser(double x, double y, ulong z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_float : IComparer<double, float>
    {
        public bool Equals(double x, float y) => x == y;
        public bool LessThan(double x, float y) => x < y;
        public bool IsCloser(double x, double y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_double : IComparer<double, double>
    {
        public bool Equals(double x, double y) => x == y;
        public bool LessThan(double x, double y) => x < y;
        public bool IsCloser(double x, double y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_decimal : IComparer<double, decimal>
    {
        public bool Equals(double x, decimal y) => (double)x == (double)y;
        public bool LessThan(double x, decimal y) => (double)x < (double)y;
        public bool IsCloser(double x, double y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_double_Rational : IComparer<double, Rational>
    {
        public bool Equals(double x, Rational y) => x == y;
        public bool LessThan(double x, Rational y) => x < y;
        public bool IsCloser(double x, double y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_byte : IComparer<decimal, byte>
    {
        public bool Equals(decimal x, byte y) => x == y;
        public bool LessThan(decimal x, byte y) => x < y;
        public bool IsCloser(decimal x, decimal y, byte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_sbyte : IComparer<decimal, sbyte>
    {
        public bool Equals(decimal x, sbyte y) => x == y;
        public bool LessThan(decimal x, sbyte y) => x < y;
        public bool IsCloser(decimal x, decimal y, sbyte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_short : IComparer<decimal, short>
    {
        public bool Equals(decimal x, short y) => x == y;
        public bool LessThan(decimal x, short y) => x < y;
        public bool IsCloser(decimal x, decimal y, short z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_ushort : IComparer<decimal, ushort>
    {
        public bool Equals(decimal x, ushort y) => x == y;
        public bool LessThan(decimal x, ushort y) => x < y;
        public bool IsCloser(decimal x, decimal y, ushort z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_int : IComparer<decimal, int>
    {
        public bool Equals(decimal x, int y) => x == y;
        public bool LessThan(decimal x, int y) => x < y;
        public bool IsCloser(decimal x, decimal y, int z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_uint : IComparer<decimal, uint>
    {
        public bool Equals(decimal x, uint y) => x == y;
        public bool LessThan(decimal x, uint y) => x < y;
        public bool IsCloser(decimal x, decimal y, uint z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_long : IComparer<decimal, long>
    {
        public bool Equals(decimal x, long y) => x == y;
        public bool LessThan(decimal x, long y) => x < y;
        public bool IsCloser(decimal x, decimal y, long z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_ulong : IComparer<decimal, ulong>
    {
        public bool Equals(decimal x, ulong y) => x == y;
        public bool LessThan(decimal x, ulong y) => x < y;
        public bool IsCloser(decimal x, decimal y, ulong z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_float : IComparer<decimal, float>
    {
        public bool Equals(decimal x, float y) => (double)x == (double)y;
        public bool LessThan(decimal x, float y) => (double)x < (double)y;
        public bool IsCloser(decimal x, decimal y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_double : IComparer<decimal, double>
    {
        public bool Equals(decimal x, double y) => (double)x == (double)y;
        public bool LessThan(decimal x, double y) => (double)x < (double)y;
        public bool IsCloser(decimal x, decimal y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_decimal : IComparer<decimal, decimal>
    {
        public bool Equals(decimal x, decimal y) => x == y;
        public bool LessThan(decimal x, decimal y) => x < y;
        public bool IsCloser(decimal x, decimal y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_decimal_Rational : IComparer<decimal, Rational>
    {
        public bool Equals(decimal x, Rational y) => x == y;
        public bool LessThan(decimal x, Rational y) => x < y;
        public bool IsCloser(decimal x, decimal y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_byte : IComparer<Rational, byte>
    {
        public bool Equals(Rational x, byte y) => x == y;
        public bool LessThan(Rational x, byte y) => x < y;
        public bool IsCloser(Rational x, Rational y, byte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_sbyte : IComparer<Rational, sbyte>
    {
        public bool Equals(Rational x, sbyte y) => x == y;
        public bool LessThan(Rational x, sbyte y) => x < y;
        public bool IsCloser(Rational x, Rational y, sbyte z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_short : IComparer<Rational, short>
    {
        public bool Equals(Rational x, short y) => x == y;
        public bool LessThan(Rational x, short y) => x < y;
        public bool IsCloser(Rational x, Rational y, short z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_ushort : IComparer<Rational, ushort>
    {
        public bool Equals(Rational x, ushort y) => x == y;
        public bool LessThan(Rational x, ushort y) => x < y;
        public bool IsCloser(Rational x, Rational y, ushort z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_int : IComparer<Rational, int>
    {
        public bool Equals(Rational x, int y) => x == y;
        public bool LessThan(Rational x, int y) => x < y;
        public bool IsCloser(Rational x, Rational y, int z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_uint : IComparer<Rational, uint>
    {
        public bool Equals(Rational x, uint y) => x == y;
        public bool LessThan(Rational x, uint y) => x < y;
        public bool IsCloser(Rational x, Rational y, uint z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_long : IComparer<Rational, long>
    {
        public bool Equals(Rational x, long y) => x == y;
        public bool LessThan(Rational x, long y) => x < y;
        public bool IsCloser(Rational x, Rational y, long z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_ulong : IComparer<Rational, ulong>
    {
        public bool Equals(Rational x, ulong y) => x == y;
        public bool LessThan(Rational x, ulong y) => x < y;
        public bool IsCloser(Rational x, Rational y, ulong z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_float : IComparer<Rational, float>
    {
        public bool Equals(Rational x, float y) => x == y;
        public bool LessThan(Rational x, float y) => x < y;
        public bool IsCloser(Rational x, Rational y, float z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_double : IComparer<Rational, double>
    {
        public bool Equals(Rational x, double y) => x == y;
        public bool LessThan(Rational x, double y) => x < y;
        public bool IsCloser(Rational x, Rational y, double z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_decimal : IComparer<Rational, decimal>
    {
        public bool Equals(Rational x, decimal y) => x == y;
        public bool LessThan(Rational x, decimal y) => x < y;
        public bool IsCloser(Rational x, Rational y, decimal z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

    public readonly struct Comparer_Rational_Rational : IComparer<Rational, Rational>
    {
        public bool Equals(Rational x, Rational y) => x == y;
        public bool LessThan(Rational x, Rational y) => x < y;
        public bool IsCloser(Rational x, Rational y, Rational z) { var zz = (double)z; return (double)y - zz + (double)x - zz is > 0; }
    }

}
