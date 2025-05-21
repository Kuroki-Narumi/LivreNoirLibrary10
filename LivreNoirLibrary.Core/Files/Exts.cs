using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Files
{
    public static partial class Exts
    {
        [GeneratedRegex(@"(?<=\.)[^.]+$")]
        public static partial Regex ExtReg { get; }
        [GeneratedRegex(@"\.[^.]+$")]
        public static partial Regex ExtRegWithDot { get; }

        public static string Replace(string path, string ext) => ExtReg.Replace(path, ext);
        public static string Trim(string path) => ExtRegWithDot.Replace(path, "");

        public static string Get(string path)
        {
            var match = ExtReg.Match(path);
            return match.Success ? match.Value : "";
        }

        public static string Join(params ReadOnlySpan<string?> exts) => string.Join('.', exts);

        public static bool TryGetCompatible(string? path, [MaybeNullWhen(false)]out string actualPath, params ReadOnlySpan<string> exts)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Path.HasExtension(path))
                {
                    path += $".{exts[0]}";
                }
                if (File.Exists(path))
                {
                    actualPath = path;
                    return true;
                }
                foreach (var ext in exts)
                {
                    var path2 = Replace(path, ext);
                    if (File.Exists(path2))
                    {
                        actualPath = path2;
                        return true;
                    }
                }
            }
            actualPath = null;
            return false;
        }

        public static string[] GetAllCompatible(string? path, params ReadOnlySpan<string> exts)
        {
            HashSet<string> results = [];
            if (!string.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    results.Add(path);
                }
                foreach (var ext in exts)
                {
                    var path2 = Replace(path, ext);
                    if (File.Exists(path2))
                    {
                        results.Add(path2);
                    }
                }
            }
            return [.. results];
        }

        public const string Text = "txt";
        public const string Json = "json";
        public const string Exe = "exe";
        public const string Dat = "dat";
        public const string Ini = "ini";
        public const string Icon = "ico";

        public const string Wav = "wav";
        public const string Wave = "wave";
        public const string Vorbis = "ogg";
        public const string MP3 = "mp3";
        public const string Flac = "flac";
        public const string Aiff = "aiff";
        public const string AAC = "aac";

        public const string Png = "png";
        public const string Bmp = "bmp";
        public const string Dib = "dib";
        public const string Jpg = "jpg";
        public const string Jpeg = "jpeg";
        public const string Gif = "gif";
        public const string Tif = "tif";
        public const string Tiff = "tiff";
        public const string Wmf = "wmf";
        public const string Ras = "ras";
        public const string Eps = "eps";
        public const string Pcs = "pcs";
        public const string Pcd = "pcd";
        public const string Tga = "tga";

        public const string PSD = "psd";
        public const string PDF = "pdf";

        public const string MP4 = "mp4";
        public const string Mpg = "mpg";
        public const string Mpeg = "mpeg";
        public const string Avi = "avi";
        public const string Wmv = "wmv";
        public const string AV1 = "av1";

        public const string Mid = "mid";
        public const string Midi = "midi";

        public const string Bms = "bms";
        public const string Bme = "bme";
        public const string Bml = "bml";
        public const string Pms = "pms";
        public const string Pml = "pml";
        public const string Bmson = "bmson";
        public const string B = "b";

        public const string BM3Score = "bm3scr";
        public const string BM3Project = "bm3prj";
        public const string Mascot = "mascot";

        public static readonly string[] MidiExts = [Mid, Midi];
        public static readonly string[] AudioExts = [Wav, Wave, Vorbis, MP3];
        public static readonly string[] ExAudioExts = [Wav, Wave, Vorbis, MP3, Flac, Aiff, AAC];
        public static readonly string[] CompatibleImageExts = [Png, Bmp, Dib, Gif, Tif, Tiff];
        public static readonly string[] ImageExts = [Png, Bmp, Dib, Gif, Tif, Tiff, Jpg, Jpeg];
        public static readonly string[] ExImageExts = [Png, Bmp, Dib, Gif, Tif, Tiff, Jpg, Jpeg, Wmf, Ras, Eps, Pcs, Pcd, Tga];
        public static readonly string[] VideoExts = [MP4, Mpg, Mpeg, Avi, Wmv, AV1];
        public static readonly string[] MediaExts = [..ImageExts, ..VideoExts];
        public static readonly string[] BeMusicExts = [Bms, Bme, Bml, Pms, Pml, B];
        public static readonly string[] BmKindExts = [Bms, Bme, Bml, Pms, Pml, Bmson];
        public static readonly string[] BmMediaExts = [Png, Bmp, Gif, Jpg, Jpeg, MP4, Mpg, Mpeg, Avi];
    }
}
