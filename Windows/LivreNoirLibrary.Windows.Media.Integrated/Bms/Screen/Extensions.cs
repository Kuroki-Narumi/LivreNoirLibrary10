using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
