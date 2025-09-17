using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public readonly struct ProgressReport(string? caption, string? message, double value, double maximum)
    {
        public string? Caption { get; } = StringPool.Get(caption);
        public string? Message { get; } = message;
        public double Value { get; } = value;
        public double Maximum { get; } = maximum;

        public static ProgressReport Initial(string? caption, double maximum = 0) => new(caption, null, 0, maximum);
        public static ProgressReport Initial(string? caption, string? message, double maximum = 0) => new(caption, message, 0, maximum);
    }
}
