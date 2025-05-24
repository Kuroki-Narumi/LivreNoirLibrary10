using System;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Media.Midi;

namespace LivreNoirLibrary.Media.BM3
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

        public static string Format(string format, string filename, IScore data, int trackId)
        {
            return Regex_Format.Replace(format, matched => matched.Value.ToLower() switch
            {
                ExportFormat_Filename => filename,
                ExportFormat_Title => data.Title ?? "",
                ExportFormat_Copyright => data.Copyright ?? "",
                ExportFormat_TrackTitle => data.GetTrackTitle(trackId),
                ExportFormat_TrackId => $"{trackId:D2}",
                _ => "",
            });
        }

        [GeneratedRegex($"{ExportFormat_Filename}|{ExportFormat_Title}|{ExportFormat_Copyright}|{ExportFormat_TrackTitle}|{ExportFormat_TrackId}", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex Regex_Format { get; }
    }
}
