using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// JsonViewWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class JsonViewWindow : FollowOwnerWindow
    {
        public const string DefaultCopyText = "Copy";
        public const string DefaultSaveAsText = "Save As...";

        public static readonly DependencyProperty SourceProperty = JsonView.SourceProperty.AddOwner(typeof(JsonViewWindow));

        [DependencyProperty]
        private string? _copyText = DefaultCopyText;
        [DependencyProperty]
        private string? _saveAsText = DefaultSaveAsText;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string? _saveFilePath;

        private static string? CoerceSaveFilePath(string? value)
        {
            if (value is not null && !ExtRegs.Json.IsMatch(value))
            {
                return Path.ChangeExtension(value, Exts.Json);
            }
            return value;
        }

        public object? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public JsonViewWindow()
        {
            DataContext = this;
            InitializeComponent();
            this.RegisterCommand(ApplicationCommands.Copy, Executed_Copy);
            this.RegisterCommand(ApplicationCommands.Save, Executed_Save);
            this.RegisterCommand(ApplicationCommands.SaveAs, Executed_Save);
        }

        private void Executed_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(JsonView.GetText());
        }

        private void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            var options = FileDialogOptions.WithInitialPath(SaveFilePath);
            if (this.SaveFileDialog(options, Filters.Json) is string path)
            {
                SaveFilePath = path;
                File.WriteAllText(path, JsonView.GetText());
            }
        }
    }
}
