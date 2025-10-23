using System;
using System.IO;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BmsTextureCache : TextureCache
    {
        public void LoadBms(IBmsData data, string directory)
        {
            void Set(string? relativePath, string cacheKey)
            {
                if (!string.IsNullOrEmpty(relativePath) && FileUtils.TryGetImageFileName(Path.GetFullPath(relativePath, directory), out var path))
                {
                    var bitmap = new BitmapImage(new Uri(path));
                    _sourceCache[cacheKey] = bitmap;
                }
            }
            Set(data.StageFile, Texture.Key_StageFile);
            Set(data.Banner, Texture.Key_Banner);
            Set(data.BackBmp, Texture.Key_BackBmp);
        }
    }
}
