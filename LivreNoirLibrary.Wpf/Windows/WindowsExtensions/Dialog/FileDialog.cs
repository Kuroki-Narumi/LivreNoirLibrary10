using System;
using System.Windows;
using Microsoft.Win32;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static string? OpenFileDialog(this Window owner, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            OpenFileDialog dialog = new();
            (options ?? FileDialogOptions.Default).Apply(dialog);
            dialog.Multiselect = false;
            dialog.Filter = string.Join('|', filters);
            return dialog.ShowDialog(owner) is true ? dialog.FileName : null;
        }

        public static string? OpenFileDialog(this DependencyObject element, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            return OpenFileDialog(Window.GetWindow(element), options, filters);
        }

        public static string[] OpenFilesDialog(this Window owner, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            OpenFileDialog dialog = new();
            (options ?? FileDialogOptions.Default).Apply(dialog);
            dialog.Multiselect = true;
            dialog.Filter = string.Join('|', filters);
            return dialog.ShowDialog(owner) is true ? dialog.FileNames : [];
        }

        public static string[] OpenFilesDialog(this DependencyObject element, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            return OpenFilesDialog(Window.GetWindow(element), options, filters);
        }

        public static string? SaveFileDialog(this Window owner, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            SaveFileDialog dialog = new();
            (options ?? FileDialogOptions.Default).Apply(dialog);
            dialog.Filter = string.Join('|', filters);
            return dialog.ShowDialog(owner) is true ? dialog.FileName : null;
        }

        public static string? SaveFileDialog(this DependencyObject element, FileDialogOptions? options = null, params ReadOnlySpan<string?> filters)
        {
            return SaveFileDialog(Window.GetWindow(element), options, filters);
        }

        public static string? OpenFolderDialog(this Window owner, FileDialogOptions? options = null)
        {
            OpenFolderDialog dialog = new();
            (options ?? FileDialogOptions.Default).Apply(dialog);
            return dialog.ShowDialog(owner) is true ? dialog.FolderName : null;
        }

        public static string? OpenFolderDialog(this DependencyObject element, FileDialogOptions? options = null)
        {
            return OpenFolderDialog(Window.GetWindow(element), options);
        }
    }
}
