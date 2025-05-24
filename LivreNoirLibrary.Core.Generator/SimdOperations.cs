using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace LivreNoirLibrary.Core;

using static Utils;

[Generator]
internal class SimdOperations : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(Generate);
    }

    private void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        // 算術
        var types = Unmanaged;
        Span<string> methods = ["CopyFrom", "Add", "Subtract", "Multiply", "Divide", "Min", "Max"];
        GenerateCore(context, Code_Vector_Overload, Code_Vector_Ptr, methods, types);
        GenerateCore(context, Code_Scalar_Overload, Code_Scalar_Ptr, methods, types, suffix: "S");
        GenerateCore(context, Code_2Scalar_Overload, Code_2Scalar_Ptr, ["Clamp"], types);
        GenerateCore(context, Code_Unary_Overload, Code_Unary_Ptr, ["Clear"], types);
        GenerateCore(context, Code_Unary_Overload, Code_Unary_Ptr, ["Abs", "Negate"], Signed);
        GenerateCore_Equals(context, types);
        // 総計
        types = [Int, UInt, Long, ULong, Float, Double];
        GenerateCore_Readonly(context, ["Sum", "Average", "Square", "MeanSquare"], types, PH_Type);
        GenerateCore_Readonly(context, ["Min", "Max"], types, PH_Type, "U");
        GenerateCore_Readonly(context, ["MinMax"], types, $"({PH_Type} Min, {PH_Type} Max)");
        // ビット演算
        methods = ["And", "Or", "Xor", "Nand", "Nor", "Xnor"];
        types = BinaryInteger;
        GenerateCore(context, Code_Vector_Overload, Code_Vector_Ptr, methods, types);
        GenerateCore(context, Code_Scalar_Overload, Code_Scalar_Ptr, methods, types, suffix: "S");
        GenerateCore(context, Code_Unary_Overload, Code_Unary_Ptr, ["Not"], types);
    }

    private static void GenerateCore(IncrementalGeneratorPostInitializationContext context, string overloadCode, string pointerCode, Span<string> methods, Span<string> types, string suffix = "")
    {
        foreach (var method in methods)
        {
            StringBuilder sb = new();
            sb.AppendLine(Code_Header);

            var text = overloadCode.Replace(PH_Method, method);
            var twoVector = text.Contains(PH_Source);

            // 配列タイプごとのオーバーロード
            foreach (var (name, conv) in SpanTypes_Destination)
            {
                var destFixed = text.Replace(PH_DestinationConvert, conv)
                                    .Replace(PH_Destination, name);
                if (twoVector)
                {
                    foreach (var (name2, conv2) in SpanTypes_ReadOnly)
                    {
                        var srcFixed = destFixed.Replace(PH_SourceConvert, conv2)
                                                .Replace(PH_Source, name2);
                        foreach (var type in types)
                        {
                            sb.AppendLine(srcFixed.Replace(PH_Type, type));
                        }
                    }
                }
                else
                {
                    foreach (var type in types)
                    {
                        sb.AppendLine(destFixed.Replace(PH_Type, type));
                    }
                }
            }

            // ポインタのオーバーロード
            text = pointerCode.Replace(PH_Method, method);
            foreach (var type in types)
            {
                sb.AppendLine(text.Replace(PH_Type, type));
            }

            sb.AppendLine(Code_Footer);
            context.AddSource($"{method}{suffix}.g.cs", sb.ToString());
        }
    }

    private static void GenerateCore_Readonly(IncrementalGeneratorPostInitializationContext context, Span<string> methods, Span<string> types, string returnType, string suffix = "")
    {
        foreach (var method in methods)
        {
            StringBuilder sb = new();
            sb.AppendLine(Code_Header);

            var text = Code_Unary_Readonly_Overload.Replace(PH_Return, returnType)
                                                   .Replace(PH_Method, method);

            // 配列タイプごとのオーバーロード
            foreach (var (name, conv) in SpanTypes_ReadOnly)
            {
                var destFixed = text.Replace(PH_SourceConvert, conv)
                                    .Replace(PH_Source, name);
                foreach (var type in types)
                {
                    sb.AppendLine(destFixed.Replace(PH_Type, type));
                }
            }

            // ポインタのオーバーロード
            text = Code_Unary_Readonly_Ptr.Replace(PH_Return, returnType)
                                          .Replace(PH_Method, method);
            foreach (var type in types)
            {
                sb.AppendLine(text.Replace(PH_Type, type));
            }

            sb.AppendLine(Code_Footer);
            context.AddSource($"{method}{suffix}.g.cs", sb.ToString());
        }
    }

    private static void GenerateCore_Equals(IncrementalGeneratorPostInitializationContext context, Span<string> types)
    {
        StringBuilder sb = new();
        sb.AppendLine(Code_Header);

        var text = Code_EqualsAll_Overload;

        // 配列タイプごとのオーバーロード
        foreach (var (name, length, conv) in SpanTypes_Equals)
        {
            var destFixed = text.Replace(PH_DestinationConvert, conv.Replace(PH_Target, "left"))
                                .Replace(PH_DestinationLength, length)
                                .Replace(PH_Destination, name);
            foreach (var (name2, length2, conv2) in SpanTypes_Equals)
            {
                var srcFixed = destFixed.Replace(PH_SourceConvert, conv2.Replace(PH_Target, "right"))
                                        .Replace(PH_SourceLength, length2)
                                        .Replace(PH_Source, name2);
                foreach (var type in types)
                {
                    sb.AppendLine(srcFixed.Replace(PH_Type, type));
                }
            }
        }

        // ポインタのオーバーロード
        text = Code_EqualsAll_Ptr;
        foreach (var type in types)
        {
            sb.AppendLine(text.Replace(PH_Type, type));
        }

        sb.AppendLine(Code_Footer);
        context.AddSource($"EqualsAll.g.cs", sb.ToString());
    }

    private static readonly (string, string)[] SpanTypes_Destination =
    [
        ($"List<{PH_Type}>", "CollectionsMarshal.AsSpan(destination)"),
        ($"{PH_Type}[]", "destination"),
        ($"Memory<{PH_Type}>", "destination.Span"),
        ($"Span<{PH_Type}>", "destination"),
    ];

    private static readonly (string, string)[] SpanTypes_ReadOnly =
    [
        ($"List<{PH_Type}>", "CollectionsMarshal.AsSpan(source)"),
        ($"{PH_Type}[]", "source"),
        ($"Memory<{PH_Type}>", "source.Span"),
        ($"Span<{PH_Type}>", "source"),
        ($"ReadOnlyMemory<{PH_Type}>", "source.Span"),
        ($"ReadOnlySpan<{PH_Type}>", "source"),
    ];

    private static readonly (string, string, string)[] SpanTypes_Equals =
    [
        ($"List<{PH_Type}>", "Count", $"CollectionsMarshal.AsSpan({PH_Target})"),
        ($"{PH_Type}[]", "Length", $"{PH_Target}"),
        ($"Memory<{PH_Type}>", "Length", $"{PH_Target}.Span"),
        ($"Span<{PH_Type}>", "Length", $"{PH_Target}"),
        ($"ReadOnlyMemory<{PH_Type}>", "Length", $"{PH_Target}.Span"),
        ($"ReadOnlySpan<{PH_Type}>", "Length", $"{PH_Target}"),
    ];

    const string Code_Header = """
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public static unsafe partial class SimdOperations
    {
""";

    const string Code_Footer = """
    }
}
""";

    const string Code_Vector_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Source}} source)
        {
            var dst = {{PH_DestinationConvert}};
            var src = {{PH_SourceConvert}};
            var length = Math.Min(dst.Length, src.Length);
            fixed ({{PH_Type}}* dstPtr = dst)
            fixed ({{PH_Type}}* srcPtr = src)
            {
                {{PH_Method}}Core(dstPtr, srcPtr, length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Source}} source, int dstOffset, int srcOffset, int length)
        {
            var dst = {{PH_DestinationConvert}};
            var src = {{PH_SourceConvert}};
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed ({{PH_Type}}* dstPtr = dst)
            fixed ({{PH_Type}}* srcPtr = src)
            {
                {{PH_Method}}Core(dstPtr + dstOffset, srcPtr + srcOffset, length);
            }
        }
""";

    const string Code_Vector_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}({{PH_Type}}* destination, {{PH_Type}}* source, int length) => {{PH_Method}}Core(destination, source, length);
""";

    const string Code_Scalar_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Type}} value)
        {
            var dst = {{PH_DestinationConvert}};
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr, value, dst.Length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Type}} value, int offset, int length)
        {
            var dst = {{PH_DestinationConvert}};
            AdjustArgs(dst.Length, ref offset, ref length);
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr + offset, value, length);
            }
        }
""";

    const string Code_Scalar_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}({{PH_Type}}* destination, {{PH_Type}} value, int length) => {{PH_Method}}Core(destination, value, length);
""";

    const string Code_2Scalar_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Type}} min, {{PH_Type}} max)
        {
            var dst = {{PH_DestinationConvert}};
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr, min, max, dst.Length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, {{PH_Type}} min, {{PH_Type}} max, int offset, int length)
        {
            var dst = {{PH_DestinationConvert}};
            AdjustArgs(dst.Length, ref offset, ref length);
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr + offset, min, max, length);
            }
        }
""";

    const string Code_2Scalar_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}({{PH_Type}}* destination, {{PH_Type}} min, {{PH_Type}} max, int length) => {{PH_Method}}Core(destination, min, max, length);
""";

    const string Code_Unary_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination)
        {
            var dst = {{PH_DestinationConvert}};
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr, dst.Length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}(this {{PH_Destination}} destination, int offset, int length)
        {
            var dst = {{PH_DestinationConvert}};
            AdjustArgs(dst.Length, ref offset, ref length);
            fixed ({{PH_Type}}* dstPtr = dst)
            {
                {{PH_Method}}Core(dstPtr + offset, length);
            }
        }
""";

    const string Code_Unary_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void {{PH_Method}}({{PH_Type}}* destination, int length) => {{PH_Method}}Core(destination, length);
""";

    const string Code_Unary_Readonly_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{PH_Return}} {{PH_Method}}(this {{PH_Source}} source)
        {
            var src = {{PH_SourceConvert}};
            fixed ({{PH_Type}}* srcPtr = src)
            {
                return {{PH_Method}}Core(srcPtr, src.Length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{PH_Return}} {{PH_Method}}(this {{PH_Source}} source, int offset, int length)
        {
            var src = {{PH_SourceConvert}};
            AdjustArgs(src.Length, ref offset, ref length);
            fixed ({{PH_Type}}* srcPtr = src)
            {
                return {{PH_Method}}Core(srcPtr + offset, length);
            }
        }
""";

    const string Code_Unary_Readonly_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{PH_Return}} {{PH_Method}}({{PH_Type}}* source, int length) => {{PH_Method}}Core(source, length);
""";

    const string Code_EqualsAll_Overload = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAll(this {{PH_Destination}} left, {{PH_Source}} right)
        {
            if (left.{{PH_DestinationLength}} != right.{{PH_SourceLength}})
            {
                return false;
            }
            var l = {{PH_DestinationConvert}};
            var r = {{PH_SourceConvert}};
            fixed ({{PH_Type}}* leftPtr = l)
            fixed ({{PH_Type}}* rightPtr = r)
            {
                return EqualsCore(leftPtr, rightPtr, l.Length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(this {{PH_Destination}} left, {{PH_Source}} right)
        {
            var l = {{PH_DestinationConvert}};
            var r = {{PH_SourceConvert}};
            var length = Math.Min(l.Length, r.Length);
            fixed ({{PH_Type}}* leftPtr = l)
            fixed ({{PH_Type}}* rightPtr = r)
            {
                return EqualsCore(leftPtr, rightPtr, length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAll(this {{PH_Destination}} left, {{PH_Source}} right, int leftOffset, int rightOffset, int length)
        {
            var l = {{PH_DestinationConvert}};
            var r = {{PH_SourceConvert}};
            AdjustArgs(l.Length, r.Length, ref leftOffset, ref rightOffset, ref length);
            fixed ({{PH_Type}}* leftPtr = l)
            fixed ({{PH_Type}}* rightPtr = r)
            {
                return EqualsCore(leftPtr, rightPtr, length);
            }
        }
""";

    const string Code_EqualsAll_Ptr = $$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsAll({{PH_Type}}* left, {{PH_Type}}* right, int length) => EqualsCore(left, right, length);
""";
}