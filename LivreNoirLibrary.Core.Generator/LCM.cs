using Microsoft.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Core;
using static Utils;

[Generator]
internal class LCM : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(Generate);
    }

    private void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        StringBuilder sb = new();
        sb.AppendLine("""
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
""");

        foreach (var type in Integer)
        {
            sb.AppendLine($$"""
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{type}} LCM(this {{type}} val1, {{type}} val2)
        {
            var gcd = GCD(val1, val2);
            return (val1 / gcd) * (val2 / gcd) * gcd;
        }
""");
        }

        sb.AppendLine("""
    }
}
""");
        context.AddSource("NumberExtensions.g.cs", sb.ToString());
    }
}