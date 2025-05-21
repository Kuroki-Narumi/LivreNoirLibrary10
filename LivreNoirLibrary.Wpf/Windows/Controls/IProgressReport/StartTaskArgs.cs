using System;
using System.Threading;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ProgressHandler(ProgressReporter p, CancellationToken c);
    public delegate void TaskFinishedHandler(bool aborted);

    public class StartTaskArgs
    {
        public bool IsAbortable { get; init; } = true;
        public ProgressHandler MainProcess { get; init; } = NullProcess;
        public ProgressReport InitialReport { get; init; }
        public TaskFinishedHandler? Finished { get; init; }

        public static void NullProcess(ProgressReporter p, CancellationToken c) { }
    }
}
