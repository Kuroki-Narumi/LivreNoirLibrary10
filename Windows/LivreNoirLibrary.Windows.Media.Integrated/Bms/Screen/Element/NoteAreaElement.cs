using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class NoteAreaElement : GroupElementBase
    {
        public const double DefaultbaseHeight = 8;

        private readonly NoteArea _source;
        private readonly Dictionary<int, NoteLaneInfo> _lanes = [];
        private TextureData _barLine;
        private TextureData _judgeLine;
        private double _baseHeight;
        private readonly List<(UIntBitmap Source, Rectangle SourceRect, Rect DestRect)> _children = [];
        private readonly Dictionary<int, double> _lastNotes = [];

        public NoteAreaElement(NoteArea source) : base(source)
        {
            _source = source;
            foreach (var child in source.Children.AsSpan())
            {
                if (child is NoteLane lane)
                {
                    _lanes[lane.Lane] = new(lane);
                }
            }
        }

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            _barLine = GetTexture(_source.BarLine);
            _judgeLine = GetTexture(_source.JudgeLine);
            _baseHeight = skin.ResolveValue(_source.BaseHeight, provider, DefaultbaseHeight);
            foreach (var (_, child) in _lanes)
            {
                var source = child.Source;
                child.X = skin.ResolveValue(source.X, provider, 0d);
                child.Width = skin.ResolveValue(source.Width, provider, 0d);
                child.Note = GetTexture(source.Note);
                child.LongHead = GetTexture(source.LongHead);
                child.LongTail = GetTexture(source.LongTail);
                child.LongBody = GetTexture(source.LongBody);
                child.ActiveLongBody = GetTexture(source.ActiveLongBody);
                child.Mine = GetTexture(source.Mine);
            }

            TextureData GetTexture(ValueExpression? texture)
            {
                skin.TryGetTextureData(texture, provider, out var data);
                return data;
            }
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            var timer = args.Timer;
            var relativeTime = timer.Get(TimerId.Play_MusicStart, args.AbsoluteTime);
            var texture = args.Textures;
            var notes = args.Notes;
            var highSpeed = args.HighSpeed;
            var children = _children;
            children.Clear();
            var lastNotes = _lastNotes;
            lastNotes.Clear();
            var lanes = _lanes;
            var areaHeight = DestHeight;
            var barTexture = _barLine;
            var width = DestWidth;
            var baseHeight = _baseHeight;
            if (relativeTime is >= 0 && TryGetTexture(barTexture, out var source, out var sourceRect))
            {
                var h = (double)sourceRect.Height;
                Rect destRect = new(0, 0, width, h);
                foreach (var bar in notes.BarLines)
                {
                    destRect.Y = areaHeight - areaHeight * bar.RelativePosition * highSpeed - h;
                    children.Add((source, sourceRect, destRect));
                }
            }
            if (TryGetTexture(_judgeLine, out source, out sourceRect))
            {
                children.Add((source, sourceRect, new(0, areaHeight - baseHeight, width, baseHeight)));
            }
            if (relativeTime is >= 0)
            {
                foreach (var child in notes.VisibleChildren)
                {
                    var lane = child.Lane;
                    if (lanes.TryGetValue(lane, out var laneInfo))
                    {
                        var visualLength = child.VisualLength;
                        TextureData textureData;
                        if (visualLength is > 0)
                        {
                            AddChild(-1, child.IsActive ? laneInfo.ActiveLongBody : laneInfo.LongBody, laneInfo.X, laneInfo.Width, child.CurrentOffset, visualLength);
                            textureData = laneInfo.LongTail;
                            AddChild(lane + 1000, textureData, laneInfo.X, laneInfo.Width, child.CurrentOffset + visualLength, 0, laneInfo.Note.Height - textureData.Height);
                        }
                        textureData =
                            child.IsMine ? laneInfo.Mine :
                            (child.VisualLength is > 0) ? laneInfo.LongHead :
                            laneInfo.Note;
                        AddChild(lane, textureData, laneInfo.X, laneInfo.Width, Math.Max(child.CurrentOffset, 0), 0);
                    }
                }
            }

            bool TryGetTexture(in TextureData data, [MaybeNullWhen(false)] out UIntBitmap bitmap, out Rectangle sourceRect)
                => texture.TryGetTexture(data, BmsTimer.GetFrameIndex(relativeTime, data), out bitmap, out sourceRect);

            void AddChild(int lane, TextureData data, double x, double width, double visualOffset, double visualHeight, double finalOffset = 0)
            {
                if (texture.TryGetTexture(data, BmsTimer.GetFrameIndex(relativeTime, data), out var bitmap, out var sourceRect))
                {
                    var h = visualHeight is > 0 ? visualHeight * areaHeight * highSpeed : sourceRect.Height;
                    var y = areaHeight - areaHeight * visualOffset * highSpeed - h - finalOffset;
                    if (lane is not -1)
                    {
                        if (lastNotes.TryGetValue(lane, out var previous) && previous - y is < 0.5)
                        {
                            return;
                        }
                        lastNotes[lane] = y;
                    }
                    children.Add((bitmap, sourceRect, new(x, y, width, h)));
                }
            }
        }

        protected override void RenderChildren(in RenderArgs args)
        {
            var colorCorrection = args.ColorCorrection;
            foreach (var (source, sourceRect, (x, y, width, height)) in _children.AsSpan())
            {
                RenderSource(args, source, sourceRect, x, y, width, height, BlendMode.Alpha, colorCorrection);
            }
        }

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
    }
}
