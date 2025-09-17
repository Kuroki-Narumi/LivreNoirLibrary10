using System;

namespace LivreNoirLibrary.Media
{
    public static partial class PackUtils
    {
        public const string ExportFormat_Filename = "<filename>";
        public const string ExportFormat_Title = "<title>";
        public const string ExportFormat_Copyright = "<copy>";
        public const string ExportFormat_TrackTitle = "<tname>";
        public const string ExportFormat_TrackId = "<tid>";

        public const string DefaultFormat_Filename = $"{ExportFormat_Filename}_";
        public const string DefaultFormat_Pack = $"{ExportFormat_Filename}_{ExportFormat_TrackId}_{ExportFormat_TrackTitle}";
        public const string DefaultFormat_Slice = $"{ExportFormat_TrackId}_{ExportFormat_TrackTitle}_";

        public static string Format(string format, string filename) => format.Replace(ExportFormat_Filename, filename);
    }
}
