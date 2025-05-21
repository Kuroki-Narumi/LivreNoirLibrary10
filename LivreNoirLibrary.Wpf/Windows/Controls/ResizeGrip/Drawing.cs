using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Windows.Data;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ResizeGrip
    {
        public const double GripSize = 20;
        public const double GripThickness = 5;
        private static readonly DoubleLinearConverter LengthConverter = new() { Offset = -GripSize * 2, Minimum = 0 };
        private static readonly DoubleAnimation Animation_Show = GetAnimation(1.0);
        private static readonly DoubleAnimation Animation_Hide = GetAnimation(0.0);

        private static DoubleAnimation GetAnimation(double to)
        {
            DoubleAnimation ani = new() { To = to, Duration = TimeSpan.FromSeconds(0.1) };
            ani.Freeze();
            return ani;
        }

        private void UpdateBrush()
        {
            Binding back = new(nameof(GripBackground)) { Source = this };
            Binding fore = new(nameof(GripForeground)) { Source = this };
            // Left & Right
            var brush = GetVerticalGripBrush(back, fore);
            _grips[4].Fill = brush;
            _grips[6].Fill = brush;
            // Top & Bottom
            brush = GetHorizontalGripBrush(back, fore);
            _grips[8].Fill = brush;
            _grips[2].Fill = brush;
            // Lower Left
            _grips[1].Fill = GetCornerBrush(back, fore, 0, 16, 4, 20, 2, -2);
            // Lower Right
            _grips[3].Fill = GetCornerBrush(back, fore, 20, 16, 16, 20, -2, -2);
            // Upper Left
            _grips[7].Fill = GetCornerBrush(back, fore, 0, 4, 4, 0, 2, 2);
            // Upper Right
            _grips[9].Fill = GetCornerBrush(back, fore, 20, 4, 16, 0, -2, 2);
        }

        private static DrawingBrush GetVerticalGripBrush(Binding background, Binding foreground)
        {
            DrawingGroup dg = new();
            // background
            GeometryDrawing gd = new() { Geometry = MediaUtils.Freeze(new RectangleGeometry(new(0, 0, 5, 1))) };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, background);
            dg.Children.Add(gd);
            // foreground
            gd = new() { Geometry = MediaUtils.CreateGeometry("M1,0 h1 v1 h-1 Z M3,0 h1 v1 h-1 Z") };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, foreground);
            dg.Children.Add(gd);

            DrawingBrush brush = new(dg)
            {
                Viewport = new(0, 0, GripThickness, 1),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
            return brush;
        }

        private static DrawingBrush GetHorizontalGripBrush(Binding background, Binding foreground)
        {
            DrawingGroup dg = new();
            // background
            GeometryDrawing gd = new() { Geometry = MediaUtils.Freeze(new RectangleGeometry(new(0, 0, 1, 5))) };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, background);
            dg.Children.Add(gd);
            // foreground
            gd = new() { Geometry = MediaUtils.CreateGeometry("M0,1 h1 v1 h-1 Z M0,3 h1 v1 h-1 Z") };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, foreground);
            dg.Children.Add(gd);

            DrawingBrush brush = new(dg)
            {
                Viewport = new(0, 0, 1, GripThickness),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
            return brush;
        }

        private static DrawingBrush GetCornerBrush(Binding background, Binding foreground, double vx, double vy, double hx, double hy, double dx, double dy)
        {
            DrawingGroup dg = new();
            // background
            GeometryDrawing gd = new() { Geometry = MediaUtils.Freeze(new RectangleGeometry(new(0, 0, 20, 20))) };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, background);
            dg.Children.Add(gd);
            // foreground
            gd = new() { Geometry = CreateDiagonalStripes(vx, vy, hx, hy, dx, dy) };
            BindingOperations.SetBinding(gd, GeometryDrawing.BrushProperty, foreground);
            dg.Children.Add(gd);

            DrawingBrush brush = new(dg)
            {
                Viewport = new(0, 0, GripSize, GripSize),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
            return brush;
        }

        private void InitElement(UIElement element)
        {
            element.Opacity = 0;
            element.MouseLeftButtonDown += OnMouseLeftButtonDown_Grip;
            element.MouseEnter += (s, e) => element.BeginAnimation(OpacityProperty, Animation_Show);
            element.MouseLeave += (s, e) => element.BeginAnimation(OpacityProperty, Animation_Hide);
            SetZIndex(element, 0xffff);
        }

        private Rectangle CreateVerticalGrip()
        {
            Rectangle rect = new()
            {
                Width = GripThickness,
                Cursor = Cursors.SizeWE
            };
            rect.SetBinding(HeightProperty, new Binding(nameof(ActualHeight)) { Source = this, Converter = LengthConverter });
            InitElement(rect);
            return rect;
        }

        private Rectangle CreateHorizontalGrip()
        {
            Rectangle rect = new()
            {
                Height = GripThickness,
                Cursor = Cursors.SizeNS
            };
            rect.SetBinding(WidthProperty, new Binding(nameof(ActualWidth)) { Source = this, Converter = LengthConverter });
            InitElement(rect);
            return rect;
        }

        private void CreateEdgeGrip(Func<Rectangle> func, int dir, Action<Rectangle> pos)
        {
            var grip = func();
            grip.Tag = dir;
            pos(grip);
            Children.Add(grip);
            _grips.Add(dir, grip);
        }

        private void CreateGrip_Left()
        {
            CreateEdgeGrip(CreateVerticalGrip, 4, g =>
            {
                SetTop(g, GripSize);
                SetLeft(g, 0);
            });
        }

        private void CreateGrip_Right()
        {
            CreateEdgeGrip(CreateVerticalGrip, 6, g =>
            {
                SetTop(g, GripSize);
                SetRight(g, 0);
            });
        }

        private void CreateGrip_Top()
        {
            CreateEdgeGrip(CreateHorizontalGrip, 8, g =>
            {
                SetTop(g, 0);
                SetLeft(g, GripSize);
            });
        }

        private void CreateGrip_Bottom()
        {
            CreateEdgeGrip(CreateHorizontalGrip, 2, g =>
            {
                SetBottom(g, 0);
                SetLeft(g, GripSize);
            });
        }

        private static StreamGeometry CreateTriangleGeometry(double ox, double oy, double x1, double y1, double x2, double y2)
        {
            StreamGeometry sg = new();
            using (var ctx = sg.Open())
            {
                MediaUtils.DrawTriangle(ctx, ox * GripSize, oy * GripSize, x1 * GripSize, y1 * GripSize, x2 * GripSize, y2 * GripSize, true, false, false);
            }
            sg.Freeze();
            return sg;
        }

        private static StreamGeometry CreateDiagonalStripes(double vx, double vy, double hx, double hy, double dx, double dy)
        {
            StreamGeometry sg = new();
            using (var ctx = sg.Open())
            {
                for (int i = 0; i < 4; i++)
                {
                    ctx.BeginFigure(new(vx, vy), true, false);
                    ctx.LineTo(new(hx, hy), false, false);
                    hx += dx;
                    vy += dy;
                    ctx.LineTo(new(hx, hy), false, false);
                    ctx.LineTo(new(vx, vy), false, false);
                    hx += dx;
                    vy += dy;
                }
            }
            sg.Freeze();
            return sg;
        }

        private void CreateCornerGrip(int dir, Action<Path> pos, double ox, double oy, double dx, double dy)
        {
            Path path = new()
            {
                Data = CreateTriangleGeometry(ox, oy, dx, 0, 0, dy),
                Tag = dir,
            };
            InitElement(path);
            pos(path);
            Children.Add(path);
            _grips.Add(dir, path);
        }

        private void CreateGrip_LowerLeft()
        {
            CreateCornerGrip(1, g =>
            {
                g.Cursor = Cursors.SizeNESW;
                SetBottom(g, 0);
                SetLeft(g, 0);
            }, 0, 1, 1, -1);
        }

        private void CreateGrip_LowerRight()
        {
            CreateCornerGrip(3, g =>
            {
                g.Cursor = Cursors.SizeNWSE;
                SetBottom(g, 0);
                SetRight(g, 0);
            }, 1, 1, -1, -1);
        }

        private void CreateGrip_UpperLeft()
        {
            CreateCornerGrip(7, g =>
            {
                g.Cursor = Cursors.SizeNWSE;
                SetTop(g, 0);
                SetLeft(g, 0);
            }, 0, 0, 1, 1);
        }

        private void CreateGrip_UpperRight()
        {
            CreateCornerGrip(9, g =>
            {
                g.Cursor = Cursors.SizeNESW;
                SetTop(g, 0);
                SetRight(g, 0);
            }, 1, 0, -1, 1);
        }
    }
}
