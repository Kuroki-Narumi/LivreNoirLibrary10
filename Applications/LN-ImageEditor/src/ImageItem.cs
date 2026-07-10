using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;

namespace LivreNoir.ImageEditor
{
    public class ImageItemList : ObservableSortedList<string, ImageItem>
    {
        public ImageItemList() : base(StringExtensions.NaturalOrderComparer) { }

        protected override string GetKey(ImageItem item) => item.FullPath;

        public void Reload()
        {
            foreach (var item in _list)
            {
                item.Reload();
            }
            GC.Collect(1);
        }
    }

    public partial class ImageItem(string path) : ObservableObjectBase
    {
        public BitmapImage Image { get; private set => SetValue(ref field, value); } = CreateImage(path);
        public string FullPath { get; } = path;
        public string Filename => Path.GetFileName(FullPath);
        public Size Size => new(Image.PixelWidth, Image.PixelHeight);

        private static BitmapImage CreateImage(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.EndInit();
            bitmap.Freeze(); // Freeze to make it thread-safe
            return bitmap;
        }

        public void Reload()
        {
            Image = CreateImage(FullPath);
            SendPropertyChanged(nameof(Size));
        }
    }
}
