using System;
using System.Diagnostics;
using System.IO.Compression;

namespace LivreNoirLibrary.Updater
{
    internal class Updater
    {
        public const string Continue = "(キーを押して続行 / Press any key to continue)";
        public const string Exit = "(キーを押して終了 / Press any key to close)";
        public const string Details = "(Enter で詳細を表示、それ以外で終了 / Press Enter to show details, any other key to close)";

        public const string Message_NeedArgs = "引数を指定して起動してください。/ This app needs an argument.";
        public const string Message_Close = "以下のアプリを全て閉じてください。/ Close all apps below.";

        public const string Message_Downloading = "download stream...";
        public const string Message_DownloadFailed = "データの取得に失敗 / Failed to get the data";

        public const string Message_CopyFiles = "copy files...";
        public const string Message_ExtendFailed = "{0} の展開に失敗 / Failed to extend {0}";
        public const string Message_Completed = "アップデートが完了しました。/ Update completed.";
        public const string Message_Incompleted = "アップデートは正常に完了しませんでした。/ Update didn't complete successfully.";

        static readonly HttpClient Client = new();
        static readonly List<Exception> Errors = [];

        private static string _dir = "";

        public static void Run(string url, string exePath)
        {
            _dir = Path.GetDirectoryName(exePath)!;
            while (CheckProcess(out var list))
            {
                AlertAndWait($"{Message_Close}\n{string.Join(" / ", list)}", Continue);
            }
            Errors.Clear();
            Console.WriteLine(Message_Downloading);
            if (GetStream(url) is Stream s)
            {
                Console.WriteLine(Message_CopyFiles);
                try
                {
                    ProcessUpdate(s);
                }
                catch (Exception e)
                {
                    Errors.Add(e);
                }
            }
            else
            {
                Console.WriteLine(Message_DownloadFailed);
            }
            if (Errors.Count <= 0)
            {
                AlertAndWait(Message_Completed);
            }
            else
            {
                var info = AlertAndWait(Message_Incompleted, Details);
                if (info.Key is ConsoleKey.Enter)
                {
                    Console.WriteLine(string.Join('\n', Errors));
                    Console.WriteLine(Exit);
                    Console.ReadKey();
                }
            }
            Process.Start(exePath);
        }

        public static void Run(string[] args)
        {
            if (args.Length < 2)
            {
                AlertAndWait(Message_NeedArgs);
                return;
            }
            _dir = Path.GetDirectoryName(args[1])!;
            while (CheckProcess(out var list))
            {
                AlertAndWait($"{Message_Close}\n{string.Join(" / ", list)}", Continue);
            }
            Errors.Clear();
            Console.WriteLine(Message_Downloading);
            if (GetStream(args[0]) is Stream s)
            {
                Console.WriteLine(Message_CopyFiles);
                try
                {
                    ProcessUpdate(s);
                }
                catch (Exception e)
                {
                    Errors.Add(e);
                }
            }
            if (Errors.Count <= 0)
            {
                AlertAndWait(Message_Completed);
            }
            else
            {
                var info = AlertAndWait(Message_Incompleted, Details);
                if (info.Key is ConsoleKey.Enter)
                {
                    Console.WriteLine(string.Join("\n", Errors));
                    Console.WriteLine(Exit);
                    Console.ReadKey();
                }
            }
        }

        private static bool CheckProcess(out List<string> list)
        {
            list = [];
            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    var dir = Path.GetDirectoryName(proc.MainModule?.FileName);
                    if (!string.IsNullOrEmpty(dir) && dir.StartsWith(_dir))
                    {
                        var name = proc.MainWindowTitle;
                        list.Add(name);
                    }
                }
                catch { }
            }
            return list.Count > 0;
        }

        public static ConsoleKeyInfo AlertAndWait(string message, string? waiter = null)
        {
            Console.WriteLine(message);
            Console.WriteLine(waiter ?? Exit);
            return Console.ReadKey();
        }

        private static Stream? GetStream(string url)
        {
            try
            {
                var task = Client.GetStreamAsync(url);
                task.Wait();
                return task.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(Message_DownloadFailed);
                Errors.Add(e);
                return null;
            }
        }

        private static void ProcessUpdate(Stream stream)
        {
            using var zip = new ZipArchive(stream);
            var list = zip.Entries;
            var count = list.Count;
            if (count > 1)
            {
                var srcDir = list[0].FullName.Split('/')[0];
                for (int i = 1; i < count; i++)
                {
                    var entry = list[i];
                    var destPath = Path.GetFullPath(Path.GetRelativePath(srcDir, entry.FullName), _dir);
                    if (destPath.EndsWith('\\') || destPath.EndsWith('/'))
                    {
                        Directory.CreateDirectory(destPath);
                        continue;
                    }
                    if (entry.LastWriteTime > File.GetLastWriteTime(destPath))
                    {
                        try
                        {
                            using var srcStream = entry.Open();
                            try
                            {
                                using var destStream = File.Create(destPath);
                                srcStream.CopyTo(destStream);
                                Console.WriteLine($"\"{destPath}\"");
                            }
                            catch (IOException e)
                            {
                                Errors.Add(e);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(string.Format(Message_ExtendFailed, destPath));
                            Errors.Add(e);
                        }
                    }
                }
            }
        }
    }
}
