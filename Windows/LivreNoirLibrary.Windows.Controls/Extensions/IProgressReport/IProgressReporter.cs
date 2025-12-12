using System;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IProgressReporter
    {
        UIElement MainElement { get; }
        TaskProgressBar ProgressBar { get; }
        Task? WorkingTask { get; set; }
        Dispatcher Dispatcher { get; }
    }

    public static class IProgressExtension
    {
        public static void StartTask(this IProgressReporter ip, StartTaskArgs args) => StartTask(ip, args.MainProcess, args.IsAbortable, args.InitialReport, args.Finished);

        public static void StartTask(this IProgressReporter ip, ProgressHandler mainProcess, bool isAbortable = true, ProgressReport initialReport = default, TaskFinishedHandler? finished = null)
        {
            if (ip.WorkingTask is { } task)
            {
                task.ConfigureAwait(false);
                task.Wait();
            }
            PrepareTask(ip, initialReport, isAbortable);
            ProgressReporter progress = new(ip.ProgressBar.OnProgressChanged);
            ip.WorkingTask = ProcessTask(ip, progress, mainProcess, finished);
        }

        public static void PrepareTask(this IProgressReporter ip, in ProgressReport report, bool abortable = false)
        {
            ip.MainElement.IsEnabled = false;
            if (ip is Window window)
            {
                window.Closing += CancelClosing;
            }
            ip.ProgressBar.Prepare(report, abortable);
        }

        public static void FinishTask(this IProgressReporter ip)
        {
            ip.ProgressBar.Terminate();
            if (ip is Window window)
            {
                window.Closing -= CancelClosing;
            }
            ip.MainElement.IsEnabled = true;
        }

        private static void CancelClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static async Task ProcessTask(IProgressReporter ip, ProgressReporter p, ProgressHandler mainProcess, TaskFinishedHandler? finished)
        {
            var aborted = false;
            var c = ip.ProgressBar.CreateCancellationTokenSource();
            var token = c.Token;
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        mainProcess(p, token);
                    }
                    catch (OperationCanceledException)
                    {
                        aborted = true;
                    }
                }, token);
            }
            finally
            {
                ip.WorkingTask = null;
                await ip.Dispatcher.BeginInvoke(() =>
                {
                    finished?.Invoke(aborted);
                    FinishTask(ip);
                });
            }
        }

        public static void StartTaskSynchronized(this IProgressReporter ip, StartTaskArgs args) => StartTaskSynchronized(ip, args.MainProcess, args.IsAbortable, args.InitialReport, args.Finished);

        public static void StartTaskSynchronized(this IProgressReporter ip, ProgressHandler mainProcess, bool isAbortable = true, ProgressReport initialReport = default, TaskFinishedHandler? finished = null)
        {
            if (ip.WorkingTask is { } task)
            {
                task.ConfigureAwait(false);
                task.Wait();
            }
            PrepareTask(ip, initialReport, isAbortable);
            ProgressReporter progress = new(ip.ProgressBar.OnProgressChanged)
            {
                IsSynchronized = true
            };
            var aborted = false;
            var c = ip.ProgressBar.CreateCancellationTokenSource();
            var token = c.Token;
            try
            {
                mainProcess(progress, token);
            }
            catch (OperationCanceledException)
            {
                aborted = true;
            }
            finally
            {
                finished?.Invoke(aborted);
                FinishTask(ip);
            }
        }
    }
}
