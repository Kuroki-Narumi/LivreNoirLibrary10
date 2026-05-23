using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_Icon.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Icon : UserControl
    {
        private readonly IconBitmapEncoder _encoder = new();

        public Unit_Icon()
        {
            DataContext = this;
            InitializeComponent();
            CreateColorPalette();
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            if (ComboBox_IconList.SelectedItem is IconInfo info &&
                this.SaveFileDialog(filters: Filters.Icon) is string path)
            {
                var encoder = _encoder;
                var icon = info.Drawing;
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 16));
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 24));
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 32));
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 64));
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 128));
                encoder.Add(Bitmap.GetSourceFromDrawing(icon, width: 256));
                using var writer = new BinaryWriter(File.Create(path));
                encoder.Save(writer);
                encoder.Clear();
            }
        }

        private void CreateColorPalette()
        {
            List<Color> colors = new(256);

            for (var y = 0; y < 16; y++)
            {
                var innerY = y % 4;
                for (var x = 0; x < 16; x++)
                {
                    var innerX = x % 4;
                    var group = GetGroup(x, y);
                    colors.Add(GetColor(group, innerX, innerY));
                }
            }

            ColorPalette.Colors = colors;

            static int GetGroup(int x, int y)
            {
                return (y / 4) * 4 + (x / 4);
            }

            static Color GetColor(int group, int x, int y)
            {
                if (group is 0)
                {
                    var index = y * 4 + x;
                    var value = (byte)(index * 17);
                    return Color.FromArgb(255, value, value, value);
                }
                else
                {
                    const int hueUnit = 360 / 15;
                    var h = (group - 1) * hueUnit;
                    var s = (x + 1) / 4f;
                    var v = (y + 1) / 4f;
                    var (r, g, b) = ColorUtils.CalcRGB(h, s, v);
                    return Color.FromArgb(255, ColorUtils.GetByte(r), ColorUtils.GetByte(g), ColorUtils.GetByte(b));
                }
            }
        }
    }

}
