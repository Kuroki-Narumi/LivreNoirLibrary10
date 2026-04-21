using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Converters;

namespace LNClock
{
    public class MainViewModel : AppSettingsBase
    {
        public event EventHandler<int>? UpdateIntervalChanged;

        public static MainViewModel Instance { get; } = Load<MainViewModel>(nameof(LNClock));
        public static void Save() => Instance.SaveInstance(nameof(LNClock));

        public WindowInfo WindowInfo { get => field; set => SetValue(ref field, value); } = new();
        public bool Topmost { get => field; set => SetValue(ref field, value); }
        public bool ShowInTaskbar { get => field; set => SetValue(ref field, value); }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color Background { get => field; set => SetValue(ref field, value); } = "#FF000077".ToColor();
        public double BackgroundOpacity { get => field; set => SetValue(ref field, value); } = 0.5;
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color Foreground { get => field; set => SetValue(ref field, value); } = "#FFFFFF".ToColor();

        public string FontName { get => field; set => SetValue(ref field, value, OnFontNameChanged); } = "Meiryo UI";
        public string StringFormat { get => field; set => SetValue(ref field, value); } = "MM月dd日(ddd) HH:mm:ss";
        public int UpdateInterval { get => field; set => SetValue(ref field, value, OnUpdateIntervalChanged); } = 1000;

        [JsonIgnore]
        public FontFamily? FontFamily { get => field; set => SetValue(ref field, value); }
        [JsonIgnore]
        public string CurrentText { get => field; set => SetValue(ref field, value); } = "";

        protected override void OnLoad()
        {
            base.OnLoad();
            OnFontNameChanged("", FontName);
        }

        private void OnUpdateIntervalChanged(int _, int value)
        {
            UpdateIntervalChanged?.Invoke(this, value);
        }

        private void OnFontNameChanged(string _, string value)
        {
            FontFamily = new FontFamily(value);
        }
    }
}
