using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using System;
using System.Diagnostics;
using System.Threading;

namespace LivreNoirLibrary.Windows.Media
{
    public static class VideoCreator
    {
        public static void CreateVideo<T>(this IVideoCreator<T> provider, string path, ProgressReporter? p = null, CancellationToken c = default)
        where T : IVideoSaveState
        {
            if (!provider.IsValid)
            {
                return;
            }
            var f = new AntiFreezeUpdater();
            var state = provider.CreateSaveState(ref f, p, c);

            var width = state.PixelWidth;
            var height = state.PixelHeight;
            var fps = state.FrameRate;
            var (fps_num, fps_den) = fps;
            var sampleRate = state.AudioSampleRate;
            var ch = state.AudioChannels;
            Mpeg4EncodeOptions codecOptions = state.IsHevc ? new HevcEncodeOptions() : new H264EncodeOptions();
            IHardwareEncodeOptions? hardwareOptions = new NvencEncodeOptions();
            VideoEncodeOptions videoOptions = new(width, height, fps, state.VideoBitrate, codecOptions, hardwareOptions);
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
                if (state.AudioExists)
                {
                    encoder.CreateAudioStream(new AudioEncodeOptions(sampleRate, ch, state.AudioBitrate));
                }
            }
            catch (Exception e)
            {
                ExConsole.Write($"ERROR: failed to create stream.");
                ExConsole.Write((e.GetType(), e.Message));
                return;
            }

            // バッファ
            using var o_a = ArrayPool.Rent<float>(sampleRate * ch);
            using var o_v = ArrayPool.Rent<byte>(width * height * 4);
            var videoSpan = o_v.Span;

            long totalFrameUnit;
            void UpdateTotalFrameCount(double time) => totalFrameUnit = (long)Math.Ceiling(time * fps_num) + fps_den;

            // デバッグ用
            var t0 = Stopwatch.GetTimestamp();
            var totalTime = state.TotalTime;
            var deadline = state.AbortDeadline;
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
                    var report = $"Write frame {time:F2}/{totalTime:F2}({frame / Stopwatch.GetElapsedTime(t0).TotalSeconds / fps_den:0.000}fps)";
                    p?.Report(report, time, totalTime);

                    // 映像バッファの更新
                    provider.UpdateSaveState(state, time);
                    provider.CopyPixels(videoSpan, width);
                    // 映像フレームの書き込み
                    encoder.WritePixels(videoSpan);

                    f.WaitForUpdate();

                    // 中止処理
                    if (c.IsCancellationRequested && !aborting && time < deadline)
                    {
                        p?.Report("Aborting...", null);
                        state.OnAbort(ref time);
                        UpdateTotalFrameCount(time);
                        frameMax = Math.Min(totalFrameUnit, frame + fps_num);
                        frameUnit = frameMax - frame;
                        aborting = true;
                    }
                }

                // 音声は1秒ごとに書き込む
                var totalSample = (int)(frameUnit * sampleRate * 2 / fps_num);
                var span = o_a.AsSpan(totalSample);
                // 音声バッファを取得
                provider.ReadSamples(span);
                span.Clamp(-1, 1);
                // 音声フレームの書き込み
                encoder.WriteSamples(span);
            }

            p?.Report("Finalizing...");
        }
    }
}
