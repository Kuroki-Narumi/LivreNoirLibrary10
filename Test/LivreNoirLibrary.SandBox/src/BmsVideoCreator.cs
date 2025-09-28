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
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;

namespace LivreNoirLibrary.SandBox
{
    public class BmsVideoCreator : ObservableObjectBase
    {
        public static readonly DrSize[] SizeList = [new(640, 480), new(1280, 720), new(1920, 1080)];
        public static readonly Rational[] FpsList = [FrameRates.Fps24, FrameRates.Fps30, FrameRates.Fps50, FrameRates.Fps60, FrameRates.Fps120, FrameRates.Fps240];

        public string? BmsPath { get; set => SetValue(ref field, value, [nameof(BmsDirectory)]); }
        public string? BmsDirectory => Path.GetDirectoryName(BmsPath);
        public BmsData? BmsData { get; set => SetValue(ref field, value); }
        public AssembleOptions AssembleOptions { get; set => SetValue(ref field, value); } = new() { Marker = false, Gain = -6 };

        public RenderTargetBitmap? RenderTarget { get; set => SetValue(ref field, value); }

        public DrSize Size
        {
            get;
            set
            {
                if (SetValue(ref field, value))
                {
                    RenderTarget = null;
                }
            }
        } = new(1280, 720);

        public Rational FrameRate { get; set => SetValue(ref field, value); } = FrameRates.Fps60;
        public bool IsHevc { get; set => SetValue(ref field, value); } = true;
        public int ApproximateKbps { get; set => SetValue(ref field, value); } = 5000;

        public BgaElement Bga { get; set => SetValue(ref field, value); } = new() { Rect = new(400, 40, 480, 480) };

        public bool OpenBms(string path)
        {
            try
            {
                var data = BmsData.Open(path);
                BmsPath = path;
                BmsData = data;
                return true;
            }
            catch (Exception ex)
            {
                ExConsole.Write(ex);
                BmsData = null;
                return false;
            }
        }

        private WaveData? _assembledData;

        public void Assemble(ProgressReporter p, CancellationToken c)
        {
            _assembledData = null;
            if (BmsData is { } data)
            {
                try
                {
                    p.Report("Assembling...", null);
                    _assembledData = data.Assemble(AssembleOptions, BmsDirectory!, p, c);
                }
                catch
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
            if (BmsData is { } data)
            {
                var (width, height) = Size;

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
                var drawingVisual = new DrawingVisual();
                var renderTarget = (RenderTarget ??= new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32));
                var size = width * height * 4;
                var videoBuffer = ArrayPool<byte>.Shared.Rent(size);
                var videoSpan = videoBuffer.AsSpan(0, size);

                // 音声
                var audioIndex = 0;
                var audioUnit = waveBuffer.SampleRate * waveBuffer.Channels;

                // BGA
                var bga = Bga;
                bga.Load(data, BmsDirectory!);

                // デバッグ用
                var t0 = Stopwatch.GetTimestamp();
                var fps_threshold = fps_den * fps_num;
                var fps_total = 0L;
                var totalFrame = (int)(waveBuffer.TotalSeconds * fps);

                p.Report("Encoding...", null);
                try
                {
                    for (var frame = 0; frame < totalFrame; frame++)
                    {
                        var tick = frame * TimeSpan.TicksPerSecond * fps_den / fps_num;
                        c.ThrowIfCancellationRequested();
                        p.Report($"Write frame {frame}/{totalFrame}({frame / Stopwatch.GetElapsedTime(t0).TotalSeconds:0.000}fps)", frame, totalFrame);
                        
                        // 映像フレームの作成
                        using (var ctx = drawingVisual.RenderOpen())
                        {
                            ctx.DrawRectangle(Brushes.White, null, new(0, 0, width, height));
                            bga.Render(ctx, tick);
                        }
                        renderTarget.Render(drawingVisual);

                        // 映像フレームの書き込み
                        renderTarget.CopyPixels(renderTarget.GetRect(), videoBuffer, width * 4, 0);
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

                        // 画面を更新するための処理
                        DependencyObjectExtensions.WaitForUpdate();
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
