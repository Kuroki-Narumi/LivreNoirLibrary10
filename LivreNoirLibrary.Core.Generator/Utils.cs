using System;

namespace LivreNoirLibrary.Core
{
    internal static partial class Utils
    {
        public const string Byte = "byte";
        public const string SByte = "sbyte";
        public const string Short = "short";
        public const string UShort = "ushort";
        public const string Int = "int";
        public const string UInt = "uint";
        public const string IntPtr = "nint";
        public const string UIntPtr = "nuint";
        public const string Long = "long";
        public const string ULong = "ulong";
        public const string Float = "float";
        public const string Double = "double";
        public const string Decimal = "decimal";
        public const string Rational = "Rational";
        public const string Char = "char";
        public const string Int128 = "Int128";
        public const string UInt128 = "UInt128";

        public static readonly string[] Unmanaged = [Byte, SByte, Short, UShort, Int, UInt, IntPtr, UIntPtr, Long, ULong, Float, Double];
        public static readonly string[] BinaryInteger = [Byte, SByte, Short, UShort, Int, UInt, IntPtr, UIntPtr, Long, ULong];
        public static readonly string[] Signed = [SByte, Short, Int, IntPtr, Long, Float, Double];
        public static readonly string[] Integer = [Int, UInt, Long, ULong, Int128, UInt128];

        public const string PH_Return = "#RETURN#";
        public const string PH_Method = "#METHOD#";
        public const string PH_Type = "#TYPE#";
        public const string PH_Destination = "#D-TYPE#";
        public const string PH_Source = "#S-TYPE#";
        public const string PH_DestinationConvert = "#D-CONVERT#";
        public const string PH_SourceConvert = "#S-CONVERT#";
    }
}
