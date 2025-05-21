using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Windows
{
    public static class ExceptionHandler
    {
        public static string? ErrorMessageHeader { get; set; } = "エラーが発生しました。";
        public static string? ErrorMessageFooter { get; set; } = "アプリフォルダ内にエラー情報を書き出しました。報告の際にご利用ください。";

        public static void SetupExceptionHandler(this Application app)
        {
            // UIスレッドの未処理例外で発生
            app.DispatcherUnhandledException += app.OnDispatcherUnhandledException;
            // UIスレッド以外の未処理例外で発生
            TaskScheduler.UnobservedTaskException += app.OnUnobservedTaskException;
            // それでも処理されない例外で発生
            AppDomain.CurrentDomain.UnhandledException += app.OnUnhandledException;
        }

        private static void OnDispatcherUnhandledException(this Application app, object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            Environment.Exit(1);
        }

        private static void OnUnobservedTaskException(this Application app, object? sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception.InnerException);
        }

        private static void OnUnhandledException(this Application app, object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
            Environment.Exit(1);
        }

        private static void HandleException(Exception? e)
        {
            var msg = SaveErrorDetails(e);
            MessageBox.Show(msg, WindowsExtensions.MessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static string SaveErrorDetails(this Exception? e)
        {
            Console.WriteLine(e);
            Console.WriteLine('\n');
            StringBuilder sb = new();
            sb.AppendLine($"exception type: {e?.GetType().FullName}");
            sb.AppendLine($"message: {e?.Message}");
            var st = e?.StackTrace?.Split('\n') ?? [""];
            var c = st.Length;
            sb.Append(st[0]);
            ExConsole.Write(sb.ToString());
            Application.Current.Dispatcher.Invoke(() => Clipboard.SetText(sb.ToString()));
            sb.AppendLine("stack trace:");
            for (int i = 1; i < c; i++)
            {
                sb.Append(st[i]);
            }
            var dir = Path.Join(IO.General.GetAssemblyDir(), "ErrorLogs");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fname = Path.Join(dir, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            File.WriteAllText(fname, sb.ToString());
            return CreateErrorMessage(e, fname, st);
        }

        private static string CreateErrorMessage(Exception? e, string fname, string[] stacktrace)
        {
            StringBuilder sb = new();
            if (!string.IsNullOrEmpty(ErrorMessageHeader))
            {
                sb.AppendLine(ErrorMessageHeader);
                sb.AppendLine();
            }
            sb.AppendLine($"{e?.GetType().FullName}: {e?.Message}");
            sb.AppendLine(stacktrace[0]);
            if (!string.IsNullOrEmpty(ErrorMessageFooter))
            {
                sb.AppendLine(string.Format(ErrorMessageFooter, fname));
            }
            return sb.ToString();
        }
    }
}
