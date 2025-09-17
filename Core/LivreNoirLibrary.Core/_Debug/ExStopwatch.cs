using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Debug
{
    public class ExStopwatch
    {
        public static StopwacthTweeter ProcessTime(string processName, bool tweet = true) => new(tweet, processName);
        public static StopwacthTweeter FileProcessTime(string type, string path, bool tweet = true) => new(tweet, $"{type} \"{path}\"");
        public static StopwacthTweeter LoadProcessTime(string path, bool tweet = true) => FileProcessTime("Loaded", path, tweet);
        public static StopwacthTweeter SaveProcessTime(string path, bool tweet = true) => FileProcessTime("Saved", path, tweet);
    }

    public readonly struct StopwacthTweeter(bool tweet = true, [CallerMemberName]string processName = "") : IDisposable
    {
        private readonly long _tick = Stopwatch.GetTimestamp();
        private readonly string _name = processName;
        private readonly bool _tweet = tweet;

        public long ErapsedTicks => Stopwatch.GetTimestamp() - _tick;
        public TimeSpan ElapsedTime => Stopwatch.GetElapsedTime(_tick);

        public void Dispose()
        {
            if (_tweet)
            {
                ExConsole.Write($"{_name} in {ElapsedTime:mm\\:ss\\.fff}");
            }
        }
    }
}
