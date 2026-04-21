using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.GameOfLife
{
    public class Settings : AppSettingsBase
    {
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color DeadCellColor { get => field; set => SetValue(ref field, value); } = Colors.Black;
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color LivingCellColor { get => field; set => SetValue(ref field, value); } = Colors.White;
    }
}
