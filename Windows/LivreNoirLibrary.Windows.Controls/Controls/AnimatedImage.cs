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
        [DependencyProperty(SetterScope = Scope.Private)]
        private int _maxFrame = 0;

        private Int32Animation? _animation;
        private BitmapDecoder? _decoder;
        private readonly List<int> _timings = [];

        public new ImageSource? Source { get => GetValue(SourceProperty) as ImageSource; set => SetValue(SourceProperty, value); }

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
                if (_decoder is not null && _animation is not null)
                {
                    BeginAnimation(FrameProperty, _animation);
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
                base.Source = _decoder?.Frames[_timings[value]];
            }
        }

        public void ClearAnimation()
        {
            IsAnimating = false;
            _animation = null;
            _decoder = null;
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
            if (decoder is GifBitmapDecoder && decoder.Frames.Count > 1)
            {
                _decoder = decoder;
                MaxFrame = decoder.Frames.Count;
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
            if (_decoder is null) { return; }
            var timings = _timings;
            timings.Clear();
            int time = 0;
            var frames = _decoder.Frames;
            for (int i = 0; i < frames.Count; i++)
            {
                var delay = frames[i].Metadata is BitmapMetadata meta ? (ushort)meta.GetQuery("/grctlext/Delay") : DefaultFrameDelay;
                time += delay;
                for (ushort j = 0; j < delay; j++)
                {
                    timings.Add(i);
                }
            }
            _animation = new Int32Animation()
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
