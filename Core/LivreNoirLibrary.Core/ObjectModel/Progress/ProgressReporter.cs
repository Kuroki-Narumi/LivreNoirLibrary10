using System;

namespace LivreNoirLibrary.ObjectModel
{
    public sealed class ProgressReporter(Action<ProgressReport> handler) : Progress<ProgressReport>(handler)
    {
        public void Report(string? caption, string? message, double value, double max) => OnReport(new(caption, message, value, max));

        public void Report(string? message) => Report(null, message, -1, -1);
        public void Report(string? message, double value) => Report(null, message, value, -1);
        public void Report(string? message, double value, double max) => Report(null, message, value, max);
        public void Report(string? caption, string? message) => Report(caption, message, -1, -1);
        public void Report(string? caption, string? message, double value) => Report(caption, message, value, -1);

        public void ReportInitial(string? caption, string? message = null, double max = 0) => OnReport(new(caption, message, 0, max));
        public void ReportPercent(double value, double max) => Report($"{(max is > 0 ? value / max : 0):P1}", value, max);
        public void ReportFraction(double value, double max) => Report($"{value:F0}/{max:F0}", value, max);
    }
}
