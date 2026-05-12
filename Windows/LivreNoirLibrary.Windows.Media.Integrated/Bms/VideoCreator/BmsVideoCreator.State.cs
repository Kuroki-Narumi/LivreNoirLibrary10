using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;
using System;
using System.Threading;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public partial class BmsVideoCreator : IVideoCreator<BmsVideoCreator.SaveState>
    {
        bool IVideoCreator<SaveState>.IsValid => Screen?.SkinRoot is IPlaySkinRoot skin && Screen.IsBmsReady;

        SaveState IVideoCreator<SaveState>.CreateSaveState(ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c)
        {
            var screen = Screen;
            var skin = (screen.SkinRoot as IPlaySkinRoot)!;

            Screen.SetupPlay(true);
            var options = Options;
            var rate = options.AudioSampleRate;
            const int ch = 2;
            const int audioBitrate = 224 * 1000;
            var (width, height) = skin.BaseSize;

            // タイマー
            var offset = options.StartOffset;
            var fadeInDuration = skin.FadeInTime.Validate(0);
            var loadingFinish = skin.LoadTime.Validate(0) - offset;
            var musicStart = loadingFinish + skin.ReadyTime.Validate(0) - offset;
            var (musicLength, _) = InitializeAudio(rate, ch, musicStart + options.AudioDelay, ref f, p, c);

            var totalTime = musicStart + Math.Max(musicLength, screen.LastSoundTime + skin.MarginTime.Validate(0));
            var fadeOutDuration = skin.FadeOutTime.Validate(0);
            var fadeOutStart = totalTime - fadeOutDuration;
            var timer = screen.Timer;
            timer.Set(TimerId.Scene_Start, 0);
            timer.Set(TimerId.Play_LoadingStart, -offset);
            timer.Set(TimerId.Play_MusicStart, musicStart);
            timer.Set(TimerId.Scene_Terminate, fadeOutStart);

            return new()
            {
                PixelWidth = width,
                PixelHeight = height,
                AudioSampleRate = rate,
                AudioChannels = ch,
                FrameRate = options.FrameRate,
                VideoBitrate = options.ApproximateKbps * 1000,
                AudioBitrate = audioBitrate,
                IsHevc = options.IsHevc,
                TotalTime = totalTime,
                AbortDeadline = fadeOutStart,
                FadeOutDuration = fadeOutDuration,
                FadeInDuration = fadeInDuration,
                LoadingFinish = loadingFinish,
            };
        }

        void IVideoCreator<SaveState>.UpdateSaveState(SaveState state, double time)
        {
            var timer = Screen.Timer;
            // フェード処理
            Screen.FadeOpacity =
                time <= state.FadeInDuration ? 1 - time / state.FadeInDuration
                : time >= state.AbortDeadline ? (time - state.AbortDeadline) / state.FadeOutDuration
                : 0;

            // ロード画面を消す処理
            if (state.NeedLoadingFinish && time >= state.LoadingFinish)
            {
                timer.Remove(TimerId.Play_LoadingStart);
                timer.Set(TimerId.Play_LoadingFinish, time);
                state.NeedLoadingFinish = false;
            }

            // 映像バッファの更新
            Screen.Update(time);
        }

        void IVideoCreator<SaveState>.CopyPixels(Span<byte> buffer, int bufferWidth)
        {
            Screen.CopyPixels(buffer, bufferWidth);
        }

        void IVideoCreator<SaveState>.ReadSamples(Span<float> buffer)
        {
            Screen.AudioComposer.Read(buffer);
        }

        public class SaveState : IVideoSaveState
        {
            public required int PixelWidth { get; init; }
            public required int PixelHeight { get; init; }
            public bool AudioExists => true;
            public required int AudioSampleRate { get; init; }
            public required int AudioChannels { get; init; }
            public required Rational FrameRate { get; init;  }
            public required int VideoBitrate { get; init;  }
            public required int AudioBitrate { get; init;  }
            public required bool IsHevc { get; init; }

            public required double TotalTime { get; init; }
            public required double AbortDeadline { get; set; }

            public required double FadeInDuration { get; init; }
            public required double FadeOutDuration { get; init; }
            public bool NeedLoadingFinish { get; internal set; } = true;
            public required double LoadingFinish { get; init; }

            public void OnAbort(ref double time)
            {
                AbortDeadline = time;
                time = time + FadeOutDuration;
            }
        }

    }
}
