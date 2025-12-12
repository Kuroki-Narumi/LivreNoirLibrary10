using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public static class Colors
    {
        public static Color Background { get; } = Color.FromArgb(255, 0, 0, 0); // エディタ背景

        public static Color HeaderText { get; } = Color.FromRgb(128, 255, 128); // レーン名 文字色

        private const byte Note100 = 255;
        private const byte Note_75 = 200;
        private const byte Note_50 = 144;
        private const byte Note_25 = 100;
        private const byte Note__0 = 24;
        public static Color Note_Red { get; } = Color.FromArgb(255, Note100, Note__0, Note__0); // スクラッチ/赤ポップ
        public static Color Note_White { get; } = Color.FromArgb(255, Note_50, Note_50, Note_50); // 白鍵/白ポップ
        public static Color Note_Blue { get; } = Color.FromArgb(255, Note__0, Note__0, Note100); // 黒鍵/青ポップ
        public static Color Note_Green { get; } = Color.FromArgb(255, Note__0, Note100, Note__0); // 緑ポップ
        public static Color Note_Yellow { get; } = Color.FromArgb(255, Note_75, Note_75, Note__0); // 黄ポップ

        public static Color Note_Bgm { get; } = Color.FromRgb(Note_75, Note__0, Note_25); // BGM ノーツ
        public static Color Note_Bga { get; } = Color.FromRgb(Note__0, Note100, Note_50); // BGA ノーツ
        public static Color Note_Bpm { get; } = Color.FromRgb(Note100, Note_50, Note__0); // BPM ノーツ
        public static Color Note_Stop { get; } = Color.FromRgb(Note100, Note__0, Note_75); // STOP ノーツ
        public static Color Note_Scroll { get; } = Color.FromRgb(Note_75, Note_75, Note__0); // SCROLL ノーツ
        public static Color Note_Speed { get; } = Color.FromRgb(Note100, Note_50, Note_50); // SPEED ノーツ
        public static Color Note_Meta { get; } = Color.FromRgb(Note_50, Note__0, Note_75); // メタ ノーツ

        public static Color Note_Mine { get; } = Color.FromRgb(Note_75, Note_25, Note__0); // 地雷 ノーツ
        public static Color Note_LongEnd { get; } = Color.FromRgb(Note_75, Note_25, Note100); // ロングノーツ終端

        public static Color Note_Invalid { get; } = Color.FromArgb(255, 255, 0, 192); // 不正ノーツ

        private const byte Back100 = 24;
        private const byte Back_50 = 12;
        private const byte Back__0 = 0;
        public static Color Back_Red { get; } = Color.FromArgb(255, Back100, Back__0, Back__0); // スクラッチ/赤ポップ 背景
        public static Color Back_White { get; } = Color.FromArgb(255, Back_50, Back_50, Back_50); // 白鍵/白ポップ 背景
        public static Color Back_Blue { get; } = Color.FromArgb(255, Back__0, Back__0, Back100); // 黒鍵/青ポップ 背景
        public static Color Back_Green { get; } = Color.FromArgb(255, Back__0, Back100, Back__0); // 緑ポップ 背景
        public static Color Back_Yellow { get; } = Color.FromArgb(255, Back_50, Back_50, Back__0); // 黄ポップ 背景

        public static Color Back_Bgm { get; } = Color.FromArgb(255, Back100, Back__0, Back_50); // BGM 背景
        public static Color Back_Bga { get; } = Color.FromArgb(255, Back__0, Back100, Back_50); // BGA 背景
        public static Color Back_Bpm { get; } = Color.FromArgb(255, Back100, Back_50, Back__0); // BPM 背景
        public static Color Back_Stop { get; } = Color.FromArgb(255, Back100, Back__0, Back_50); // STOP 背景
        public static Color Back_Scroll { get; } = Color.FromArgb(255, Back100, Back100, Back__0); // SCROLL 背景
        public static Color Back_Speed { get; } = Color.FromArgb(255, Back100, Back_50, Back_50); // SPEED 背景
        public static Color Back_Meta { get; } = Color.FromArgb(255, Back_50, Back__0, Back_50); // メタ 背景

        private const byte Long100 = 180;
        private const byte Long_75 = 120;
        private const byte Long__0 = 60;
        public static Color Long_Red { get; } = Color.FromArgb(255, Long100, Long__0, Long__0); // スクラッチ/赤ポップ ロングノート
        public static Color Long_White { get; } = Color.FromArgb(255, Long_75, Long_75, Long_75); // 白鍵/白ポップ ロングノート
        public static Color Long_Blue { get; } = Color.FromArgb(255, Long__0, Long__0, Long100); // 黒鍵/青ポップ ロングノート
        public static Color Long_Green { get; } = Color.FromArgb(255, Long__0, Long100, Long__0); // 緑ポップ ロングノート
        public static Color Long_Yellow { get; } = Color.FromArgb(255, Long_75, Long_75, Long__0); // 黄ポップ ロングノート

        public static Color NoteHilight { get; } = Color.FromArgb(128, 255, 255, 255); // ノーツ ハイライト
        public static Color NoteShadow { get; } = Color.FromArgb(128, 0, 0, 0); // ノーツ 影

        public static Color Selected { get; } = Color.FromRgb(160, 255, 255); // ノーツ 選択中
        public static Color SelectedStroke { get; } = Color.FromRgb(255, 255, 255); // ノーツ 選択中 枠
        public static Color SelectedLong { get; } = Color.FromRgb(64, 192, 192); // ノーツ 選択中 ロング

        public static Color BarLine { get; } = Color.FromArgb(224, 255, 255, 255); // 小節線
        public static Color BarText { get; } = Color.FromArgb(48, 255, 255, 255); // 小節番号
        public static Color BeatLine { get; } = Color.FromArgb(72, 255, 255, 255); // 拍線
        public static Color SubBeatLine { get; } = Color.FromArgb(36, 255, 255, 255); // 拍線(小)
        public static Color LaneBorder { get; } = Color.FromArgb(96, 255, 255, 255); // レーン境界

        public static Color WaveForm { get; } = Color.FromRgb(192, 192, 224); // 波形プレビュー
    }
}
