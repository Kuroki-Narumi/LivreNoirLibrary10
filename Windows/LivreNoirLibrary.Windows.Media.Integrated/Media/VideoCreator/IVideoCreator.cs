using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using System;
using System.Threading;

namespace LivreNoirLibrary.Windows.Media
{
    public interface IVideoCreator<T>
        where T : IVideoSaveState
    {
        bool IsValid => true;

        T CreateSaveState(ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c);
        void UpdateSaveState(T state, double time);
        void CopyPixels(Span<byte> buffer, int bufferWidth);
        void ReadSamples(Span<float> buffer);
    }
}
