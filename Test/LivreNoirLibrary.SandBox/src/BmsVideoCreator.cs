using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Integrated;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;
using System;
using System.Buffers;
using System.Diagnostics;

namespace LivreNoirLibrary.SandBox
{
    public class BmsVideoCreator(BmsScreen screen, BmsVideoCreateOptions options) : ObservableObjectBase
    {
        public const double QpMinimum = HardwareOptionsBase.QP_Min;
        public const double QpMaximum = HardwareOptionsBase.QP_Max;

        public BmsScreen Screen { get; } = screen;
        public BmsVideoCreateOptions Options { get; } = options;

        private double InitializeAudio(int sampleRate, int channels, double delay, ProgressReporter? p, CancellationToken c)
        {
            var screen = Screen;
            // 演奏時間の算出
            var composer = screen.AudioComposer;
            composer.Setup(sampleRate, channels, delay);
            var provider = composer.Provider;
            var musicLength = 0d;
            var i = 0;
            var count = composer.Timeline.AudioItemCount;

            var isSynchronized = p is not null && p.IsSynchronized;
            p?.Report("Initializing Audio");
            foreach (var (wp, time, length, _) in composer.Timeline.Range(0, double.PositiveInfinity))
            {
                if (provider.TryGetWaveBuffer(wp, out var buffer))
                {
                    musicLength = Math.Max(musicLength, time + buffer.TotalSeconds);
                    if (isSynchronized)
                    {
                        // 画面更新を待つ(フリーズ対策)
                        DependencyObjectExtensions.WaitForUpdate();
                    }
                }
                i++;
                p?.Report($"Initializing Audio({i}/{count})", i, count);
                c.ThrowIfCancellationRequested();
            }
            return musicLength;
        }

        public void Assemble(string path, ProgressReporter? p = null, CancellationToken c = default)
        {
            var screen = Screen;
            if (screen.IsBmsReady)
            {
                try
                {
                    const int rate = 48000;
                    var isSynchronized = p is not null && p.IsSynchronized;

                    screen.SetupAudio(true);

                    var composer = screen.AudioComposer;
                    var offset = screen.FirstSoundTime;
                    var duration = InitializeAudio(rate, 2, -offset, p, c) - offset;

                    var bufferSize = rate * 2;
                    var audioBuffer = ArrayPool<float>.Shared.Rent(bufferSize);
                    using WaveEncoder encoder = new(path, new(rate, 2, SampleFormat.Int16));
                    var totalSample = (int)Math.Ceiling(duration * rate) * 2;

                    for (var time = 0; totalSample is > 0; time += 1, totalSample -= bufferSize)
                    {
                        var span = audioBuffer.AsSpan(0, Math.Min(bufferSize, totalSample));
                        // 音声バッファを取得
                        composer.Read(span);
                        span.Clamp(-1, 1);
                        // 音声フレームの書き込み
                        encoder.Write(span);

                        var report = $"Assembling {time}/{duration:F2}";
                        p?.Report(report, time, duration);
                        if (isSynchronized)
                        {
                            // 画面更新を待つ(フリーズ対策)
                            DependencyObjectExtensions.WaitForUpdate();
                        }
                        c.ThrowIfCancellationRequested();
                    }
                }
                catch(Exception e)
                {
                    ExConsole.Write(e);
                }
            }
        }

        public void CreateVideo(string path, ProgressReporter? p = null, CancellationToken c = default)
        {
            var screen = Screen;
            if (screen.Skin is { } skin && screen.IsBmsReady)
            {
                var isSynchronized = p is not null && p.IsSynchronized;

                const int rate = 48000;
                screen.DetermineExpressions();
                screen.SetupPlay(true);
                var options = Options;

                // タイマー
                var fadeInDuration = options.FadeInDuration;
                var loadingFinish = options.LoadDuration;
                var musicStart = loadingFinish + options.ReadyDuration;
                var musicLength = InitializeAudio(rate, 2, musicStart, p, c);
                // タイマー
                var totalTime = musicStart + Math.Max(musicLength, screen.LastSoundTime + options.AfterMargin);
                var fadeOutDuration = options.FadeOutDuration;
                var fadeOutStart = totalTime - fadeOutDuration;
                var needLoadingFinish = true;
                var timer = screen.Timer;
                timer.Set(TimerId.Scene_Start, 0);
                timer.Set(TimerId.Play_LoadingStart, 0);
                timer.Set(TimerId.Play_MusicStart, musicStart);
                timer.Set(TimerId.Scene_Terminate, fadeOutStart);

                // 音声
                var audioBuffer = ArrayPool<float>.Shared.Rent(rate * 2);
                var composer = screen.AudioComposer;

                var (width, height) = skin.BaseSize;
                // エンコーダー
                using FFmpegEncoder encoder = new(path);
                var fps = options.FrameRate;
                var fps_inv = 1d / fps;
                var (fps_num, fps_den) = fps;
                ICodecOptions codecOptions = options.IsHevc ? new HevcEncodeOptions() : new H264EncodeOptions();
                IHardwareEncodeOptions? hardwareOptions = new NvencEncodeOptions() { QP = options.QP };
                VideoEncodeOptions videoOptions = new(width, height, fps, options.ApproximateKbps * 1000, codecOptions, hardwareOptions);
                encoder.CreateVideoStream(videoOptions);
                encoder.CreateAudioStream(new AudioEncodeOptions(rate, 2));
                // 映像バッファ
                var size = width * height * 4;
                var videoBuffer = ArrayPool<byte>.Shared.Rent(size);
                var videoSpan = videoBuffer.AsSpan(0, size);

                // デバッグ用
                var t0 = Stopwatch.GetTimestamp();
                var totalFrameUnit = (long)Math.Ceiling(totalTime * fps_num);

                p?.Report("Encoding...", null);
                try
                {
                    for (var frame = 0L; frame < totalFrameUnit; )
                    {
                        var frameMax = Math.Min(totalFrameUnit, frame + fps_num);
                        var frameUnit = frameMax - frame;
                        // 映像
                        for (; frame < frameMax; frame += fps_den)
                        {
                            var time = (double)frame / fps_num;

                            screen.FadeOpacity = 
                                time <= fadeInDuration ? 1 - time / fadeInDuration
                                : time >= fadeOutStart ? (time - fadeOutStart - fps_inv) / fadeOutDuration 
                                : 0;

                            // ロード画面を消す処理
                            if (needLoadingFinish && time >= loadingFinish)
                            {
                                screen.FinishLoading(loadingFinish);
                                needLoadingFinish = false;
                            }

                            var report = $"Write frame {time:F2}/{totalTime:F2}({frame / Stopwatch.GetElapsedTime(t0).TotalSeconds / fps_den:0.000}fps)";
                            p?.Report(report, time, totalTime);

                            // 映像バッファの更新
                            screen.Update(time);
                            screen.CopyPixels(videoSpan, width);
                            // 映像フレームの書き込み
                            encoder.WritePixels(videoSpan);

                            c.ThrowIfCancellationRequested();

                            if (isSynchronized)
                            {
                                // 画面更新を待つ(フリーズ対策)
                                DependencyObjectExtensions.WaitForUpdate();
                            }
                        }

                        // 音声は1秒ごとに書き込む
                        var totalSample = (int)(frameUnit * rate * 2 / fps_num);
                        var span = audioBuffer.AsSpan(0, totalSample);
                        // 音声バッファを取得
                        composer.Read(span);
                        span.Clamp(-1, 1);
                        // 音声フレームの書き込み
                        encoder.WriteSamples(span);

                        c.ThrowIfCancellationRequested();
                    }

                    p?.Report("Finalizing...");
                }
                finally
                {
                    ArrayPool<float>.Shared.Return(audioBuffer);
                    ArrayPool<byte>.Shared.Return(videoBuffer);
                }
            }
        }
    }
}
