using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Input
{
    [JsonConverter(typeof(KeyInputJsonConverter))]
    public readonly struct KeyInput(Key key, ModifierKeys modifier) : IEquatable<KeyInput>, IParsable<KeyInput>, ISpanParsable<KeyInput>
    {
        public readonly Key Key = key;
        public readonly ModifierKeys Modifier = modifier;

        public KeyInput(Key key) : this(key, Keyboard.Modifiers) { }
        public KeyInput(KeyInput key) : this(key.Key, key.Modifier) { }
        public KeyInput(KeyEventArgs e) : this(e.Key is Key.System ? e.SystemKey : e.Key, Keyboard.Modifiers) { }
        public KeyInput(int value) : this((Key)(value & 0xFF), (ModifierKeys)(value >> 8)) { }

        public bool Ctrl => ModGet(Modifier, ModifierKeys.Control);
        public bool Alt => ModGet(Modifier, ModifierKeys.Alt);
        public bool Shift => ModGet(Modifier, ModifierKeys.Shift);
        public bool Windows => ModGet(Modifier, ModifierKeys.Windows);

        public override string ToString() => ToString(Key, Modifier);
        public int ToInt() => (int)Key | ((int)Modifier << 8);

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is KeyInput input && Equals(input);

        public bool Equals(KeyInput other) => Key == other.Key && Modifier == other.Modifier;
        public override int GetHashCode() => HashCode.Combine(Key, Modifier);

        public static bool operator ==(KeyInput left, KeyInput right) => left.Equals(right);
        public static bool operator !=(KeyInput left, KeyInput right) => !left.Equals(right);

        public static string ToString(Key key, ModifierKeys modifier = 0)
        {
            return $"{(ModGet(modifier, ModifierKeys.Windows) ? "Win+" : "")}{(ModGet(modifier, ModifierKeys.Control) ? "Ctrl+" : "")}{(ModGet(modifier, ModifierKeys.Alt) ? "Alt+" : "")}{(ModGet(modifier, ModifierKeys.Shift) ? "Shift+" : "")}{GetKeyName(key)}";
        }

        public static string GetKeyName(Key key) => _key2name.TryGetValue(key, out var name) ? name : key.ToString();
        public static Key GetKey(ReadOnlySpan<char> text) => TryGetKey(text, out var key) ? key : Key.None;
        public static bool TryGetKey(ReadOnlySpan<char> text, out Key key) => _name2key.TryGetValue(text, out key) || Enum.TryParse(text, out key);
        public static bool TryGetModifier(ReadOnlySpan<char> text, out ModifierKeys modifierKeys) => _name2mod.TryGetValue(text, out modifierKeys);

        public static bool IsCtrlDown() => ModGet(Keyboard.Modifiers, ModifierKeys.Control);
        public static bool IsAltDown() => ModGet(Keyboard.Modifiers, ModifierKeys.Alt);
        public static bool IsShiftDown() => ModGet(Keyboard.Modifiers, ModifierKeys.Shift);
        public static bool IsWindowsDown() => ModGet(Keyboard.Modifiers, ModifierKeys.Windows);

        private static bool ModGet(ModifierKeys modifier, ModifierKeys target) => (modifier & target) is not ModifierKeys.None;

        public static bool IsTextKey(Key key) => GetType(key) is KeyType.Number or KeyType.Letter;

        public static KeyType GetType(Key key) => _keyTypes.TryGetValue(key, out KeyType value) ? value : KeyType.Other;

        public static bool IsSystemKey(Key key)
        {
            return GetType(key) switch
            {
                KeyType.System or KeyType.Ctrl or KeyType.Alt or KeyType.Shift or KeyType.Windows => true,
                _ => false,
            };
        }

        public static KeyInput Parse(string? s) => Parse(s.AsSpan(), null);
        public static KeyInput Parse(string? s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
        public static bool TryParse([NotNullWhen(true)] string? s, out KeyInput result) => TryParse(s.AsSpan(), null, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out KeyInput result) => TryParse(s.AsSpan(), provider, out result);

        public static KeyInput Parse(ReadOnlySpan<char> s) => Parse(s, null);
        public static KeyInput Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out var value))
            {
                return value;
            }
            return ThrowFormatException();
        }

        private static KeyInput ThrowFormatException() => throw new FormatException();

        public static bool TryParse(ReadOnlySpan<char> s, out KeyInput result) => TryParse(s, null, out result);
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out KeyInput result)
        {
            Key key = 0;
            ModifierKeys modifier = 0;
            var modDic = _name2mod;
            foreach (var range in s.Split('+'))
            {
                var value = s[range].Trim();
                if (modDic.TryGetValue(value, out var mod))
                {
                    modifier |= mod;
                }
                else if (TryGetKey(value, out var k))
                {
                    key = k;
                }
            }
            result = new KeyInput(key, modifier);
            return true;
        }

        private static readonly Dictionary<Key, string> _key2name = new()
        {
            [Key.None] = "",
            [Key.Enter] = nameof(Key.Enter),
            [Key.CapsLock] = nameof(Key.CapsLock),
            [Key.Escape] = "Esc",
            [Key.PageUp] = nameof(Key.PageUp),
            [Key.PageDown] = nameof(Key.PageDown),
            [Key.Left] = "←",
            [Key.Up] = "↑",
            [Key.Right] = "→",
            [Key.Down] = "↓",
            [Key.PrintScreen] = nameof(Key.PrintScreen),
            [Key.D0] = "0",
            [Key.D1] = "1",
            [Key.D2] = "2",
            [Key.D3] = "3",
            [Key.D4] = "4",
            [Key.D5] = "5",
            [Key.D6] = "6",
            [Key.D7] = "7",
            [Key.D8] = "8",
            [Key.D9] = "9",
            [Key.OemSemicolon] = "*",
            [Key.OemPlus] = "+",
            [Key.OemComma] = "<",
            [Key.OemMinus] = "-",
            [Key.OemPeriod] = ">",
            [Key.OemQuestion] = "?",
            [Key.OemTilde] = "@",
            [Key.OemOpenBrackets] = "[",
            [Key.OemPipe] = "|",
            [Key.OemCloseBrackets] = "]",
            [Key.OemQuotes] = "^",
            [Key.OemBackslash] = "_",
        };

        private static readonly Dictionary<string, Key>.AlternateLookup<ReadOnlySpan<char>> _name2key = _key2name.Invert(StringComparer.OrdinalIgnoreCase).GetAlternateLookup<ReadOnlySpan<char>>();

        private static readonly Dictionary<string, ModifierKeys>.AlternateLookup<ReadOnlySpan<char>> _name2mod = new Dictionary<string, ModifierKeys>(StringComparer.OrdinalIgnoreCase)
        {
            ["Ctrl"] = ModifierKeys.Control,
            ["Control"] = ModifierKeys.Control,
            ["Alt"] = ModifierKeys.Alt,
            ["Shift"] = ModifierKeys.Shift,
            ["Win"] = ModifierKeys.Windows,
            ["Windows"] = ModifierKeys.Windows,
        }.GetAlternateLookup<ReadOnlySpan<char>>();

        private static readonly Dictionary<Key, KeyType> _keyTypes = new()
        {
            { Key.None, KeyType.None },
            { Key.Cancel, KeyType.System },
            { Key.Back, KeyType.System },
            { Key.Tab, KeyType.System },
            { Key.LineFeed, KeyType.System },
            { Key.Clear, KeyType.System },
            { Key.Enter, KeyType.System },
            { Key.Pause, KeyType.System },
            { Key.CapsLock, KeyType.System },
            { Key.KanaMode, KeyType.System },
            { Key.JunjaMode, KeyType.System },
            { Key.FinalMode, KeyType.System },
            { Key.KanjiMode, KeyType.System },
            { Key.Escape, KeyType.System },
            { Key.ImeConvert, KeyType.System },
            { Key.ImeNonConvert, KeyType.System },
            { Key.ImeAccept, KeyType.System },
            { Key.ImeModeChange, KeyType.System },
            { Key.Space, KeyType.System },
            { Key.PageUp, KeyType.Move },
            { Key.PageDown, KeyType.Move },
            { Key.End, KeyType.Move },
            { Key.Home, KeyType.Move },
            { Key.Left, KeyType.Move },
            { Key.Up, KeyType.Move },
            { Key.Right, KeyType.Move },
            { Key.Down, KeyType.Move },
            { Key.Select, KeyType.System },
            { Key.Print, KeyType.System },
            { Key.Execute, KeyType.System },
            { Key.PrintScreen, KeyType.System },
            { Key.Insert, KeyType.Move },
            { Key.Delete, KeyType.Move },
            { Key.Help, KeyType.System },
            { Key.D0, KeyType.Number },
            { Key.D1, KeyType.Number },
            { Key.D2, KeyType.Number },
            { Key.D3, KeyType.Number },
            { Key.D4, KeyType.Number },
            { Key.D5, KeyType.Number },
            { Key.D6, KeyType.Number },
            { Key.D7, KeyType.Number },
            { Key.D8, KeyType.Number },
            { Key.D9, KeyType.Number },
            { Key.A, KeyType.Letter },
            { Key.B, KeyType.Letter },
            { Key.C, KeyType.Letter },
            { Key.D, KeyType.Letter },
            { Key.E, KeyType.Letter },
            { Key.F, KeyType.Letter },
            { Key.G, KeyType.Letter },
            { Key.H, KeyType.Letter },
            { Key.I, KeyType.Letter },
            { Key.J, KeyType.Letter },
            { Key.K, KeyType.Letter },
            { Key.L, KeyType.Letter },
            { Key.M, KeyType.Letter },
            { Key.N, KeyType.Letter },
            { Key.O, KeyType.Letter },
            { Key.P, KeyType.Letter },
            { Key.Q, KeyType.Letter },
            { Key.R, KeyType.Letter },
            { Key.S, KeyType.Letter },
            { Key.T, KeyType.Letter },
            { Key.U, KeyType.Letter },
            { Key.V, KeyType.Letter },
            { Key.W, KeyType.Letter },
            { Key.X, KeyType.Letter },
            { Key.Y, KeyType.Letter },
            { Key.Z, KeyType.Letter },
            { Key.LWin, KeyType.Windows },
            { Key.RWin, KeyType.Windows },
            { Key.Apps, KeyType.Application },
            { Key.Sleep, KeyType.System },
            { Key.NumPad0, KeyType.NumPad },
            { Key.NumPad1, KeyType.NumPad },
            { Key.NumPad2, KeyType.NumPad },
            { Key.NumPad3, KeyType.NumPad },
            { Key.NumPad4, KeyType.NumPad },
            { Key.NumPad5, KeyType.NumPad },
            { Key.NumPad6, KeyType.NumPad },
            { Key.NumPad7, KeyType.NumPad },
            { Key.NumPad8, KeyType.NumPad },
            { Key.NumPad9, KeyType.NumPad },
            { Key.Multiply, KeyType.NumPad },
            { Key.Add, KeyType.NumPad },
            { Key.Separator, KeyType.NumPad },
            { Key.Subtract, KeyType.NumPad },
            { Key.Decimal, KeyType.NumPad },
            { Key.Divide, KeyType.NumPad },
            { Key.F1, KeyType.Function },
            { Key.F2, KeyType.Function },
            { Key.F3, KeyType.Function },
            { Key.F4, KeyType.Function },
            { Key.F5, KeyType.Function },
            { Key.F6, KeyType.Function },
            { Key.F7, KeyType.Function },
            { Key.F8, KeyType.Function },
            { Key.F9, KeyType.Function },
            { Key.F10, KeyType.Function },
            { Key.F11, KeyType.Function },
            { Key.F12, KeyType.Function },
            { Key.F13, KeyType.Function },
            { Key.F14, KeyType.Function },
            { Key.F15, KeyType.Function },
            { Key.F16, KeyType.Function },
            { Key.F17, KeyType.Function },
            { Key.F18, KeyType.Function },
            { Key.F19, KeyType.Function },
            { Key.F20, KeyType.Function },
            { Key.F21, KeyType.Function },
            { Key.F22, KeyType.Function },
            { Key.F23, KeyType.Function },
            { Key.F24, KeyType.Function },
            { Key.NumLock, KeyType.System },
            { Key.Scroll, KeyType.System },
            { Key.LeftShift, KeyType.Shift },
            { Key.RightShift, KeyType.Shift },
            { Key.LeftCtrl, KeyType.Ctrl },
            { Key.RightCtrl, KeyType.Ctrl },
            { Key.LeftAlt, KeyType.Alt },
            { Key.RightAlt, KeyType.Alt },
            { Key.BrowserBack, KeyType.Browser },
            { Key.BrowserForward, KeyType.Browser },
            { Key.BrowserRefresh, KeyType.Browser },
            { Key.BrowserStop, KeyType.Browser },
            { Key.BrowserSearch, KeyType.Browser },
            { Key.BrowserFavorites, KeyType.Browser },
            { Key.BrowserHome, KeyType.Browser },
            { Key.VolumeMute, KeyType.Volume },
            { Key.VolumeDown, KeyType.Volume },
            { Key.VolumeUp, KeyType.Volume },
            { Key.MediaNextTrack, KeyType.Media },
            { Key.MediaPreviousTrack, KeyType.Media },
            { Key.MediaStop, KeyType.Media },
            { Key.MediaPlayPause, KeyType.Media },
            { Key.LaunchMail, KeyType.System },
            { Key.SelectMedia, KeyType.System },
            { Key.LaunchApplication1, KeyType.System },
            { Key.LaunchApplication2, KeyType.System },
            { Key.OemSemicolon, KeyType.Letter },
            { Key.OemPlus, KeyType.Letter },
            { Key.OemComma, KeyType.Letter },
            { Key.OemMinus, KeyType.Letter },
            { Key.OemPeriod, KeyType.Letter },
            { Key.OemQuestion, KeyType.Letter },
            { Key.OemTilde, KeyType.Letter },
            { Key.AbntC1, KeyType.Letter },
            { Key.AbntC2, KeyType.Letter },
            { Key.OemOpenBrackets, KeyType.Letter },
            { Key.OemPipe, KeyType.Letter },
            { Key.OemCloseBrackets, KeyType.Letter },
            { Key.OemQuotes, KeyType.Letter },
            { Key.Oem8, KeyType.Letter },
            { Key.OemBackslash, KeyType.Letter },
            { Key.ImeProcessed, KeyType.System },
            { Key.System, KeyType.System },
            { Key.OemAttn, KeyType.System },
            { Key.OemFinish, KeyType.System },
            { Key.OemCopy, KeyType.System },
            { Key.OemAuto, KeyType.System },
            { Key.OemEnlw, KeyType.System },
            { Key.OemBackTab, KeyType.System },
            { Key.Attn, KeyType.System },
            { Key.CrSel, KeyType.System },
            { Key.ExSel, KeyType.System },
            { Key.EraseEof, KeyType.System },
            { Key.Play, KeyType.System },
            { Key.Zoom, KeyType.System },
            { Key.NoName, KeyType.System },
            { Key.Pa1, KeyType.System },
            { Key.OemClear, KeyType.System },
            { Key.DeadCharProcessed, KeyType.System },
        };
    }
}
