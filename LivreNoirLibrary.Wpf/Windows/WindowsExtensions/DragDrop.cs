using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Windows;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static string[] GetFileList(this DragEventArgs e) => e.Data.GetData(DataFormats.FileDrop) is string[] list ? list : [];

        public static bool IsAvailable(this DragEventArgs e, Regex acceptExt) => GetFileList(e).AnyMatch(acceptExt);
        public static bool IsAvailable(this DragEventArgs e, params ReadOnlySpan<Regex> acceptExts) => GetFileList(e).AnyMatch(acceptExts);

        public static bool TryGetAvailable(this DragEventArgs e, Regex acceptExt, [MaybeNullWhen(false)] out string path) => GetFileList(e).TryGetMatched(acceptExt, out path);
        public static bool TryGetAvailable(this DragEventArgs e, [MaybeNullWhen(false)] out string path, params ReadOnlySpan<Regex> acceptExts)
            => GetFileList(e).TryGetMatched(out path, acceptExts);

        public static IEnumerable<string> EnumAvailable(this DragEventArgs e, Regex acceptExt) => GetFileList(e).EnumMatched(acceptExt);
        public static IEnumerable<string> EnumAvailable(this DragEventArgs e, params Regex[] acceptExts) => GetFileList(e).EnumMatched(acceptExts);

        public static void ApplyEffect(this DragEventArgs e, Regex acceptExt, DragDropEffects effect = DragDropEffects.Copy)
        {
            if (!e.Handled)
            {
                e.Effects = IsAvailable(e, acceptExt) ? effect : DragDropEffects.None;
                e.Handled = true;
            }
        }

        public static void ApplyEffect(this DragEventArgs e, DragDropEffects effect = DragDropEffects.Copy, params ReadOnlySpan<Regex> acceptExts)
        {
            if (!e.Handled)
            {
                e.Effects = IsAvailable(e, acceptExts) ? effect : DragDropEffects.None;
                e.Handled = true;
            }
        }
    }
}
