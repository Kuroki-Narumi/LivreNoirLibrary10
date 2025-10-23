using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.Windows.Media.Effects;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BgaImageSource : ObservableObjectBase
    {
        private readonly MediaCacheCollection _cache = new();
        private readonly SolidColorBrush _background = new() { Color = Color.FromRgb(0, 0, 0) };
        private readonly TransparentConverter _transparent = new();

        public Color BackgroundColor
        {
            get => _background.Color; 
            set
            {
                _background.Color = value;
                SendPropertyChanged();
            }
        }

        public Color TransparentColor
        {
            get => _transparent.Color;
            set
            {
                _transparent.Color = value;
                SendPropertyChanged();
            }
        }

        public bool ShowMissLayer { get; set => SetValue(ref field, value); }
        public long MissLayerDisplayTime { get; set => SetValue(ref field, value); }

        public VisualBrush VisualBrush { get; private set => SetValue(ref field, value); }
        public BitmapSource? Source_Base { get; private set => SetValue(ref field, value); }
        public BitmapSource? Source_Layer1 { get; private set => SetValue(ref field, value); }
        public BitmapSource? Source_Layer2 { get; private set => SetValue(ref field, value); }
        public BitmapSource? Source_Poor { get; private set => SetValue(ref field, value); }

        public BgaImageSource()
        {
            Canvas bga = new()
            {
                Background = _background,
                Width = 256,
                Height = 256,
            };
            DoubleLimitConverter conv = new() { Maximum = 256 };
            void CreateImage(string sourcePath)
            {
                Image image = new()
                {
                    Effect = _transparent,
                    Stretch = Stretch.Uniform,
                };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                image.SetBinding(FrameworkElement.WidthProperty, new Binding("Source.Width") { Converter = conv, RelativeSource = RelativeSource.Self });
                image.SetBinding(FrameworkElement.HeightProperty, new Binding("Source.Height") { Converter = conv, RelativeSource = RelativeSource.Self });
                image.SetBinding(Image.SourceProperty, new Binding(sourcePath) { Source = this });
                bga.Children.Add(image);
            }
            CreateImage(nameof(Source_Base));
            CreateImage(nameof(Source_Layer1));
            CreateImage(nameof(Source_Layer2));
            CreateImage(nameof(Source_Poor));

            VisualBrush = new()
            {
                Visual = bga
            };
        }

        public void Clear()
        {
            _cache.Clear();
            Source_Base = Source_Layer1 = Source_Layer2 = Source_Poor = null;
        }

        public void Update(TimingList timings, BmsTimer timer, long absoluteTick)
        {
            var cache = _cache;
            if (timer.TryGet(TimerId.Play_MusicStart, absoluteTick, out var currentTick))
            {
                if (timings.TryGetBgaInfo(Channel.Bga_Base, currentTick, out var startTick, out var path))
                {
                    Source_Base = cache.GetBitmap(path, currentTick - startTick);
                }
                if (timings.TryGetBgaInfo(Channel.Bga_Layer1, currentTick, out startTick, out path))
                {
                    Source_Layer1 = cache.GetBitmap(path, currentTick - startTick);
                }
                if (timings.TryGetBgaInfo(Channel.Bga_Layer2, currentTick, out startTick, out path))
                {
                    Source_Layer2 = cache.GetBitmap(path, currentTick - startTick);
                }
                Source_Poor = ShowMissLayer && 
                    timer.TryGet(TimerId.Play_Miss, absoluteTick, out var elapsed) && 
                    (elapsed < MissLayerDisplayTime) && 
                    timings.TryGetBgaInfo(Channel.Bga_Poor, currentTick, out startTick, out path)
                    ? cache.GetBitmap(path, currentTick - startTick) : null;
            }
            else
            {
                Source_Base = Source_Layer1 = Source_Layer2 = Source_Poor = null;
            }
        }
    }
}
