using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Bmson
{
    public record HeaderInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("subtitle")]
        public string? SubTitle { get; set; }

        [JsonPropertyName("artist")]
        public string? Artist { get; set; }

        [JsonPropertyName("subartists")]
        public List<string>? SubArtists { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("mode_hint")]
        public KeyType ModeHint { get; set; } = Constants.DefaultModeHint;

        [JsonPropertyName("chart_name")]
        public string ChartName { get; set; } = "";

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("init_bpm")]
        public decimal InitialBpm { get; set; }

        [JsonIgnore]
        public double JudgeRank { get; set; } = Constants.DefaultJudgeRank;

        [JsonPropertyName("judge_rank")]
        public double? JudgeRunk_Nullable { get => JudgeRank is Constants.DefaultJudgeRank ? null : JudgeRank; set => JudgeRank = value ?? Constants.DefaultJudgeRank; }

        [JsonIgnore]
        public double Total { get; set; } = Constants.DefaultTotal;
        [JsonPropertyName("total")]
        public double? Total_Nullable{ get => Total is Constants.DefaultTotal ? null : Total; set => Total = value ?? Constants.DefaultTotal; }

        [JsonPropertyName("back_image")]
        public string? BackImage { get; set; }

        [JsonPropertyName("eyecatch_image")]
        public string? EyecatchImage { get; set; }

        [JsonPropertyName("banner_image")]
        public string? BannerImage { get; set; }

        [JsonPropertyName("preview_music")]
        public string? PreviewMusic { get; set; }

        [JsonPropertyName("resolution")]
        public long Resolution { get; set; } = Constants.DefaultResolution;

        [JsonIgnore]
        public LongNoteMode LnType { get; set; }
        [JsonPropertyName("ln_type")]
        public LongNoteMode? LnType_Nullable { get => LnType is LongNoteMode.Auto ? null : LnType; set => LnType = value ?? LongNoteMode.Auto; }
    }
}
