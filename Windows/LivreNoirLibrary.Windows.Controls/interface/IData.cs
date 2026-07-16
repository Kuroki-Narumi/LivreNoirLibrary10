using LivreNoirLibrary.IO;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IDataNew
    {
        bool CanNew { get; }
        void ExecuteNewData();
    }

    public interface IDataOpen
    {
        bool CanOpen { get; }
        FileDialogOptions? OpenDialogOptions { get; }
        string OpenFilter { get; }
        void ExecuteOpenData(string path);
    }

    public interface IDataSave
    {
        bool CanSave { get; }
        FileDialogOptions? SaveDialogOptions { get; }
        string SaveFilter { get; }
        string? SaveFilePath { get; }
        void ExecuteSaveData(string path);
    }

    public static class IDataExtensions
    {
        public static void RegisterNewCommand<T>(this T obj) where T : UIElement, IDataNew => obj.RegisterCommand(ApplicationCommands.New, obj.Executed_New, obj.CanExecute_New);
        public static void RegisterOpenCommand<T>(this T obj) where T : UIElement, IDataOpen => obj.RegisterCommand(ApplicationCommands.Open, obj.Executed_Open, obj.CanExecute_Open);
        public static void RegisterSaveCommand<T>(this T obj) where T : UIElement, IDataSave => obj.RegisterCommand(ApplicationCommands.Save, obj.Executed_Save, obj.CanExecute_Save);
        public static void RegisterSaveAsCommand<T>(this T obj) where T : UIElement, IDataSave => obj.RegisterCommand(ApplicationCommands.SaveAs, obj.Executed_SaveAs, obj.CanExecute_Save);

        private static void CanExecute_New(this IDataNew obj, object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = obj.CanNew;
        private static void CanExecute_Open(this IDataOpen obj, object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = obj.CanOpen;
        private static void CanExecute_Save(this IDataSave obj, object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = obj.CanSave;

        private static void Executed_New(this IDataNew obj, object sender, ExecutedRoutedEventArgs e)
        {
            if (obj.CanNew)
            {
                e.Handled = true;
                obj.ExecuteNewData();
            }
        }

        private static void Executed_Open<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, IDataOpen
        {
            if (obj.CanOpen)
            {
                e.Handled = true;
                if (obj.OpenFileDialog(obj.OpenDialogOptions, obj.OpenFilter) is { } path)
                {
                    obj.ExecuteOpenData(path);
                }
            }
        }

        private static void Executed_Save<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, IDataSave
        {
            if (obj.CanSave)
            {
                e.Handled = true;
                var path = obj.SaveFilePath;
                if (string.IsNullOrEmpty(path))
                {
                    SaveWithDialog(obj);
                }
                else
                {
                    obj.ExecuteSaveData(path);
                }
            }
        }

        private static void Executed_SaveAs<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, IDataSave
        {
            if (obj.CanSave)
            {
                e.Handled = true;
                SaveWithDialog(obj);
            }
        }

        private static void SaveWithDialog<T>(this T obj)
            where T : DependencyObject, IDataSave
        {
            if (obj.SaveFileDialog(obj.SaveDialogOptions, obj.SaveFilter) is { } path)
            {
                obj.ExecuteSaveData(path);
            }
        }
    }
}
