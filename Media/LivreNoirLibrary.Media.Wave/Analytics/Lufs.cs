using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Analysis
    {
        private static readonly float[] _ms_factors = [1, 1, 1, 1.41421356f, 1.41421356f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

        public static float CalculateLufs(IWaveBuffer waveBuffer) => CalculateLufs(waveBuffer.Data, waveBuffer.SampleRate, waveBuffer.Channels);
        public static float CalculateLufs(ReadOnlySpan<float> buffer, int sampleRate, int channels)
        {
            var sampleLength = buffer.Length / channels;
            if (sampleLength is 0)
            {
                return float.NegativeInfinity;
            }
            Console.WriteLine("start calculate LUFS");

            var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            var t0_total = t0;

            void Notify(string content)
            {
                var t = System.Diagnostics.Stopwatch.GetTimestamp();
                Console.WriteLine($"{content} in {(t - t0) / TimeSpan.TicksPerMicrosecond * 0.001:0.###}ms");
                t0 = t;
            }

            // インターリーブ配列(ch0,ch1,ch0,ch1...)をチャンネルごとに分割するためのバッファ
            var splitBuffer = new UnmanagedArray<float>(buffer.Length);
            // 処理単位(0.4秒ごと)
            var blockSize = (int)(sampleRate * 0.4);
            var capacity = 1 + sampleLength / blockSize;
            using var o1 = ArrayPool.Rent<float>(capacity);
            using var o2 = ObjectPool.Rent<List<float>>();
            // RMSを一時保存するためのリスト
            var msList = o1.Span;
            // 
            var msFactor = _ms_factors.AsSpan();
            var gatedList = o2.Value;
            try
            {
                // 1. フィルタ
                // ステージ1: 頭部の音響効果（剛体球モデル）をシミュレート
                var filter = BiQuadFilter.HighShelf(sampleRate, 1500, gain: 4);
                buffer.Transpose(splitBuffer.AsSpan(), channels);
                for (var c = 0; c < channels; c++)
                {
                    filter.Apply(splitBuffer.Slice(c * sampleLength, sampleLength));
                }
                // ステージ2: RLBウェイト
                filter = BiQuadFilter.HighPass(sampleRate, 38, 0.5);
                for (var c = 0; c < channels; c++)
                {
                    filter.Apply(splitBuffer.Slice(c * sampleLength, sampleLength));
                }
                // 2. RMSの計算
                if (sampleLength < blockSize)
                {
                    var ms = 0f;
                    for (var c = 0; c < channels; c++)
                    {
                        ms += SimdOperations.MeanSquare(splitBuffer.Slice(c * sampleLength, sampleLength)) * msFactor[c];
                    }
                    if (ms is 0)
                    {
                        return float.NegativeInfinity;
                    }
                    msList[0] = ms;
                }
                else
                {
                    var hopSize = blockSize / 4;
                    for (var c = 0; c < channels; c++)
                    {
                        var factor = msFactor[c];
                        var slice = splitBuffer.Slice(c * sampleLength, sampleLength);
                        var msIndex = 0;
                        for (var index = 0; index + blockSize < sampleLength; index += hopSize, msIndex++)
                        {
                            msFactor[msIndex] += SimdOperations.MeanSquare(slice.Slice(index, blockSize)) * factor;
                        }
                    }
                }
                Notify("calcultate mean squares");

                // 3. ゲーティング
                // ステップ1: 絶対閾値
                gatedList.EnsureCapacity(capacity);
                // LKFS値が -70dB を超えるxのみを考慮する:
                // -0.691 + 10 * log10(x) > -70 ;
                // x > 10 ^ {(-70 + 0.691) / 10} ≒ 1.1724653045822963959543852795004e-7
                const double th1 = 1.1724653045822963959543852795004e-7;
                foreach (var ms in msList)
                {
                    if (ms > th1)
                    {
                        gatedList.Add(ms);
                    }
                }
                if (gatedList.Count is 0)
                {
                    return float.NegativeInfinity;
                }
                Notify("absolute gate");

                // ステップ2: 相対閾値
                var th2 = gatedList.Average() * 0.1;
                gatedList.Clear();
                // LKFS値が (absL - 10)dB を超えるxのみを考慮する:
                // absL = -0.691 + 10 * log10(avgJ) ;
                // -0.691 + 10 * log10(x) > absL - 10 ;
                // x > avgJ * 0.1
                foreach (var ms in msList)
                {
                    if (ms > th2)
                    {
                        gatedList.Add(ms);
                    }
                }
                if (gatedList.Count is 0)
                {
                    return float.NegativeInfinity;
                }
                Notify("relative gate");

                return -0.691f + 10 * MathF.Log10(gatedList.Average());
            }
            finally
            {
                splitBuffer.Free();
                t0 = t0_total;
                Notify("total time:");
            }
        }
    }
}