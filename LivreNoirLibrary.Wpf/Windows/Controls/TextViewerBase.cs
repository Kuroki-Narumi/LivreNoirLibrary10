using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class TextViewerBase : Control
    {
        public const bool DefaultUseNormalText = true;

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private bool _useNormalText = DefaultUseNormalText;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Brush? _textOutline;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _textOutlineThickness;

        protected static void OnNeedRefreshPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TextViewerBase)?.ReserveRefresh();
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            _text_cache.Clear();
            base.OnDpiChanged(oldDpi, newDpi);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == FontSizeProperty)
            {
                InvalidateVisual();
            }
            else if (IsTextProperty(e.Property))
            {
                _text_cache.Clear();
                InvalidateVisual();
            }
        }

        protected virtual bool IsTextProperty(DependencyProperty prop)
        {
            return prop == FlowDirectionProperty ||
                   prop == FontFamilyProperty ||
                   prop == FontStyleProperty ||
                   prop == FontWeightProperty ||
                   prop == FontStretchProperty ||
                   prop == ForegroundProperty;
        }

        private readonly Dictionary<double, Dictionary<string, FormattedText>> _text_cache = [];
        private bool _need_refresh;
        protected readonly RenderTextOption _text_option = new();
        protected double _fontSize;

        public void ReserveRefresh()
        {
            if (!_need_refresh)
            {
                _need_refresh = true;
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _fontSize = FontSize;
            _text_option.Foreground = Foreground;
            _text_option.Stroke = MediaUtils.GetPen(TextOutline, TextOutlineThickness);
            _text_option.UseDrawText = UseNormalText;
            if (_need_refresh)
            {
                _need_refresh = false;
                Refresh();
            }
            base.OnRender(drawingContext);
        }

        protected virtual void Refresh() { }

        protected void RenderText(DrawingContext dc, double x, double y, string? text, double size = double.NaN, VerticalAlignment va = VerticalAlignment.Center, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _text_option.X = x;
                _text_option.Y = y;
                _text_option.VerticalAlignment = va;
                _text_option.HorizontalAlignment = ha;
                MediaUtils.RenderText(dc, GetFormattedText(text, size.Validate(_fontSize)), _text_option);
            }
        }

        protected FormattedText GetFormattedText(string text, double size)
        {
            if (!_text_cache.TryGetValue(size, out var dic))
            {
                dic = [];
                _text_cache.Add(size, dic);
            }
            if (!dic.TryGetValue(text, out var ft))
            {
                ft = this.CreateFormattedText(text, new(this) { FontSize = size });
                dic.Add(text, ft);
            }
            return ft;
        }
    }
}
