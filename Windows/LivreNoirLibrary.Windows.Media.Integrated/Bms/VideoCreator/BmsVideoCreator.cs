using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Controls.Bms.Elements;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BmsVideoCreator(BmsScreen screen, IBmsVideoCreatorOptions options) : ObservableObjectBase
    {
        public BmsScreen Screen { get; } = screen;
        public IBmsVideoCreatorOptions Options { get; } = options;

        private double InitializeAudio(int sampleRate, int channels, double delay, ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c)
        {
            var screen = Screen;
            // 演奏時間の算出
            var composer = screen.AudioComposer;
            composer.Setup(sampleRate, channels, delay);
            var provider = composer.Provider;
            var musicLength = 0d;
            var i = 0;
            var count = composer.Timeline.AudioItemCount;

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
                    var f = new AntiFreezeUpdater();
                    const int rate = 48000;
                    const int ch = 2;

                    screen.SetupAudio(true);

                    var composer = screen.AudioComposer;
                    var offset = screen.FirstSoundTime;
                    var duration = InitializeAudio(rate, ch, -offset, ref f, p, c) - offset;

                    var bufferSize = rate * ch;
                    var audioBuffer = ArrayPool<float>.Shared.Rent(bufferSize);
                    using WaveEncoder encoder = new(path, new(rate, ch, SampleFormat.Int16));
                    var totalSample = (int)Math.Ceiling(duration * rate) * ch;

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
                var f = new AntiFreezeUpdater();
                const int rate = 48000;
                const int ch = 2;

                screen.DetermineExpressions();
                screen.SetupPlay(true);
                var options = Options;

                // タイマー
                var fadeInDuration = skin.FadeInTime.Validate(0);
                var loadingFinish = skin.LoadTime.Validate(0);
                var musicStart = loadingFinish + skin.ReadyTime.Validate(0);
                var musicLength = InitializeAudio(rate, ch, musicStart, ref f, p, c);

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
                var fps = options.FrameRate;
                var (fps_num, fps_den) = fps;
                var kbps = options.ApproximateKbps;
                Mpeg4EncodeOptions codecOptions = options.IsHevc ? new HevcEncodeOptions() : new H264EncodeOptions();
                if (!codecOptions.EnsureLevel(width, height, (double)fps, kbps))
                {
                    ExConsole.Write($"!ERROR: video size is too large ({width}x{height} {(double)fps}fps {kbps}kbps)");
                    return;
                }
                using FFmpegEncoder encoder = new(path);
                IHardwareEncodeOptions? hardwareOptions = new NvencEncodeOptions() { QP = options.QP };
                VideoEncodeOptions videoOptions = new(width, height, fps, kbps * 1000, codecOptions, hardwareOptions);
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
                    var totalFrameUnit = (long)Math.Ceiling(totalTime * fps_num) + fps_den;
                    for (var frame = 0L; frame < totalFrameUnit; )
                    {
                        var frameMax = Math.Min(totalFrameUnit, frame + fps_num);
                        var frameUnit = frameMax - frame;
                        // 映像
                        for (; frame < frameMax; frame += fps_den)
                        {
                            var time = (double)frame / fps_num;
                            // フェード処理
                            screen.FadeOpacity = 
                                time <= fadeInDuration ? 1 - time / fadeInDuration
                                : time >= fadeOutStart ? (time - fadeOutStart) / fadeOutDuration 
                                : 0;
                            // ロード画面を消す処理
                            if (needLoadingFinish && time >= loadingFinish)
                            {
                                timer.Remove(TimerId.Play_LoadingStart);
                                timer.Set(TimerId.Play_LoadingFinish, time);
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
