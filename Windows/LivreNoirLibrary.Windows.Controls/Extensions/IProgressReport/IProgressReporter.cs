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
        public static void StartTask(this IProgressReporter ip, StartTaskArgs args)
        {
            try
            {
                ip.WorkingTask?.Wait();
            }
            catch { }
            PrepareTask(ip, args.InitialReport, args.IsAbortable);
            ProgressReporter progress = new(ip.ProgressBar.OnProgressChanged);
            ip.WorkingTask = ProcessTask(ip, progress, args);
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

        private static async Task ProcessTask(IProgressReporter ip, ProgressReporter p, StartTaskArgs args)
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
                        args.MainProcess(p, token);
                    }
                    catch (OperationCanceledException)
                    {
                        aborted = true;
                    }
                }, token);
            }
            finally
            {
                _ = ip.Dispatcher.BeginInvoke(() =>
                {
                    ip.WorkingTask = null;
                    args.Finished?.Invoke(aborted);
                    FinishTask(ip);
                });
            }
        }
    }
}
