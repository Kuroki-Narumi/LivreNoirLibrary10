using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LivreNoirLibrary.Files
{
    public static partial class Filters
    {
        private static readonly string[] WildCard = ["*"];

        private readonly struct FilterItem(string description, ReadOnlySpan<string> exts)
        {
            public readonly string Description = description;
            public readonly string[] Exts = exts.ToArray();
        }

        private static readonly Dictionary<string, FilterItem> _list = [];

        private static string Create(string name, string desc, params ReadOnlySpan<string> exts)
        {
            FilterItem item = new(desc, exts);
            _list.Add(name, item);
            return Get(item);
        }

        public static string GetByName(string name)
        {
            if (_list.TryGetValue(name, out var item))
            {
                return Get(item);
            }
            return name;
        }

        public static (string, string[]) GetRawByName(string name)
        {
            if (_list.TryGetValue(name, out var item))
            {
                return (item.Description, item.Exts);
            }
            return (name, WildCard);
        }

        private static string Get(in FilterItem item) => Get(item.Description, item.Exts);

        public static string Get(string description, params string[] exts)
        {
            var extStr = string.Join(';', exts.Select(e => $"*.{e}"));
            StringBuilder sb = new();
            sb.Append(description);
            sb.Append(" (");
            sb.Append(extStr);
            sb.Append(")|");
            sb.Append(extStr);
            return sb.ToString();
        }

        public static string Get(string description, params ReadOnlySpan<IEnumerable<string>> extLists)
        {
            StringBuilder sb = new();
            for (int i = 0; i < extLists.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(';');
                }
                sb.AppendJoin(';', extLists[i].Select(e => $"*.{e}"));
            }
            var extStr = sb.ToString();

            sb.Clear();
            sb.Append(description);
            sb.Append(" (");
            sb.Append(extStr);
            sb.Append(")|");
            sb.Append(extStr);
            return sb.ToString();
        }

        public static string Join(params ReadOnlySpan<string?> filters) => string.Join('|', filters);

        public static string Join(params ReadOnlySpan<IEnumerable<string>> filterLists)
        {
            StringBuilder sb = new();
            for (int i = 0; i < filterLists.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append('|');
                }
                sb.AppendJoin('|', filterLists[i]);
            }
            return sb.ToString();
        }

        public static readonly string Any = Create(nameof(Any), "Any File", "*");
        public static readonly string Text = Create(nameof(Text), "Text File", Exts.Text);
        public static readonly string Json = Create(nameof(Json), "JavaScript Object Notation", Exts.Json);
        public static readonly string Exe = Create(nameof(Exe), "Windows Executable File", Exts.Exe);
        public static readonly string Ini = Create(nameof(Ini), "Windows Initialization File", Exts.Ini);
        public static readonly string Icon = Create(nameof(Icon), "Windows Icon File", Exts.Icon);

        public static readonly string Wave = Create(nameof(Wave), "RIFF Waveform Audio Format", Exts.Wav, Exts.Wave);
        public static readonly string Vorbis = Create(nameof(Vorbis), "OGG Vorbis", Exts.Vorbis);
        public static readonly string MP3 = Create(nameof(MP3), "MPEG-1 Audio Layer-3", Exts.MP3);
        public static readonly string Flac = Create(nameof(Flac), "Free Lossless Audio Codec", Exts.Flac);
        public static readonly string Aiff = Create(nameof(Aiff), "Audio Interchange File Format", Exts.Aiff);
        public static readonly string AAC = Create(nameof(AAC), "Advanced Audio Coding", Exts.AAC);
        public static readonly string Audio = Create(nameof(Audio), "Audio File", Exts.AudioExts);
        public static readonly string ExAudio = Create(nameof(ExAudio), "Audio File", Exts.ExAudioExts);
        public static readonly string WavVorbis_Save = Join(Wave, Vorbis, MP3, Flac, Aiff, AAC);
        public static readonly string VorbisWav_Save = Join(Vorbis, Wave, MP3, Flac, Aiff, AAC);

        public static readonly string Png = Create(nameof(Png), "Portable Network Graphics", Exts.Png);
        public static readonly string Bmp = Create(nameof(Bmp), "Windows Bitmap File", Exts.Bmp, Exts.Dib);
        public static readonly string Gif = Create(nameof(Gif), "Graphics Interchange Format", Exts.Gif);
        public static readonly string Tiff = Create(nameof(Tiff), "Tagged Image File Format", Exts.Tif, Exts.Tiff);
        public static readonly string Jpg = Create(nameof(Jpg), "Joint Photographic Experts Group", Exts.Jpg, Exts.Jpeg);
        public static readonly string Image = Create(nameof(Image), "Image File", Exts.ImageExts);
        public static readonly string ExImage = Create(nameof(ExImage), "Image File", Exts.ExImageExts);
        public static readonly string Image_Save = Join(Png, Bmp, Gif, Tiff);

        public static readonly string Mpeg = Create(nameof(Mpeg), "Mpeg1 video", Exts.Mpg, Exts.Mpeg);
        public static readonly string MP4 = Create(nameof(MP4), "Mpeg 4 video", Exts.MP4);
        public static readonly string Avi = Create(nameof(Avi), "Audio Video Interleave", Exts.Avi);
        public static readonly string Wmv = Create(nameof(Wmv), "Windows Media Video", Exts.Wmv);
        public static readonly string Video = Create(nameof(Video), "Video File", Exts.VideoExts);
        public static readonly string Video_Save = Join(MP4, Mpeg, Avi, Wmv);

        public static readonly string Midi = Create(nameof(Midi), "Standard MIDI File", Exts.Mid, Exts.Midi);
        public static readonly string Bms = Create(nameof(Bms), "Be Music Script", Exts.BeMusicExts);
        public static readonly string Bms_Save = Create(nameof(Bms_Save), "Be Music Script", Exts.Bms, Exts.Bml, Exts.Bme);
        public static readonly string Pms_Save = Create(nameof(Pms_Save), "BMS 9 buttons", Exts.Pms, Exts.Pml);
        public static readonly string B_Save = Create(nameof(B_Save), "BMS for test", Exts.B);
        public static readonly string Bmson = Create(nameof(Bmson), "Bmson", Exts.Bmson);
        public static readonly string BmKind = Create(nameof(BmKind), "kind of Be Music", Exts.BmKindExts);
        public static readonly string BmMedia = Create(nameof(BmMedia), "Media files for BMS", Exts.BmMediaExts);
        public static readonly string BeMusic_Save = Join(Bms_Save, Pms_Save, B_Save, Bmson);
        public static readonly string BeMusic_Save_P = Join(Pms_Save, Bms_Save, B_Save, Bmson);

        public static readonly string BM3Score = Create(nameof(BM3Score), "BMSMaker3 Score File", Exts.BM3Score, Exts.Mid, Exts.Midi);
        public static readonly string BM3Score_Save = Join(Create(nameof(BM3Score_Save), "BMSMaker3 Score File", Exts.BM3Score), Midi);

        public static readonly string BM3Project = Create(nameof(BM3Project), "BMSMaker3 Porject File", Exts.BM3Project);

        public static readonly string Mascot = Create(nameof(Mascot), "Mascot Setting File", Exts.Mascot, Exts.Json);
        public static readonly string Mascot_Save = Join(Create(nameof(Mascot_Save), "Mascot Setting File", Exts.Mascot), Json);
    }
}
