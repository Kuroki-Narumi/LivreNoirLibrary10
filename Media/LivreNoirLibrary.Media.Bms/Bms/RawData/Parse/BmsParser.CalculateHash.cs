using LivreNoirLibrary.Collections;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser
    {
        public static SearchValues<char> NewLineChars { get; } = SearchValues.Create("\r\n\f\u0085\u2028\u2029");
        public static SearchValues<char> SeparatorChars { get; } = SearchValues.Create("\t :　：");
        private static readonly byte[] _hashSeparator = "\n"u8.ToArray();

        /// <summary>
        /// 与えられたパスをBMSファイルとみなして開き、実質的な内容のみを抽出してハッシュ値を計算する。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static byte[] ComputeHashFromFile(string path, IHashFunction function)
        {
            using var stream = File.OpenRead(path);
            return ComputeHash(stream, function);
        }

        /// <summary>
        /// 与えられたストリームをBMSテキストとみなし、実質的な内容のみを抽出してハッシュ値を計算する。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(Stream stream, IHashFunction function)
        {
            var text = ReadRawText(stream);
            return ComputeHash(text, function);
        }

        /// <summary>
        /// 与えられたテキストをBMSとみなし、実質的な内容のみを抽出してハッシュ値を計算する。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string text, IHashFunction function)
        {
            var textSpan = text.AsSpan();
            var isBase36 = true;

            // 1st pass: 行データの抽出
            // 参照範囲が元テキストのどの位置から始まるか
            var nextIndex = 0;
            // 未処理のテキスト
            var remaining = textSpan;
            // セグメントのリスト
            List<HashSegment> segments = [];
            while (remaining.Length is > 0)
            {
                var startIndex = nextIndex;
                // 1行抽出
                var idx = remaining.IndexOfAny(NewLineChars);
                ReadOnlySpan<char> line;
                if ((uint)idx < (uint)remaining.Length)
                {
                    var stride = 1;
                    // CRCFへの対応
                    if (remaining[idx] == '\r' && (uint)(idx + 1) < (uint)remaining.Length && remaining[idx + 1] == '\n')
                    {
                        stride = 2;
                    }
                    nextIndex += idx + stride;
                    line = remaining[..idx];
                    remaining = remaining[(idx + stride)..];
                }
                else
                {
                    nextIndex += remaining.Length;
                    line = remaining;
                    remaining = default;
                }
                // 先頭の空白を削除
                while (line.Length > 0 && char.IsWhiteSpace(line[0]))
                {
                    startIndex++;
                    line = line[1..];
                }
                // 末尾の空白を削除
                while (line.Length > 0 && char.IsWhiteSpace(line[^1]))
                {
                    line = line[..^1];
                }
                // コメント行や空行は無視
                if (line.Length < 2 || line[0] is not '#')
                {
                    continue;
                }
                // 基数の更新
                if (TryGetBase(line, out var value))
                {
                    isBase36 = value <= 36;
                }
                // シーケンス行？
                var isSequence = line.Length > 1 && char.IsNumber(line[1]);
                // セパレータ(最初の空白またはコロン)の位置
                idx = line.IndexOfAny(SeparatorChars);
                if (idx < 0)
                {
                    idx = line.Length;
                }
                segments.Add(new(startIndex, line.Length, idx, isSequence));
            }

            // 2nd pass: case(大文字小文字)を調節してハッシュ用のバッファに突っ込む
            // case変更用のバッファ
            UnmanagedArray<char> charBuffer = new();
            // UTF8変換用のバッファ
            UnmanagedArray<byte> encodeBuffer = new();
            var encoding = Encoding.UTF8;
            // 最終的なデータのリスト
            List<byte[]> hashList = [];
            // データの最大サイズ
            var maxDataLength = 0;

            void ToLower(ReadOnlySpan<char> line, int separatorIndex)
            {
                line[..separatorIndex].ToLowerInvariant(charBuffer.AsSpan());
                charBuffer.CopyFrom(line[separatorIndex..], separatorIndex);
            }

            foreach (var segment in segments.AsSpan())
            {
                var line = textSpan.Slice(segment.StartIndex, segment.Length);
                charBuffer.EnsureSize(line.Length);
                var separatorIndex = segment.SeparatorIndex;
                if (isBase36)
                {
                    // シーケンスコマンドは全て小文字化
                    if (segment.IsSequence)
                    {
                        line.ToLowerInvariant(charBuffer.AsSpan());
                    }
                    // それ以外はセパレータまで小文字化、以降はそのまま
                    else
                    {
                        ToLower(line, separatorIndex);
                    }
                }
                else
                {
                    // シーケンスコマンドは全てそのまま
                    if (segment.IsSequence)
                    {
                        charBuffer.CopyFrom(line);
                    }
                    // それ以外はセパレータまで小文字化、以降はそのまま
                    else
                    {
                        // 定義コマンドは定義番号の開始位置をセパレータ位置として扱う
                        if (TryGetDef(line, out _, out var defSeparatorIndex))
                        {
                            separatorIndex = defSeparatorIndex;
                        }
                        ToLower(line, separatorIndex);
                    }
                }
                // utf8に変換
                var chars = charBuffer.Slice(0, line.Length);
                encodeBuffer.EnsureSize(encoding.GetMaxByteCount(chars.Length));
                var bytesWritten = encoding.GetBytes(chars, encodeBuffer.AsSpan());
                var data = encodeBuffer.Slice(0, bytesWritten).ToArray();
                hashList.Add(data);
                maxDataLength = Math.Max(maxDataLength, bytesWritten);
            }

            // 3rd pass: 辞書順でソート
            hashList.Sort(HashLineComparison);

            // 4th pass: ハッシュ値計算
            function.Initialize();
            var hashSpan = hashList.AsSpan();
            var count = hashSpan.Length - 1;
            var separator = _hashSeparator;
            for (var i = 0; i < count; i++)
            {
                function.Update(hashSpan[i]);
                function.Update(separator);
            }
            if (hashSpan.Length > 0)
            {
                function.UpdateFinal(hashSpan[^1]);
            }
            else
            {
                function.UpdateFinal([]);
            }
            return function.Hash;
        }

        private static bool TryGetDef(ReadOnlySpan<char> line, out DefType type, out int separatorIndex)
        {
            foreach (var (expr, defType) in DefTags)
            {
                var defLen = expr.Length;
                // defLen + 2 = 定義プレフィクス + インデックス2桁
                if (line.Length >= defLen + 2 && IsMatch(line, expr))
                {
                    type = defType;
                    separatorIndex = defLen;
                    return true;
                }
            }
            type = default;
            separatorIndex = default;
            return false;
        }

        private static int HashLineComparison(byte[] a, byte[] b) => a.SequenceCompareTo(b);

        private readonly record struct HashSegment(int StartIndex, int Length, int SeparatorIndex, bool IsSequence);
    }
}
