using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Analysis
    {
        public static float CalculateLufs(IWaveBuffer waveBuffer) => CalculateLufs(waveBuffer.Data, waveBuffer.SampleRate, waveBuffer.Channels);
        public static unsafe float CalculateLufs(ReadOnlySpan<float> buffer, int sampleRate, int channels)
        {
            var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            var t0_total = t0;
            Console.WriteLine("start calculate LUFS");
            var length = buffer.Length / channels;
            if (length is 0)
            {
                return float.NegativeInfinity;
            }

            // 0. 作業用バッファの作成
            // チャンネルごとに独立して処理
            var dataList = new UnmanagedArray<float>[channels];
            var dataPointers = stackalloc float*[channels];
            for (var c = 0; c < channels; c++)
            {
                UnmanagedArray<float> ary = new(length);
                dataList[c] = ary;
                dataPointers[c] = ary.Pointer;
            }
            fixed (float* ptr = buffer)
            {
                for (var i = 0; i < length; i++)
                {
                    var p = ptr + i * 2;
                    for (var c = 0; c < channels; c++)
                    {
                        dataPointers[c][i] = p[c];
                    }
                }
            }
            Notify("copy buffer");

            // 1. プレフィルタリング(K周波数ウェイト)
            // ステージ1: 頭部の音響効果（剛体球モデル）をシミュレート
            var filter = BiQuadFilter.HighShelf(sampleRate, 1500, BiQuadFilter.InvSqrt2, 4);
            for (var c = 0; c < channels; c++)
            {
                filter.ClearState();
                filter.Apply(dataPointers[c], length);
            }
            Notify("apply High Shelf filter");
            // ステージ2: RLBウェイト
            filter.SetupHighPass(sampleRate, 38, 0.5);
            for (var c = 0; c < channels; c++)
            {
                filter.ClearState();
                filter.Apply(dataPointers[c], length);
            }
            Notify("apply High Pass filter");

            var msList = ObjectPool.Rent<List<float>>();
            var gatedList = ObjectPool.Rent<List<float>>();
            try
            {
                // 2. RMSの計算
                msList.Clear();
                var blockSize = (int)(sampleRate * 0.4);
                var capacity = 1 + length / blockSize;
                msList.EnsureCapacity(capacity);
                var hopSize = blockSize / 4;
                if (length < blockSize)
                {
                    var ms = 0f;
                    for (var c = 0; c < channels; c++)
                    {
                        ms += SimdOperations.MeanSquare(dataPointers[c], length);
                    }
                    if (ms is 0)
                    {
                        return float.NegativeInfinity;
                    }
                    msList.Add(ms);
                }
                else
                {
                    for (var index = 0; index + blockSize < length; index += hopSize)
                    {
                        var ms = 0f;
                        for (var c = 0; c < channels; c++)
                        {
                            ms += SimdOperations.MeanSquare(dataPointers[c] + index, blockSize);
                        }
                        msList.Add(ms);
                    }
                }
                Notify("calcultate mean squares");
                // データバッファは解放しておく
                for (var c = 0; c < channels; c++)
                {
                    dataList[c].Dispose();
                    dataList[c] = null!;
                }
                dataList = null;
                Notify("dispose buffers");

                // 3. ゲーティング
                // ステップ1: 絶対閾値
                gatedList.Clear();
                gatedList.EnsureCapacity(capacity);
                // LKFS値が -70dB を超えるxのみを考慮する:
                // -0.691 + 10 * log10(x) > -70
                // x > 10 ^ {(-70 + 0.691) / 10} ≒ 1.1724653045822963959543852795004e-7
                const double th1 = 1.1724653045822963959543852795004e-7;
                foreach (var ms in CollectionsMarshal.AsSpan(msList))
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
                // absL = -0.691 + 10 * log10(avgJ)
                // -0.691 + 10 * log10(x) > absL - 10
                // x > avgJ * 0.1
                foreach (var ms in CollectionsMarshal.AsSpan(msList))
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
                ObjectPool.Return(msList);
                ObjectPool.Return(gatedList);
                t0 = t0_total;
                Notify("total time:");
            }

            void Notify(string content)
            {
                var t = System.Diagnostics.Stopwatch.GetTimestamp();
                Console.WriteLine($"{content} in {(t - t0) / TimeSpan.TicksPerMicrosecond * 0.001:0.###}ms");
                t0 = t;
            }
        }
    }
}
