using System;
using System.Threading;
using System.Threading.Tasks;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ProgressHandler(ProgressReporter p, CancellationToken c);
    public delegate Task AsyncProgressHandler(ProgressReporter p, CancellationToken c);
    public delegate void TaskFinishedHandler(bool aborted);

    public class StartTaskArgs
    {
        public ProgressHandler? MainProcess { get; init; }
        public AsyncProgressHandler? AsyncProcess { get; init; }
        public ProgressReport InitialReport { get; init; }
        public bool IsAbortable { get; init; } = true;
        public TaskFinishedHandler? Finished { get; init; }
    }
}
