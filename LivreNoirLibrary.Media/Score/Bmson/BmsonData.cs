using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bmson
{
    public record BmsonData
    {
        public static BmsonData Open(string path) => General.Open(path, Json.Load<BmsonData>);

        public void Save(string path, bool pretty = true, bool ext = true)
        {
            General.Save(path, s => Dump(s, pretty), ext ? ExtRegs.Bmson : null, Exts.Bmson);
        }

        public void Dump(Stream stream, bool pretty = true)
        {
            Json.Dump(stream, this, pretty);
        }

        /// <summary>
        /// Specifies the version of this bmson.
        /// </summary>
        [JsonPropertyName("version")]
        public Version Version { get; set; } = Constants.DefaultVersion;

        /// <summary>
        /// information, e.g. title, artist, …
        /// </summary>
        [JsonPropertyName("info")]
        public HeaderInfo Info { get; set; } = new();

        /// <summary>
        /// location of bar-lines in pulses
        /// </summary>
        [JsonPropertyName("lines")]
        public List<BarLine>? BarList { get; set; }

        /// <summary>
        /// bpm changes
        /// </summary>
        [JsonPropertyName("bpm_events")]
        public List<Bpm>? BpmList { get; set; }

        /// <summary>
        /// stop events
        /// </summary>
        [JsonPropertyName("stop_events")]
        public List<Stop>? StopList { get; set; }

        /// <summary>
        /// note data
        /// </summary>
        [JsonPropertyName("sound_channels")]
        public List<SoundChannel> SoundList { get; set; } = [];

        /// <summary>
        /// bga data
        /// </summary>
        [JsonPropertyName("bga")]
        public BgaInfo? BgaInfo { get; set; }

        /// <summary>
        /// scroll data
        /// </summary>
        [JsonPropertyName("scroll_events")]
        public List<RateEvent>? ScrollList { get; set; }

        /// <summary>
        /// speed data
        /// </summary>
        [JsonPropertyName("speed_events")]
        public List<RateEvent>? SpeedList { get; set; }
    }
}
