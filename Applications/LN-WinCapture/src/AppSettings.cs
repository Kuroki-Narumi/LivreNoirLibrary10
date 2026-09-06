using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LivreNoir.WinCapture
{
    public class AppSettings : AppSettingsBase
    {
        public const string AppName = "WinCapture";

        public static AppSettings Instance { get; } = Load<AppSettings>(AppName);
        public static void Save() => Instance.SaveInstance(AppName);

        public WindowInfo WindowInfo { get; set => SetValue(ref field, value); } = new();
        public string? TargetTitle { get; set => SetValue(ref field, value); }
        public string? TargetFileName { get; set => SetValue(ref field, value); }
        public WindowSearchMode TargetSearchMode { get; set => SetValue(ref field, value); } = WindowSearchMode.TitleAndFile;
        public bool IsCursorCaptureEnabled { get; set => SetValue(ref field, value); }
        public bool CaptureClientArea { get; set => SetValue(ref field, value); } = true;
        public KeyInput CaptureHotKey { get; set => SetValue(ref field, value, [nameof(HotKeyText)]); }
        public ClipRect ClipRect { get; set => SetValue(ref field, value); } = new();
        public string? MaskPath { get; set => SetValue(ref field, value, UpdateMaskImage); }

        [JsonIgnore]
        public string HotKeyText => CaptureHotKey.Key is 0 ? "(select)" : CaptureHotKey.ToString();
        [JsonIgnore]
        public BitmapImage? MaskImage { get; private set => SetValue(ref field, value); }
        [JsonIgnore]
        public CapturedItemCollection CapturedItems { get; } = [];
        [JsonIgnore]
        public CapturedItem? SelectedItem { get; set => SetValue(ref field, value); }

        public void InitializeMaskImage()
        {
            UpdateMaskImage(null, MaskPath);
        }

        private void UpdateMaskImage(string? _, string? path)
        {
            MaskImage = File.Exists(path) ? Bitmap.GetSourceFromFile(path) : null;
        }
    }
}
