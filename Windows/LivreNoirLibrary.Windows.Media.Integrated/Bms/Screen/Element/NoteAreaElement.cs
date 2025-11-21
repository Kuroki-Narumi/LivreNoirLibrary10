using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteAreaElement : ScreenElementBase
    {
        private readonly NoteArea _skinInfo;
        private readonly Dictionary<int, NoteLaneInfo> _lanes = [];
        private readonly List<(CroppedBitmap, Rect)> _children = [];
        private TextureData _barLine;

        private class NoteLaneInfo(NoteLane source)
        {
            public NoteLane Source { get; } = source;
            public int Lane { get; } = source.Lane;
            public double X { get; set; }
            public double Width { get; set; }
            public TextureData Note { get; set; }
            public TextureData LongHead { get; set; }
            public TextureData LongTail { get; set; }
            public TextureData LongBody { get; set; }
            public TextureData ActiveLongBody { get; set; }
            public TextureData Mine { get; set; }
        }

        public NoteAreaElement(NoteArea source) : base(source)
        {
            _skinInfo = source;
            foreach (var child in source.Children)
            {
                if (child is NoteLane n)
                {
                    _lanes[n.Lane] = new(n);
                }
            }
            ClipToBounds = true;
        }

        public void LoadDestination(Skin skin, IVariableProvider? provider)
        {
            IScreenElementExtension.LoadDestination(this, skin, provider);
            _barLine = GetTexture(_skinInfo.BarLine);
            foreach (var (_, child) in _lanes)
            {
                var source = child.Source;
                if (skin.TryResolveValue<double>(source.X, provider, out var v))
                {
                    child.X = v;
                }
                if (skin.TryResolveValue(source.Width, provider, out v))
                {
                    child.Width = v;
                }
                child.Note = GetTexture(source.Note);
                child.LongHead = GetTexture(source.LongHead);
                child.LongTail = GetTexture(source.LongTail);
                child.LongBody = GetTexture(source.LongBody);
                child.ActiveLongBody = GetTexture(source.ActiveLongBody);
                child.Mine = GetTexture(source.Mine);
            }
            TextureData GetTexture(string? texture)
            {
                skin.TryGetTexture(texture, provider, out var data);
                return data;
            }
        }

        public void Update(NoteElementCollection source, BmsTimer timer, long absoluteTick, TextureCache texture, double highSpeed)
        {
            if (timer.TryGet(TimerId.Play_MusicStart, absoluteTick, out var relativeTick))
            {
                var children = _children;
                children.Clear();
                var lanes = _lanes;
                ViewModel.Update(timer, absoluteTick);
                var areaHeight = ViewModel.DestHeight;
                var barTexture = _barLine;
                var width = Width;
                foreach (var bar in source.BarLines)
                {
                    AddChild(barTexture, 0, width, bar.RelativePosition);
                }
                foreach (var child in source.VisibleChildren)
                {
                    if (lanes.TryGetValue(child.Lane, out var laneInfo))
                    {
                        var textureData =
                            child.IsMine ? laneInfo.Mine :
                            (child.VisualLength is > 0) ? laneInfo.LongHead :
                            laneInfo.Note;
                        if (child.VisualLength is > 0)
                        {
                            AddChild(child.IsActive ? laneInfo.ActiveLongBody : laneInfo.LongBody, laneInfo.X, laneInfo.Width, child.CurrentOffset, child.VisualLength);
                        }
                        AddChild(textureData, laneInfo.X, laneInfo.Width, child.CurrentOffset);
                        if (child.VisualLength is > 0)
                        {
                            AddChild(laneInfo.LongTail, laneInfo.X, laneInfo.Width, child.CurrentOffset + child.VisualLength);
                        }
                    }
                }
                InvalidateVisual();

                void AddChild(TextureData data, double x, double width, double offset, double height = 0)
                {
                    if (texture.GetBitmap(data, BmsTimer.GetFrameIndex(relativeTick, data), out _) is { } bitmap)
                    {
                        var h = height is 0 ? bitmap.Height : height * areaHeight * highSpeed;
                        var y = areaHeight - areaHeight * offset * highSpeed - h;
                        Rect rect = new(x, y, width, h);
                        children.Add((bitmap, rect));
                    }
                }
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            foreach (var (bitmap, rect) in _children.AsSpan())
            {
                drawingContext.DrawImage(bitmap, rect);
            }
        }
    }
}
