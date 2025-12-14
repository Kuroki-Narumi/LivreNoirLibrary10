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
using LivreNoirLibrary.Windows.Controls.Bms.Elements;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
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

        private struct AntiFreeze(ProgressReporter? p)
        {
            const long UpdateThreshold = TimeSpan.TicksPerSecond / 30;
            private readonly bool _isSynchronized = p is not null && p.IsSynchronized;
            private long _t0 = Stopwatch.GetTimestamp();

            public void WaitForUpdate()
            {
                long t1;
                if (_isSynchronized && (t1 = Stopwatch.GetTimestamp()) - _t0 is >= UpdateThreshold)
                {
                    // 画面更新を待つ(フリーズ対策)
                    DependencyObjectExtensions.WaitForUpdate();
                    _t0 = t1;
                }
            }
        }

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

            AntiFreeze f = new(p);
            p?.Report("Initializing Audio");
            foreach (var (wp, time, length, _) in composer.Timeline.Range(0, double.PositiveInfinity))
            {
                if (provider.TryGetWaveBuffer(wp, out var buffer))
                {
                    musicLength = Math.Max(musicLength, time + buffer.TotalSeconds);
                }
                i++;
                p?.Report($"Initializing Audio({i}/{count})", i, count);
                f.WaitForUpdate();
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
                    const int ch = 2;

                    screen.SetupAudio(true);

                    var composer = screen.AudioComposer;
                    var offset = screen.FirstSoundTime;
                    var duration = InitializeAudio(rate, ch, -offset, p, c) - offset;

                    var bufferSize = rate * ch;
                    var audioBuffer = ArrayPool<float>.Shared.Rent(bufferSize);
                    using WaveEncoder encoder = new(path, new(rate, ch, SampleFormat.Int16));
                    var totalSample = (int)Math.Ceiling(duration * rate) * ch;

                    AntiFreeze f = new(p);
                    for (var time = 0; totalSample is > 0; time += 1, totalSample -= bufferSize)
                    {
                        var span = audioBuffer.AsSpan(0, Math.Min(bufferSize, totalSample));
                        // 音声バッファを取得
                        composer.Read(span);
                        span.Clamp(-1, 1);
                        // 音声フレームの書き込み
                        encoder.Write(span);

                        var report = $"Assembling {time}/{duration:F2}";
                        f.WaitForUpdate();
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
            if (screen.SkinRoot is PlaySkinRoot skin && screen.IsBmsReady)
            {
                const int rate = 48000;
                const int ch = 2;

                screen.DetermineExpressions();
                screen.SetupPlay(true);
                var options = Options;

                // タイマー
                var fadeInDuration = skin.FadeInTime.Validate(0);
                var loadingFinish = skin.LoadTime.Validate(0);
                var musicStart = loadingFinish + skin.ReadyTime.Validate(0);
                var musicLength = InitializeAudio(rate, ch, musicStart, p, c);
                // タイマー
                var totalTime = musicStart + Math.Max(musicLength, screen.LastSoundTime + skin.MarginTime.Validate(0));
                var fadeOutDuration = skin.FadeOutTime.Validate(0);
                var fadeOutStart = totalTime - fadeOutDuration;
                var needLoadingFinish = true;
                var timer = screen.Timer;
                timer.Set(TimerId.Scene_Start, 0);
                timer.Set(TimerId.Play_LoadingStart, 0);
                timer.Set(TimerId.Play_MusicStart, musicStart);
                timer.Set(TimerId.Scene_Terminate, fadeOutStart);

                // 音声
                var audioBuffer = ArrayPool<float>.Shared.Rent(rate * ch);
                var composer = screen.AudioComposer;

                // エンコーダー
                var (width, height) = skin.BaseSize;
                using FFmpegEncoder encoder = new(path);
                var fps = options.FrameRate;
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

                p?.Report("Encoding...", null);
                try
                {
                    // デバッグ用
                    var t0 = Stopwatch.GetTimestamp();
                    var totalFrameUnit = (long)Math.Ceiling(totalTime * fps_num);
                    AntiFreeze f = new(p);
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
                                : time >= fadeOutStart ? (time - fadeOutStart) / fadeOutDuration 
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

                            f.WaitForUpdate();
                            c.ThrowIfCancellationRequested();
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
