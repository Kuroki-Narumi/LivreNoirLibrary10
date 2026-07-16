using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class PathParseException(ReadOnlySpan<char> data, int index) : Exception(BuildMessage(data, index))
    {
        public string Expression { get; } = new(data);
        public int ErrorIndex { get; } = index;

        static string BuildMessage(ReadOnlySpan<char> data, int index) => $"failed to parse: data=\"{data}\" index={index}";
    }

    public static class PathParser
    {
        public static string Join(this IEnumerable<PathData> list) => string.Join(' ', list);

        public static List<PathData> Parse(ReadOnlySpan<char> data)
        {
            List<PathData> result = [];
            if (!TryParse(data, result, out var index))
            {
                throw new PathParseException(data, index);
            }
            return result;
        }

        /// <summary>
        /// Attempts to parse the SVG path expression and store the results in the provided <paramref name="target"/>.
        /// </summary>
        /// <param name="data">A SVG path expression to be parsed.</param>
        /// <param name="target">A list that stores <see cref="PathData"/> that has been successfully parsed.</param>
        /// <param name="errorIndex">The zero-based index of the first invalid expression found in <paramref name="data"/>, or <see langword="-1"/> if parsing completes successfully.</param>
        /// <returns><see langword="true"/> if entire expression was successfully parsed; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse(ReadOnlySpan<char> data, List<PathData> target, out int errorIndex)
        {
            target.Clear();
            var currentCommand = PathCommand.None;
            var isRelative = false;
            var args = (stackalloc float[6]);
            for (errorIndex = 0; errorIndex < data.Length; errorIndex++)
            {
                var c = data[errorIndex];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                switch (c)
                {
                    case 'M' or 'm':
                        currentCommand = PathCommand.Moveto;
                        isRelative = c is 'm';
                        continue;
                    case 'L' or 'l':
                        currentCommand = PathCommand.Lineto;
                        isRelative = c is 'l';
                        continue;
                    case 'H' or 'h':
                        currentCommand = PathCommand.HorizontalLineto;
                        isRelative = c is 'j';
                        continue;
                    case 'V' or 'v':
                        currentCommand = PathCommand.VerticalLineto;
                        isRelative = c is 'v';
                        continue;
                    case 'C' or 'c':
                        currentCommand = PathCommand.CurveTo;
                        isRelative = c is 'c';
                        continue;
                    case 'S' or 's':
                        currentCommand = PathCommand.SmoothCurveto;
                        isRelative = c is 's';
                        continue;
                    case 'Q' or 'q':
                        currentCommand = PathCommand.QuadraticBezier;
                        isRelative = c is 'q';
                        continue;
                    case 'T' or 't':
                        currentCommand = PathCommand.SmoothQuadratic;
                        isRelative = c is 't';
                        continue;
                    case 'A' or 'a':
                        currentCommand = PathCommand.EllipticalArc;
                        isRelative = c is 'a';
                        continue;
                    case 'Z' or 'z':
                        currentCommand = PathCommand.None;
                        isRelative = c is 'z';
                        target.Add(new(PathCommand.Closepath, isRelative, args));
                        continue;
                }
                switch (currentCommand)
                {
                    case PathCommand.HorizontalLineto:
                    case PathCommand.VerticalLineto:
                        if (TryGetArg(data, ref errorIndex, args))
                        {
                            target.Add(new(currentCommand, isRelative, args));
                            continue;
                        }
                        return false;
                    case PathCommand.Moveto:
                    case PathCommand.Lineto:
                    case PathCommand.SmoothQuadratic:
                        if (GetArgs2(data, ref errorIndex, args))
                        {
                            target.Add(new(currentCommand, isRelative, args));
                            if (currentCommand is PathCommand.Moveto)
                            {
                                currentCommand = PathCommand.Lineto;
                            }
                            continue;
                        }
                        return false;
                    case PathCommand.SmoothCurveto:
                    case PathCommand.QuadraticBezier:
                        if (GetArgs4(data, ref errorIndex, args))
                        {
                            target.Add(new(currentCommand, isRelative, args));
                            continue;
                        }
                        return false;
                    case PathCommand.CurveTo:
                        if (GetArgs6(data, ref errorIndex, args))
                        {
                            target.Add(new(currentCommand, isRelative, args));
                            continue;
                        }
                        return false;
                    case PathCommand.EllipticalArc:
                        if (GetArgs7(data, ref errorIndex, args, out var largeArc, out var sweep))
                        {
                            target.Add(new(currentCommand, isRelative, args, largeArc, sweep));
                            continue;
                        }
                        return false;
                }
                return false;
            }
            errorIndex = -1;
            return true;
        }

        static bool TrySkipSpace(ReadOnlySpan<char> data, ref int i)
        {
            while (i < data.Length && char.IsWhiteSpace(data[i]))
            {
                i++;
            }
            return i < data.Length;
        }

        static bool TryGetValue(ReadOnlySpan<char> data, Span<float> span)
        {
            if (float.TryParse(data, CultureInfo.InvariantCulture, out var value))
            {
                span[0] = value;
                return true;
            }
            return false;
        }

        static bool TryGetArg(ReadOnlySpan<char> data, ref int i, Span<float> span)
        {
            var start = i;
            // 符号
            if (data[i] is '+' or '-')
            {
                i++;
            }
            // 整数部分
            while (i < data.Length && char.IsAsciiDigit(data[i]))
            {
                i++;
            }
            // データ末尾に到達した場合
            if (i >= data.Length)
            {
                return TryGetValue(data[start..], span);
            }
            // 小数部分が存在する
            if (data[i] is '.')
            {
                i++;
                while (i < data.Length && char.IsAsciiDigit(data[i]))
                {
                    i++;
                }
                // データ末尾に到達した場合
                if (i >= data.Length)
                {
                    return TryGetValue(data[start..], span);
                }
            }
            // 指数部分が存在する
            if (data[i] is 'E' or 'e')
            {
                i++;
                // 指数の符号
                if (i < data.Length && data[i] is '+' or '-')
                {
                    i++;
                }
                // 指数の値
                while (i < data.Length && char.IsAsciiDigit(data[i]))
                {
                    i++;
                }
            }
            return TryGetValue(data[start..i], span);
        }

        static bool TrySkipSeparator(ReadOnlySpan<char> data, ref int i)
        {
            if (TrySkipSpace(data, ref i) && data[i] is ',')
            {
                i++;
                return TrySkipSpace(data, ref i);
            }
            return i < data.Length;
        }

        static bool GetArgs2(ReadOnlySpan<char> data, ref int i, Span<float> span)
        {
            return TryGetArg(data, ref i, span) && TrySkipSeparator(data, ref i) && TryGetArg(data, ref i, span[1..]);
        }

        static bool GetArgs4(ReadOnlySpan<char> data, ref int i, Span<float> span)
        {
            return GetArgs2(data, ref i, span) && TrySkipSeparator(data, ref i) && GetArgs2(data, ref i, span[2..]);
        }

        static bool GetArgs6(ReadOnlySpan<char> data, ref int i, Span<float> span)
        {
            return GetArgs4(data, ref i, span) && TrySkipSeparator(data, ref i) && GetArgs2(data, ref i, span[4..]);
        }

        static bool GetArgs7(ReadOnlySpan<char> data, ref int i, Span<float> span, out bool largeArc, out bool sweep)
        {
            largeArc = sweep = false;
            // 最初の3つ
            if (GetArgs2(data, ref i, span) && TrySkipSeparator(data, ref i) && 
                TryGetArg(data, ref i, span[2..]) && TrySkipSeparator(data, ref i))
            {
                // フラグ1
                switch (data[i])
                {
                    case '0':
                        break;
                    case '1':
                        largeArc = true;
                        break;
                    default:
                        return false;
                }
                i++;
                if (!TrySkipSeparator(data, ref i))
                {
                    return false;
                }
                // フラグ2
                switch (data[i])
                {
                    case '0':
                        break;
                    case '1':
                        sweep = true;
                        break;
                    default:
                        return false;
                }
                i++;
                // 終点座標
                return TrySkipSeparator(data, ref i) && GetArgs2(data, ref i, span[3..]);
            }
            return false;
        }
    }
}
