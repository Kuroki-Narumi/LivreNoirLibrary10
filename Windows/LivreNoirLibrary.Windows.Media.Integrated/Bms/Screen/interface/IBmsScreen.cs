using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections;
using System.Threading;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface IBmsScreen
    {
        bool IsBmsReady { get; }
        double FirstSoundTime { get; }
        double LastSoundTime { get; }
        ISkinRoot? SkinRoot { get; }
        IBmsTimer Timer { get; }
        double FadeOpacity { get; set; }
        AudioComposer<string> AudioComposer { get; }

        void SetupAudio();
        void SetupPlay(bool isAutoPlay);
        void Update(double time);
        void CopyPixels(Span<byte> buffer, int width);
    }
}
