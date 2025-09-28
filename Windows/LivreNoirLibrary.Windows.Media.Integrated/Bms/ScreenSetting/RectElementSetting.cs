using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Drawing;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Media.Bms
{
    public class RectElementSetting : ElementSetting
    {
        public Rectangle Rect
        {
            get;
            set
            {
                var (ox, oy, ow, oh) = field;
                var xChanged = ox != value.X;
                var yChanged = oy != value.Y;
                var wChanged = ow != value.Width;
                var hChanged = oh != value.Height;
                var locationChanged = xChanged || yChanged;
                var sizeChagned = wChanged || hChanged;
                if (locationChanged || sizeChagned)
                {
                    field = value;
                    SendPropertyChanged();
                    if (xChanged)
                    {
                        SendPropertyChanged(nameof(X));
                    }
                    if (yChanged)
                    {
                        SendPropertyChanged(nameof(Y));
                    }
                    if (wChanged)
                    {
                        SendPropertyChanged(nameof(Width));
                    }
                    if (hChanged)
                    {
                        SendPropertyChanged(nameof(Height));
                    }
                    if (locationChanged)
                    {
                        SendPropertyChanged(nameof(Location));
                    }
                    if (sizeChagned)
                    {
                        SendPropertyChanged(nameof(Size));
                    }
                    OnRectChanged(value);
                }
            }
        }

        protected virtual void OnRectChanged(Rectangle newRect) { }

        [JsonIgnore]
        public int X { get => Rect.X; set => Rect = new(value, Rect.Y, Rect.Width, Rect.Height); }
        [JsonIgnore]
        public int Y { get => Rect.Y; set => Rect = new(Rect.X, value, Rect.Width, Rect.Height); }
        [JsonIgnore]
        public int Width { get => Rect.Width; set => Rect = new(Rect.X, Rect.Y, value, Rect.Height); }
        [JsonIgnore]
        public int Height { get => Rect.Height; set => Rect = new(Rect.X, Rect.Y, Rect.Width, value); }
        [JsonIgnore]
        public Point Location { get => Rect.Location; set => Rect = new(value, Rect.Size); }
        [JsonIgnore]
        public Size Size { get => Rect.Size; set => Rect = new(Rect.Location, value); }
    }
}
