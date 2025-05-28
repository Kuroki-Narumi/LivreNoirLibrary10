using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class AnimatedImage : Image
    {
        public const ushort DefaultFrameDelay = 3;

        public static new readonly DependencyProperty SourceProperty = PropertyUtils.RegisterTwoWay<ImageSource>(typeof(AnimatedImage), callback: OnSourceChanged);

        protected static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AnimatedImage)!.OnSourceChanged(e.NewValue as ImageSource);
        }

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private bool _isAnimating = false;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _frame = -1;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private int _maxFrame = 0;

        private Int32Animation? _gif_animation;
        private GifBitmapDecoder? _gif_decoder;
        private readonly List<int> _timings = [];

        public new ImageSource? Source
        {
            get => GetValue(SourceProperty) as ImageSource;
            set => SetValue(SourceProperty, value);
        }

        private void OnSourceChanged(ImageSource? source)
        {
            ClearAnimation();
            if (source is BitmapImage bitmap && CreateGifDecoder(bitmap))
            {
                CreateAnimation();
                Frame = 0;
                IsAnimating = true;
            }
            else
            {
                base.Source = source;
            }
        }

        private void OnIsAnimatingChanged(bool value)
        {
            if (value)
            {
                if (_gif_decoder is not null && _gif_animation is not null)
                {
                    BeginAnimation(FrameProperty, _gif_animation);
                }
                else
                {
                    SetValue(IsAnimatingProperty, false);
                }
            }
            else
            {
                BeginAnimation(FrameProperty, null);
            }
        }

        private void OnFrameChanged(int value)
        {
            if ((uint)value < (uint)_timings.Count)
            {
                base.Source = _gif_decoder?.Frames[_timings[value]];
            }
        }

        public void ClearAnimation()
        {
            IsAnimating = false;
            _gif_animation = null;
            _gif_decoder = null;
            _timings.Clear();
        }

        private bool CreateGifDecoder(BitmapImage bitmap)
        {
            BitmapDecoder? decoder = null;
            if (bitmap.UriSource is not null)
            {
                decoder = BitmapDecoder.Create(bitmap.UriSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            else if (bitmap.StreamSource is not null)
            {
                decoder = BitmapDecoder.Create(bitmap.StreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            }
            if (decoder is GifBitmapDecoder d && d.Frames.Count > 1)
            {
                _gif_decoder = d;
                MaxFrame = _gif_decoder.Frames.Count;
                return true;
            }
            else
            {
                MaxFrame = 0;
                return false;
            }
        }

        private void CreateAnimation()
        {
            if (_gif_decoder is null) { return; }
            var timings = _timings;
            timings.Clear();
            int time = 0;
            var frames = _gif_decoder.Frames;
            for (int i = 0; i < frames.Count; i++)
            {
                var delay = frames[i].Metadata is BitmapMetadata meta ? (ushort)meta.GetQuery("/grctlext/Delay") : DefaultFrameDelay;
                time += delay;
                for (ushort j = 0; j < delay; j++)
                {
                    timings.Add(i);
                }
            }
            _gif_animation = new Int32Animation()
            {
                From = 0,
                To = time,
                Duration = TimeSpan.FromMilliseconds(time * 10),
                FillBehavior = FillBehavior.Stop,
                RepeatBehavior = RepeatBehavior.Forever,
            };
        }
    }
}
