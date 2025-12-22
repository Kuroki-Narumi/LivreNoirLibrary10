using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public static class Extensions
    {
        public static void SetBmsOptions(this IDictionary<string, string> dict, IBmsViewModel viewModel)
        {
            dict["StageFile Defined"] = viewModel.IsHeaderDefined(HeaderType.StageFile).ToString();
            dict["Banner Defined"] = viewModel.IsHeaderDefined(HeaderType.Banner).ToString();
            dict["BackBmp Defined"] = viewModel.IsHeaderDefined(HeaderType.BackBmp).ToString();
        }

        public static void SetBmsVariables(this IDictionary<string, string> dict, IBmsViewModel viewModel)
        {
            var title = viewModel.Title ?? "";
            var subTitle = viewModel.SubTitle ?? "";
            dict["Title"] = title;
            dict["SubTitle"] = subTitle;
            dict["FullTitle"] = string.IsNullOrEmpty(title) ? subTitle : (string.IsNullOrEmpty(subTitle) ? title : $"{title} {subTitle}");

            var artist = viewModel.Artist ?? "";
            var subArtist = viewModel.SubArtist ?? "";
            dict["Artist"] = artist;
            dict["SubArtist"] = subArtist;
            dict["FullArtist"] = string.IsNullOrEmpty(artist) ? subArtist : (string.IsNullOrEmpty(subArtist) ? artist : $"{artist} ({subArtist})");

            foreach (var key in _generalHeaders)
            {
                dict[key.ToString()] = viewModel.GetTextHeader(key, "");
            }

            dict["TotalNotes"] = viewModel.CurrentTimeline.GetNoteCount().ToString();
        }

        public static void SetBmsTexture(this TextureCache cache, IBmsViewModel viewModel, string basePath)
        {
            cache.Set(Texture.Key_StageFile, viewModel.StageFile, basePath);
            cache.Set(Texture.Key_Banner, viewModel.Banner, basePath);
            cache.Set(Texture.Key_BackBmp, viewModel.BackBmp, basePath);
            cache.Set(Texture.Key_Bmp00, viewModel.GetDefValue(DefType.Bmp, 0), basePath);
        }

        static readonly HeaderType[] _generalHeaders =
        [
            HeaderType.Genre,
            HeaderType.Bpm,
            HeaderType.PlayLevel,
            HeaderType.Difficulty,
            HeaderType.Rank,
            HeaderType.Total,
            HeaderType.StageFile,
            HeaderType.Banner,
            HeaderType.BackBmp,
            HeaderType.LnMode,
            HeaderType.DefExRank,
            HeaderType.Comment,
        ];

        public static void SetPlayInfos(this IDictionary<string, string> dict, ITimeCounter timingList)
        {
            dict["MinBpm"] = timingList.MinTempo.ToString();
            dict["MaxBpm"] = timingList.MaxTempo.ToString();
            dict["AverageBpm"] = timingList.AverageTempo.ToString();
            dict["MainBpm"] = timingList.MainTempo.ToString();
            dict["MainTimeBpm"] = timingList.MainTimeTempo.ToString();
            dict["TotalTime"] = TimeSpan.FromSeconds(timingList.LastSoundTime).AutoFormat_Minutes();
        }

        public static void UpdateCurrentInfos(this IDictionary<string, string> dict, in UpdateArgs args)
        {
            var timer = args.Timer;
            var absTime = args.AbsoluteTime;
            var timings = args.Timings;

            if (timer.TryGet(TimerId.Play_MusicStart, absTime, out var currentTime))
            {
                dict["CurrentTime"] = TimeSpan.FromSeconds(currentTime).AutoFormat_Minutes();
                dict["CurrentBpm"] = timings.Time2Tempo(currentTime).ToString();
            }
            else
            {
                dict["CurrentTime"] = "0:00";
                dict["CurrentBpm"] = dict["Bpm"];
            }
            if (dict.TryGetValue("TotalTime", out var tt))
            {
                dict["FullTime"] = $"{dict["CurrentTime"]}/{tt}";
            }
            else
            {
                dict["FullTime"] = dict["CurrentTime"];
            }

            var score = args.ScoreManager;
            dict["CurrentCombo"] = score.Combo.ToString();
            dict["MaxCombo"] = score.MaxCombo.ToString();
            dict["CurrentScore"] = score.Score.ToString();
            dict["CurrentGauge"] = score.Gauge.ToString();
        }
    }
}