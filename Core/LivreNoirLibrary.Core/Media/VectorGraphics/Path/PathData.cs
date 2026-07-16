using LivreNoirLibrary.Collections;
using System;
using System.Globalization;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public readonly struct PathData
    {
        public readonly PathCommand Command;
        public readonly bool Relative;
        public readonly bool LargeArc;
        public readonly bool Sweep;
        public readonly float Arg1;
        public readonly float Arg2;
        public readonly float Arg3;
        public readonly float Arg4;
        public readonly float Arg5;
        public readonly float Arg6;

        internal PathData(PathCommand command, bool relative, ReadOnlySpan<float> args, bool largeArc = false, bool sweep = false)
        {
            Command = command;
            Relative = relative;
            Arg1 = args[0];
            Arg2 = args[1];
            Arg3 = args[2];
            Arg4 = args[3];
            Arg5 = args[4];
            Arg6 = args[5];
            LargeArc = largeArc;
            Sweep = sweep;
        }

        public PathData Multiply(float factor) => new(Command, Relative, [Arg1 * factor, Arg2 * factor, Arg3 * factor, Arg4 * factor, Arg5 * factor, Arg6 * factor], LargeArc, Sweep);

        public override string ToString() => Command switch
        {
            PathCommand.Moveto => $"{Head('M')}{V(Arg1)},{V(Arg2)}",
            PathCommand.Lineto => $"{Head('L')}{V(Arg1)},{V(Arg2)}",
            PathCommand.HorizontalLineto => $"{Head('H')}{V(Arg1)}",
            PathCommand.VerticalLineto => $"{Head('V')}{V(Arg1)}",
            PathCommand.CurveTo => $"{Head('C')}{V(Arg1)},{V(Arg2)} {V(Arg3)},{V(Arg4)} {V(Arg5)},{V(Arg6)}",
            PathCommand.SmoothCurveto => $"{Head('S')}{V(Arg1)},{V(Arg2)} {V(Arg3)},{V(Arg4)}",
            PathCommand.QuadraticBezier => $"{Head('Q')}{V(Arg1)},{V(Arg2)} {V(Arg3)},{V(Arg4)}",
            PathCommand.SmoothQuadratic => $"{Head('T')}{V(Arg1)},{V(Arg2)}",
            PathCommand.EllipticalArc =>  $"{Head('A')}{V(Arg1)},{V(Arg2)} {V(Arg3)} {F1}{F2} {V(Arg4)}{V(Arg5)}",
            PathCommand.Closepath => $"{Head('Z')}",
            _ => "**Invalid Command**",
        };

        public const float LowerBound =         0.001f;
        public const float UpperBound = 1_000_000;

        private static string V(float value)
        {
            var format = (Math.Abs(value) is >= LowerBound and < UpperBound) ? "0.###" : "e4";
            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        private char Head(char c) => Relative ? c : char.ToLowerInvariant(c);
        private char F1 => LargeArc ? '0' : '1';
        private char F2 => Sweep ? '0' : '1';
    }
}
