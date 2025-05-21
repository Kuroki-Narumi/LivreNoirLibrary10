using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Files
{
    public static partial class ExtRegs
    {
        private const string Prefix = @"\.(";
        private const string Suffix = ")$";
        private const char Separator = '|';

        public static Regex Create(string ext)
        {
            return new($"{Prefix}{ext}{Suffix}", RegexOptions.IgnoreCase);
        }

        public static Regex Create(params ReadOnlySpan<string?> exts)
        {
            return new($"{Prefix}{string.Join(Separator, exts)}{Suffix}", RegexOptions.IgnoreCase);
        }

        public static Regex CreateJoin(IEnumerable<string> exts)
        {
            return new($"{Prefix}{string.Join(Separator, exts)}{Suffix}", RegexOptions.IgnoreCase);
        }

        public static Regex CreateJoin(params ReadOnlySpan<IEnumerable<string>> exts)
        {
            StringBuilder sb = new();
            sb.Append(Prefix);
            for (int i = 0; i < exts.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(Separator);
                }
                sb.AppendJoin(Separator, exts[i]);
            }
            sb.Append(Suffix);
            return new(sb.ToString(), RegexOptions.IgnoreCase);
        }

        public static bool IsMatch(string path, string ext)
        {
            return Create(ext).IsMatch(path);
        }

        public static bool IsMatch(string path, params ReadOnlySpan<string?> exts)
        {
            return Create(exts).IsMatch(path);
        }

        public static readonly Regex Text = Create(Exts.Text);
        public static readonly Regex Json = Create(Exts.Json);
        public static readonly Regex Exe = Create(Exts.Exe);
        public static readonly Regex Dat = Create(Exts.Dat);
        public static readonly Regex Ini = Create(Exts.Ini);
        public static readonly Regex Icon = Create(Exts.Icon);

        public static readonly Regex Wav = Create(Exts.Wav, Exts.Wave);
        public static readonly Regex Vorbis = Create(Exts.Vorbis);
        public static readonly Regex WavVorbis = Create(Exts.Wav, Exts.Wave, Exts.Vorbis);
        public static readonly Regex MP3 = Create(Exts.MP3);
        public static readonly Regex Flac = Create(Exts.Flac);
        public static readonly Regex Aiff = Create(Exts.Aiff);
        public static readonly Regex AAC = Create(Exts.AAC);

        public static readonly Regex Audio = Create(Exts.AudioExts);
        public static readonly Regex ExAudio = Create(Exts.ExAudioExts);

        public static readonly Regex Png = Create(Exts.Png);
        public static readonly Regex Bmp = Create(Exts.Bmp, Exts.Dib);
        public static readonly Regex Jpeg = Create(Exts.Jpg, Exts.Jpeg);
        public static readonly Regex Gif = Create(Exts.Gif);
        public static readonly Regex Tiff = Create(Exts.Tif, Exts.Tiff);
        public static readonly Regex Wmf = Create(Exts.Wmf);
        public static readonly Regex Ras = Create(Exts.Ras);
        public static readonly Regex Eps = Create(Exts.Eps);
        public static readonly Regex Pcs = Create(Exts.Pcs);
        public static readonly Regex Pcd = Create(Exts.Pcd);
        public static readonly Regex Tga = Create(Exts.Tga);

        public static readonly Regex CompatibleImage = Create(Exts.CompatibleImageExts);
        public static readonly Regex Image = Create(Exts.ImageExts);
        public static readonly Regex ExImage = Create(Exts.ExImageExts);

        public static readonly Regex MP4 = Create(Exts.MP4);
        public static readonly Regex Mpeg = Create(Exts.Mpg, Exts.Mpeg);
        public static readonly Regex Avi = Create(Exts.Avi);
        public static readonly Regex Wmv = Create(Exts.Wmv);
        public static readonly Regex Video = Create(Exts.VideoExts);

        public static readonly Regex Midi = Create(Exts.Mid, Exts.Midi);
        public static readonly Regex Bms = Create(Exts.Bms, Exts.Bme, Exts.Bml);
        public static readonly Regex Pms = Create(Exts.Pms, Exts.Pml);
        public static readonly Regex Bmson = Create(Exts.Bmson);
        public static readonly Regex BeMusic = Create(Exts.BeMusicExts);
        public static readonly Regex BmKind = Create(Exts.BmKindExts);
        public static readonly Regex BmMedia = Create(Exts.BmMediaExts);

        public static readonly Regex BM3Score = Create(Exts.BM3Score, Exts.Mid, Exts.Midi);
        public static readonly Regex BM3Project = Create(Exts.BM3Project);
        public static readonly Regex Mascot = Create(Exts.Mascot);
    }
}
