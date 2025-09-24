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
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.SandBox
{
    public class ViewModel : ObservableObjectBase
    {
        public static readonly Size[] SizeList = [new(256, 256), new(512, 512), new(640, 480), new(1280, 720), new(1920, 1080)];
        public static readonly Rational[] FpsList = [FrameRates.Fps24, FrameRates.Fps30, FrameRates.Fps50, FrameRates.Fps60, FrameRates.Fps120, FrameRates.Fps240];

        public string? BmsPath { get; set => SetValue(ref field, value, [nameof(BmsDirectory)]); }
        public string? BmsDirectory => Path.GetDirectoryName(BmsPath);
        public BmsData? BmsData { get; set => SetValue(ref field, value); }
        public AssembleOptions AssembleOptions { get; set => SetValue(ref field, value); } = new() { Marker = false, Gain = -3 };

        public Size BgaSize { get; set => SetValue(ref field, value); } = SizeList[0];
        public Rational Fps { get; set => SetValue(ref field, value); } = FrameRates.Fps60;
        public Channel[] BgaLayers { get; set => SetValue(ref field, value); } = [Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2];

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
        private void AssembleCore(bool adjust, ProgressReporter p, CancellationToken c)
        {
            _assembledData = null;
            if (BmsData is { } data)
            {
                try
                {
                    var op = AssembleOptions;
                    op.Adjust = adjust;
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

        public void AssembleBms(ProgressReporter p, CancellationToken c) => AssembleCore(true, p, c);

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

        public void CreateVideo(string path, ProgressReporter p, CancellationToken c)
        {
            if (BmsData is { } data)
            {
                AssembleCore(false, p, c);
                if (!TryFlushAssembledData(out var waveBuffer))
                {
                    throw new OperationCanceledException();
                }
                TimeCounter counter = new(data);
                var (width, height) = BgaSize.ToInt();

                // 読み書きバッファ
                var size = width * height * 4;
                var videoBuffer = ArrayPool<byte>.Shared.Rent(size);
                var videoSpan = videoBuffer.AsSpan(0, size);
                var audioIndex = 0;
                var audioUnit = waveBuffer.SampleRate * waveBuffer.Channels;

                // 映像キャッシュ
                BgaTimingList bga = new(data, counter, BmsDirectory!);
                Dictionary<string, ImageData> imageCache = [];
                var layers = BgaLayers;

                using FFmpegEncoder encoder = new(path);
                var fps = Fps;
                HevcEncodeOptions hevcOptions = new();
                VideoEncodeOptions videoOptions = new(width, height, fps, 5_000_000, hevcOptions, null);
                var video = encoder.CreateVideoStream(videoOptions);
                var audio = encoder.CreateAudioStream(new AudioEncodeOptions(waveBuffer.SampleRate, waveBuffer.Channels));

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

                var t0 = Stopwatch.GetTimestamp();
                var fps_unit = fps.Denominator;
                var fps_threshold = fps_unit * fps.Numerator;
                var fps_total = 0L;
                var totalFrame = (int)(waveBuffer.TotalSeconds * fps);
                p.Report("Encoding...", null);
                try
                {
                    for (var frame = 0; frame < totalFrame; frame++)
                    {
                        SimdOperations.Clear(videoSpan);
                        c.ThrowIfCancellationRequested();
                        p.Report($"Write frame {frame}/{totalFrame}({frame / Stopwatch.GetElapsedTime(t0).TotalSeconds:0.000}fps)", frame, totalFrame);
                        var time = (decimal)(frame / fps);
                        foreach (var channel in layers)
                        {
                            if (bga.TryGetValue(channel, time, out var actualTime, out var bgaPath))
                            {
                                var image = imageCache.GetOrAdd(bgaPath, p => new ImageData(p, 0, 0));
                                image.CopyPixels(time, videoSpan, width, height);
                            }
                        }
                        encoder.WritePixels(videoSpan);
                        c.ThrowIfCancellationRequested();
                        fps_total += fps_unit;
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

                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(videoBuffer);
                }
                p.Report("Finalizing...");
            }
        }
    }
}
