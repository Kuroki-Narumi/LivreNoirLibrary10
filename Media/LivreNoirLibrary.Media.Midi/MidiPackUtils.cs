using System;
using System.Text.RegularExpressions;
using static LivreNoirLibrary.Media.PackUtils;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class MidiPackUtils
    {
        public static string Format(string format, string filename, IScore data, int trackId)
        {
            return Regex_Format.Replace(format, matched => matched.Value.ToLower() switch
            {
                ExportFormat_Filename => filename,
                ExportFormat_Title => data.Title ?? "",
                ExportFormat_Copyright => data.Copyright ?? "",
                ExportFormat_TrackTitle => data.GetTrack(trackId).Title ?? "",
                ExportFormat_TrackId => $"{trackId:D2}",
                _ => "",
            });
        }

        [GeneratedRegex($"{ExportFormat_Filename}|{ExportFormat_Title}|{ExportFormat_Copyright}|{ExportFormat_TrackTitle}|{ExportFormat_TrackId}", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex Regex_Format { get; }
    }
}
