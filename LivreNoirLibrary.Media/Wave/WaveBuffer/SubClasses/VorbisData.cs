using System;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Ogg.Vorbis;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Wave
{
    public class VorbisData : WaveBuffer, IAudioMetaData
    {
        private readonly VorbisCommentEditor _editor = new();

        public IdHeader IdHeader => _editor.IdHeader;
        public VorbisCommentList Comments => _editor.Comments;

        public override void Clear()
        {
            base.Clear();
            _editor.Comments.Clear();
        }

        public override void WriteMetaTags(IMetaTag format, IMetaTag stream)
        {
            Comments.SetLoopMarkers(_markers);
            stream.SetMetaTags(Comments.AsMetaTags());
            stream.SetMetaTag(StandardCommentKeys.Encoder, nameof(LivreNoirLibrary));
        }

        public void Load(Stream stream)
        {
            Clear();
            var pos = stream.Position;
            _editor.Load(stream, true);
            stream.Position = pos;
            using AudioDecoder decoder = new(stream, true);
            Load(decoder);
            _editor.Comments.GetLoopMarkers(_markers);
        }

        public void Save(string path, bool ext = true) => General.Save(path, Dump, ext ? ExtRegs.Vorbis : null, Exts.Vorbis);

        public void Dump(Stream stream)
        {
            Comments.SetLoopMarkers(_markers);
            _editor.Dump(stream);
        }

        public static void SaveAsVorbis(string path, WaveBuffer source, int bitRate = 320, bool ext = true)
        {
            if (source is VorbisData v)
            {
                v.Save(path, ext);
                return;
            }
            General.PrepareSave(ref path, ext ? ExtRegs.Vorbis : null, Exts.Vorbis);
            using var sw = ExStopwatch.SaveProcessTime(path, General.Tweet);
            using AudioEncoder encoder = new(path, new(source.SampleRate, source.Channels, bitRate));
            var markers = source.Markers;
            if (markers.TryGetByName(Marker.LoopStartName, out var marker))
            {
                var pos = marker.Position;
                encoder.StreamInfo.SetMetaTag(StandardCommentKeys.LoopStart, $"{pos}");
                if (markers.TryGetByName(Marker.LoopEndName, out marker))
                {
                    encoder.StreamInfo.SetMetaTag(StandardCommentKeys.LoopLength, $"{marker.Position - pos}");
                }
            }
            encoder.StreamInfo.SetMetaTag(StandardCommentKeys.Encoder, nameof(LivreNoirLibrary));
            encoder.Write(source.Data);
            encoder.Flush();
        }

        protected override void WriteJsonMetaData(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WritePropertyName("id header");
            IdHeader.WriteJson(writer, options);

            writer.WritePropertyName("comments");
            Comments.WriteJson(writer, options);
        }

        public double Tempo
        {
            get => Comments.TryGetDouble(StandardCommentKeys.Tempo, out var v) ? v : IAudioMetaData.DefaultTempo;
            set => Comments.Set(StandardCommentKeys.Tempo, $"{value}");
        }

        public bool IsTempoSet() => Comments.TryGetDouble(StandardCommentKeys.Tempo, out _);

        public string? Genre { get => Comments.GetOrNull(StandardCommentKeys.Genre); set => Comments.Set(StandardCommentKeys.Genre, value); }
        public string? Title { get => Comments.GetOrNull(StandardCommentKeys.Title); set => Comments.Set(StandardCommentKeys.Title, value); }
        public string? Artist { get => Comments.GetOrNull(StandardCommentKeys.Artist); set => Comments.Set(StandardCommentKeys.Artist, value); }
        public string? Copyright { get => Comments.GetOrNull(StandardCommentKeys.Copyright); set => Comments.Set(StandardCommentKeys.Copyright, value); }
    }
}
