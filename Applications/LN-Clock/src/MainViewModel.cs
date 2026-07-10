using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Converters;

namespace LivreNoir.Clock
{
    public class MainViewModel : AppSettingsBase
    {
        public const double MinWidth = 128;
        public const double MaxWidth = 1280;
        public const double MinHeight = 16;
        public const double MaxHeight = 960;

        public const bool DefaultTopMost = true;
        public const bool DefaultShowInTaskbar = false;
        public static Color DefaultBackground { get; } = "#80000077".ToColor();
        public static Color DefaultForeground { get; } = "#FFFFFF".ToColor();
        public const string DefaultStringFormat = "MM月dd日(ddd) HH:mm:ss";
        public const int DefaultUpdateInterval = 1000;
        public static FontFamily DefaultFontFamily { get; } = new("Meiryo UI");

        public event EventHandler<int>? UpdateIntervalChanged;

        public static MainViewModel Instance { get; } = Load<MainViewModel>(nameof(Clock));
        public static void Save() => Instance.SaveInstance(nameof(Clock));

        public double Left { get; set => SetValue(ref field, value); } = double.NaN;
        public double Top { get; set => SetValue(ref field, value); } = double.NaN;
        public double Width { get; set => SetValue(ref field, value); } = double.NaN;
        public double Height { get; set => SetValue(ref field, value); } = double.NaN;
        public bool Topmost { get; set => SetValue(ref field, value); } = DefaultTopMost;
        public bool ShowInTaskbar { get; set => SetValue(ref field, value); } = DefaultShowInTaskbar;

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color Background { get; set => SetValue(ref field, value); } = DefaultBackground;
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color Foreground { get; set => SetValue(ref field, value); } = DefaultForeground;

        [JsonIgnore]
        public FontFamily? FontFamily { get; set => SetValue(ref field, value, [nameof(FontName)]); } = DefaultFontFamily;
        public string? FontName
        {
            get => FontFamily?.Source;
            set
            {
                if (value != FontFamily?.Source)
                {
                    FontFamily = new(value);
                }
            }
        }

        public string StringFormat { get; set => SetValue(ref field, value); } = DefaultStringFormat;
        public int UpdateInterval { get; set => SetValue(ref field, value, OnUpdateIntervalChanged); } = DefaultUpdateInterval;

        [JsonIgnore]
        public string CurrentText { get; set => SetValue(ref field, value); } = "";

        private void OnUpdateIntervalChanged(int _, int value)
        {
            UpdateIntervalChanged?.Invoke(this, value);
        }

        public void SetDefault()
        {
            Topmost = DefaultTopMost;
            ShowInTaskbar = DefaultShowInTaskbar;
            Background = DefaultBackground;
            Foreground = DefaultForeground;
            FontFamily = DefaultFontFamily;
            StringFormat = DefaultStringFormat;
            UpdateInterval = DefaultUpdateInterval;
        }
    }
}
