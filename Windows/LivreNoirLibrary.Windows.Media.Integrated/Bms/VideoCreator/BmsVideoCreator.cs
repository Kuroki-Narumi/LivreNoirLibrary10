using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
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
using LivreNoirLibrary.Windows.Media;
using NAudio.Wave;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BmsVideoCreator(IBmsScreen screen, IBmsVideoCreatorOptions options) : ObservableObjectBase
    {
        private readonly WaveOutEvent _waveOut = new();

        public IBmsScreen Screen { get; } = screen;
        public IBmsVideoCreatorOptions Options { get; } = options;
        public bool IsPlaying { get; private set => SetValue(ref field, value); }

        private (double Duration, double MaxLength) InitializeAudio(int sampleRate, int channels, double delay, ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c)
        {
            var screen = Screen;
            var composer = screen.AudioComposer;
            composer.Setup(sampleRate, channels, delay);
            var provider = composer.Provider;
            var musicLength = 0d;
            var maxLength = 0d;
            var i = 0;
            var count = composer.Timeline.KeyCount;

            p?.Report("Initializing Audio", null);
            var notIgnore = !composer.IgnoreItemDuration;
            foreach (var list in composer.Timeline)
            {
                if (provider.TryGetWaveBuffer(list.Key, out var buffer))
                {
                    var (time, len, _) = list.LastItem;
                    var total = buffer.TotalSeconds;
                    total = (notIgnore && len is >= 0) ? Math.Min(len, total) : total;
                    maxLength = Math.Max(maxLength, total);
                    musicLength = Math.Max(musicLength, time + total);
                }
                i++;
                p?.Report($"{i}/{count}", i, count);
                f.WaitForUpdate(c);
            }
            return (musicLength, maxLength);
        }

        public void Assemble(string path, ProgressReporter? p = null, CancellationToken c = default)
        {
            var screen = Screen;
            if (screen.IsBmsReady)
            {
                try
                {
                    var f = new AntiFreezeUpdater();
                    var rate = Options.AudioSampleRate;
                    const int ch = 2;

                    screen.SetupAudio();

                    var composer = screen.AudioComposer;
                    var offset = screen.FirstSoundTime;
                    var (duration, _) = InitializeAudio(rate, ch, -offset, ref f, p, c);
                    duration -= offset;

                    var bufferSize = rate * ch;
                    using var o = ArrayPool.Rent<float>(bufferSize);
                    var audioBuffer = o.Array;
                    using WaveEncoder encoder = new(path, new(rate, ch, SampleFormat.Int16));
                    var totalSample = (int)Math.Ceiling(duration * rate) * ch;

                    p?.Report("Assembling", null);
                    for (var time = 0; totalSample is > 0; time += 1, totalSample -= bufferSize)
                    {
                        var span = audioBuffer.AsSpan(0, Math.Min(bufferSize, totalSample));
                        // 音声バッファを取得
                        composer.Read(span);
                        span.Clamp(-1, 1);
                        // 音声フレームの書き込み
                        encoder.Write(span);

                        p?.Report($"{time}/{duration:F2}", time, duration);
                        f.WaitForUpdate(c);
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
            if (screen.SkinRoot is IPlaySkinRoot skin && screen.IsBmsReady)
            {
                var f = new AntiFreezeUpdater();
                const int ch = 2;

                screen.SetupPlay(true);
                var options = Options;
                var rate = options.AudioSampleRate;

                // タイマー
                var offset = options.StartOffset;
                var fadeInDuration = skin.FadeInTime.Validate(0);
                var loadingFinish = skin.LoadTime.Validate(0) - offset;
                var musicStart = loadingFinish + skin.ReadyTime.Validate(0) - offset;
                var (musicLength, _) = InitializeAudio(rate, ch, musicStart + options.AudioDelay, ref f, p, c);

                var totalTime = musicStart + Math.Max(musicLength, screen.LastSoundTime + skin.MarginTime.Validate(0));
                var fadeOutDuration = skin.FadeOutTime.Validate(0);
                var fadeOutStart = totalTime - fadeOutDuration;
                var needLoadingFinish = true;
                var timer = screen.Timer;
                timer.Set(TimerId.Scene_Start, 0);
                timer.Set(TimerId.Play_LoadingStart, -offset);
                timer.Set(TimerId.Play_MusicStart, musicStart);
                timer.Set(TimerId.Scene_Terminate, fadeOutStart);

                // 音声
                var composer = screen.AudioComposer;

                // エンコーダー
                var (width, height) = skin.BaseSize;
                var fps = options.FrameRate;
                var (fps_num, fps_den) = fps;
                var kbps = options.ApproximateKbps;
                Mpeg4EncodeOptions codecOptions = options.IsHevc ? new HevcEncodeOptions() : new H264EncodeOptions();
                IHardwareEncodeOptions? hardwareOptions = new NvencEncodeOptions();
                VideoEncodeOptions videoOptions = new(width, height, fps, kbps * 1000, codecOptions, hardwareOptions);
                f.WaitForUpdate(c);
                try
                {
                    using var test = General.CreateSafe(path);
                }
                catch (Exception e)
                {
                    ExConsole.Write($"ERROR: failed to create file \"{path}\".");
                    ExConsole.Write((e.GetType(), e.Message));
                    return;
                }
                using FFmpegEncoder encoder = new(path);
                try
                {
                    encoder.CreateVideoStream(videoOptions);
                    encoder.CreateAudioStream(new AudioEncodeOptions(rate, 2));
                }
                catch (Exception e)
                {
                    ExConsole.Write($"ERROR: failed to create stream.");
                    ExConsole.Write((e.GetType(), e.Message));
                    return;
                }

                // バッファ
                using var o_a = ArrayPool.Rent<float>(rate * ch);
                using var o_v = ArrayPool.Rent<byte>(width * height * 4);
                var videoSpan = o_v.Span;

                long totalFrameUnit;
                void UpdateTotalFrameCount(double time) => totalFrameUnit = (long)Math.Ceiling(time * fps_num) + fps_den;

                p?.Report("Encoding...", null);
                // デバッグ用
                var t0 = Stopwatch.GetTimestamp();
                UpdateTotalFrameCount(totalTime);
                var aborting = false;
                for (var frame = 0L; frame < totalFrameUnit;)
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

                        // 中止処理
                        if (c.IsCancellationRequested && !aborting && time < fadeOutStart)
                        {
                            p?.Report("Aborting...", null);
                            fadeOutStart = time;
                            UpdateTotalFrameCount(time + fadeOutDuration);
                            frameMax = Math.Min(totalFrameUnit, frame + fps_num);
                            frameUnit = frameMax - frame;
                            aborting = true;
                        }
                    }

                    // 音声は1秒ごとに書き込む
                    var totalSample = (int)(frameUnit * rate * 2 / fps_num);
                    var span = o_a.AsSpan(totalSample);
                    // 音声バッファを取得
                    composer.Read(span);
                    span.Clamp(-1, 1);
                    // 音声フレームの書き込み
                    encoder.WriteSamples(span);
                }

                p?.Report("Finalizing...");
            }
        }

        public bool SetupRealTimePlay(ProgressReporter? p = null, CancellationToken c = default)
        {
            var screen = Screen;
            if (screen.IsBmsReady)
            {
                var f = new AntiFreezeUpdater();
                var options = Options;
                var rate = options.AudioSampleRate;
                const int ch = 2;
                var offset = options.StartOffset;
                var delay = options.AudioDelay;

                screen.SetupPlay(true);

                screen.FadeOpacity = 0;
                var composer = screen.AudioComposer;
                _waveOut.Init(composer, false);

                // タイマー
                var timer = screen.Timer;
                timer.Set(TimerId.Scene_Start, 0);
                timer.Set(TimerId.Play_MusicStart, -offset);

                var (_, maxLength) = InitializeAudio(rate, ch, delay - offset, ref f, p, c);
                //composer.EnsureBufferSize(maxLength);
                f.WaitForUpdate(c);

                screen.Update(0);
                f.WaitForUpdate(c);
                return true;
            }
            return false;
        }

        private long _startTime;

        public void StartRealTimePlay()
        {
            IsPlaying = true;
            _startTime = Stopwatch.GetTimestamp();
            CompositionTarget.Rendering += RTP_Update;
            _waveOut.Play();
        }

        public void StopRealTimePlay()
        {
            IsPlaying = false;
            CompositionTarget.Rendering -= RTP_Update;
            _waveOut.Stop();
        }

        private void RTP_Update(object? sender, EventArgs e)
        {
            var time = Stopwatch.GetElapsedTime(_startTime).TotalSeconds;
            Screen.Update(time);
        }
    }
}
