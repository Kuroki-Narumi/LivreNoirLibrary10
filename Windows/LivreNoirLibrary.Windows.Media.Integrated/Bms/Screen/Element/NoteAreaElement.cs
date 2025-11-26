using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class NoteAreaElement : GroupElementBase
    {
        private readonly NoteArea _source;
        private readonly Dictionary<int, NoteLaneInfo> _lanes = [];
        private readonly List<(UIntBitmap Source, System.Drawing.Rectangle SourceRect, System.Drawing.Rectangle DestRect)> _children = [];
        private TextureData _barLine;

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

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            var timer = args.Timer;
            if (timer.TryGet(TimerId.Play_MusicStart, args.AbsoluteTime, out var relativeTime))
            {
                var texture = args.Textures;
                var notes = args.Notes;
                var highSpeed = args.HighSpeed;
                var children = _children;
                children.Clear();
                var lanes = _lanes;
                var areaHeight = DestHeight;
                var barTexture = _barLine;
                var width = DestWidth;
                foreach (var bar in notes.BarLines)
                {
                    AddChild(barTexture, 0, width, bar.RelativePosition);
                }
                foreach (var child in notes.VisibleChildren)
                {
                    if (lanes.TryGetValue(child.Lane, out var laneInfo))
                    {
                        var visualLength = child.VisualLength;
                        TextureData textureData;
                        if (visualLength is > 0)
                        {
                            AddChild(child.IsActive ? laneInfo.ActiveLongBody : laneInfo.LongBody, laneInfo.X, laneInfo.Width, child.CurrentOffset, visualLength);
                            textureData = laneInfo.LongTail;
                            AddChild(textureData, laneInfo.X, laneInfo.Width, child.CurrentOffset + visualLength, 0, laneInfo.Note.Height - textureData.Height);
                        }
                        textureData =
                            child.IsMine ? laneInfo.Mine :
                            (child.VisualLength is > 0) ? laneInfo.LongHead :
                            laneInfo.Note;
                        AddChild(textureData, laneInfo.X, laneInfo.Width, child.CurrentOffset);
                    }
                }

                void AddChild(TextureData data, double x, double width, double offset, double height = 0, double finalOffset = 0)
                {
                    if (texture.TryGetTexture(data, BmsTimer.GetFrameIndex(relativeTime, data), out var bitmap, out var sourceRect))
                    {
                        var h = height is 0 ? sourceRect.Height : height * areaHeight * highSpeed;
                        var y = areaHeight - areaHeight * offset * highSpeed - h - finalOffset;
                        var rect = new Rect(x, y, width, h).ToDrawingRect();
                        children.Add((bitmap, sourceRect, rect));
                    }
                }
            }
        }

        protected override void RenderChildren(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            foreach (var (source, sourceRect, targetRect) in _children.AsSpan())
            {
                buffer1.Resize(targetRect.Width, targetRect.Height);
                source.StretchCopy(buffer1, sourceRect, buffer2);
                target.Blend(buffer1, targetRect.Location, BlendMode.Alpha);
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
