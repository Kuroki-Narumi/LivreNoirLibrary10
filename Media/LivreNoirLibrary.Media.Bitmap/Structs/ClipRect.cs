using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media
{
    [TypeConverter(typeof(ClipRectTypeConverter))]
    [JsonConverter(typeof(ClipRectJsonConverter))]
    public class ClipRect : ObservableObjectBase
    {
        public double X { get; set => SetValue(ref field, value, [nameof(AbsoluteX)]); }
        public double Y { get; set => SetValue(ref field, value, [nameof(AbsoluteY)]); }
        public double Width { get; set => SetValue(ref field, value, [nameof(AbsoluteWidth)]); }
        public double Height { get; set => SetValue(ref field, value, [nameof(AbsoluteHeight)]); }

        public double RelativeWidth { get; set => SetValue(ref field, value, [nameof(AbsoluteX), nameof(AbsoluteWidth)]); }
        public double RelativeHeight { get; set => SetValue(ref field, value, [nameof(AbsoluteY), nameof(AbsoluteHeight)]); }

        public double AbsoluteX { get => X * RelativeWidth; set => X = value / (RelativeWidth > 0 ? RelativeWidth : 1); }
        public double AbsoluteY { get => Y * RelativeHeight; set => Y = value / (RelativeHeight > 0 ? RelativeHeight : 1); }
        public double AbsoluteWidth { get => Width * RelativeWidth; set => Width = value / (RelativeWidth > 0 ? RelativeWidth : 1); }
        public double AbsoluteHeight { get => Height * RelativeHeight; set => Height = value / (RelativeHeight > 0 ? RelativeHeight : 1); }

        public ClipRect() { }

        public ClipRect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString() => $"{X}, {Y}, {Width}, {Height}";
    }
}
