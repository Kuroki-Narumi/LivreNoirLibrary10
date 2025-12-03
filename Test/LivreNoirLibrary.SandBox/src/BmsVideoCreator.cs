using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrSize = System.Drawing.Size;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Integrated;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.SandBox
{
    public class BmsVideoCreator : ObservableObjectBase
    {
        public static readonly DrSize[] SizeList = [new(640, 480), new(1280, 720), new(1920, 1080)];
        public static readonly Rational[] FpsList = [FrameRates.Fps24, FrameRates.Fps30, FrameRates.Fps50, FrameRates.Fps60, FrameRates.Fps120, FrameRates.Fps240];

        [JsonIgnore]
        public BmsScreen Screen { get; } = new();
        public AssembleOptions AssembleOptions { get; set => SetValue(ref field, value); } = new() { SetMarker = false, Gain = -3 };

        public Rational FrameRate { get; set => SetValue(ref field, value); } = FrameRates.Fps60;
        public bool IsHevc { get; set => SetValue(ref field, value); } = true;
        public int ApproximateKbps { get; set => SetValue(ref field, value); } = 10000;
        public double FadeinTime { get; set; } = 0;
        public double LoadTime { get; set; } = 0;
        public double LoadCompleteTime { get; set; } = 0;
        public double FadeoutTime { get; set; } = 1;

        public void LoadSkin(Skin? skin) => Screen.Skin = skin;
        public bool OpenBms(string path)
        {
            Screen.BmsPath = path;
            return Screen.IsBmsReady;
        }

        private WaveData? _assembledData;

        public void Assemble(ProgressReporter p, CancellationToken c)
        {
            _assembledData = null;
            if (Screen.IsBmsReady)
            {
                try
                {
                    p.Report("Assembling...", null);
                    AssembleOptions.RootDirectory = Screen.Directory!;
                    (_assembledData, _) = Screen.ViewModel.Assemble(WaveBufferProvider.Default, AssembleOptions, p, c);
                }
                catch (OperationCanceledException)
                {
                    _assembledData = null;
                    throw;
                }
            }
        }

        public bool TryFlushAssembledData([MaybeNullWhen(false)]out WaveData data)
        {
            if (_assembledData is { } wav)
            {
                data = wav;
                _assembledData = null;
                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }

        public void CreateVideo(string path, WaveData waveBuffer, ProgressReporter p, CancellationToken c)
        {
            var screen = Screen;
            if (screen.Skin is { } skin && screen.IsBmsReady)
            {
                screen.DetermineExpressions();
                screen.SetupPlay(true);
                var (width, height) = skin.BaseSize;
                // エンコーダー
                using FFmpegEncoder encoder = new(path);
                var fps = FrameRate;
                var (fps_num, fps_den) = fps;
                ICodecOptions codecOptions = IsHevc ? new HevcEncodeOptions() : new H264EncodeOptions();
                IHardwareEncodeOptions? hardwareOptions = new NvencEncodeOptions();
                VideoEncodeOptions videoOptions = new(width, height, fps, ApproximateKbps * 1000, codecOptions, hardwareOptions);
                var video = encoder.CreateVideoStream(videoOptions);
                var audio = encoder.CreateAudioStream(new AudioEncodeOptions(waveBuffer.SampleRate, waveBuffer.Channels));
                // 読み書きバッファ
                var size = width * height * 4;
                var videoBuffer = ArrayPool<byte>.Shared.Rent(size);
                var videoSpan = videoBuffer.AsSpan(0, size);
                var renderTarget = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                var videoBufferStride = width * 4;
                // 音声
                var audioIndex = 0;
                var audioUnit = waveBuffer.SampleRate * waveBuffer.Channels;
                var audioBuffer = ArrayPool<float>.Shared.Rent(audioUnit);
                var audioSpan = audioBuffer.AsSpan();

                // タイマー
                var fadeinFinish = FadeinTime;
                var loadingFinish = fadeinFinish + LoadTime;
                var musicStart = loadingFinish + LoadCompleteTime;
                var fadeoutStart = musicStart + waveBuffer.TotalSeconds;
                var totalTime = fadeoutStart + FadeoutTime;
                screen.StartLoading(0);
                screen.FinishLoading(loadingFinish);
                screen.StartMusic(musicStart);

                // デバッグ用
                var t0 = Stopwatch.GetTimestamp();
                var fps_threshold = fps_den * fps_num;
                var fps_total = 0L;
                var totalFrame = (int)(totalTime * fps);

                p.Report("Encoding...", null);
                try
                {
                    screen.StartLoading(0);
                    for (var frame = 0; frame < totalFrame; frame++)
                    {
                        var time = (double)frame * fps_den / fps_num;
                        c.ThrowIfCancellationRequested();
                        var report = $"Write frame {frame}/{totalFrame}({frame / Stopwatch.GetElapsedTime(t0).TotalSeconds:0.000}fps)";
                        p.Report(report, frame, totalFrame);

                        screen.Update(time);
                        // 画面更新を待つ
                        screen.CopyPixels(videoSpan, width);
                        DependencyObjectExtensions.WaitForUpdate();
                        // 映像フレームの書き込み
                        encoder.WritePixels(videoSpan);
                        c.ThrowIfCancellationRequested();

                        // 1秒に1回音声フレームを書き込む
                        fps_total += fps_den;
                        if (fps_total >= fps_threshold)
                        {
                            WriteAudio();
                            c.ThrowIfCancellationRequested();
                            fps_total -= fps_threshold;
                        }

                    }
                    while (WriteAudio())
                    {
                        c.ThrowIfCancellationRequested();
                    }
                    p.Report("Finalizing...");

                    bool WriteAudio()
                    {
                        if (audioUnit is > 0)
                        {
                            var source = waveBuffer.Data;
                            var audioSpan = source.Slice(audioIndex, audioUnit);
                            audioSpan.Clamp(-1, 1);
                            encoder.WriteSamples(audioSpan);
                            audioIndex += audioSpan.Length;
                            audioUnit = Math.Min(audioUnit, source.Length - audioIndex);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(videoBuffer);
                }
            }
        }
    }
}
