using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Windows
{
    public static partial class Pipe
    {
        private static CancellationTokenSource? _ct_source;
        private static bool _server_started;
        private static readonly HashSet<int> _sub_processes = [];

        private const string ClientStartMessage = "{B4A9E1AB-C3B6-4553-91A0-BA3C9976C9F0}";
        private const string ClientExitMessage = "{BAA1AF9E-1126-4511-9CE4-94D8F6161345}";

        private const string CaptureGroup_Args = "args";
        [GeneratedRegex($@"^{ClientStartMessage}(?<{CaptureGroup_Args}>.+)?", RegexOptions.CultureInvariant | RegexOptions.Singleline)]
        private static partial Regex Regex_ClientStart { get; }

        [STAThread]
        public static void SetupPipe<TApp>(this TApp app, StartupEventArgs e)
            where TApp : Application, IPipeApplication
        {
            if (TrySetupPipe(app))
            {
            }
            else if (TApp.IsSingleton)
            {
                SendStartMessage<TApp>(e);
                app.Shutdown();
            }
            else
            {
                SendStartMessageAsync<TApp>(e);
            }
        }

        [STAThread]
        public static bool TrySetupPipe<TApp>(this TApp app)
            where TApp : Application, IPipeApplication
        {
            if (ApplicationMutex.CreateMutex(app, TApp.PipeName, OnExit<TApp>))
            {
                StartPipeServer(app);
                return true;
            }
            return false;
        }

        private static void OnExit<TApp>(object sender, ExitEventArgs e)
            where TApp : IPipeApplication
        {
            _ct_source?.Cancel();
        }

        private static void StartPipeServer<TApp>(TApp app)
            where TApp : Application, IPipeApplication
        {
            if (_server_started)
            {
                return;
            }
            _server_started = true;
            _ct_source = new();
            ServerProcess(app, _ct_source.Token);
        }

        private static async void ServerProcess<TApp>(TApp app, CancellationToken c)
            where TApp : Application, IPipeApplication
        {
            var pipeName = TApp.PipeName;
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(int));
            try
            {
                while (true)
                {
                    using NamedPipeServerStream stream = new(pipeName, PipeDirection.In);
                    await stream.WaitForConnectionAsync(c);
                    var count = await stream.ReadAsync(buffer.AsMemory(0, sizeof(int)), c);
                    if (count is sizeof(int))
                    {
                        var processId = BitConverter.ToInt32(buffer);
                        using StreamReader reader = new(stream);
                        var message = await reader.ReadToEndAsync(c);
                        ProcessRecievedMessage(app, processId, message);
                    }
                    c.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private static void ProcessRecievedMessage<TApp>(TApp app, int processId, string message)
            where TApp : Application, IPipeApplication
        {
            if (message is ClientExitMessage)
            {
                if (_sub_processes.Remove(processId))
                {
                    //ExConsole.Write($"Client Exit; pId={processId}\nCurrent Clients=[{string.Join(", ", _sub_processes)}]");
                    app.OnPipeClientExit(processId);
                    if (_sub_processes.Count is 0)
                    {
                        app.Dispatcher.BeginInvoke(() =>
                        {
                            var window = app.MainWindow;
                            if (TApp.ShowServerOnClientExit)
                            {
                                window.Show();
                            }
                            else if (window.Visibility is not Visibility.Visible)
                            {
                                window.Close();
                            }
                        });
                    }
                }
            }
            else
            {
                var match = Regex_ClientStart.Match(message);
                if (match.Success)
                {
                    if (TApp.IsSingleton || _sub_processes.Add(processId))
                    {
                        if (!TApp.IsSingleton && Process.GetProcessById(processId) is Process p)
                        {
                            p.EnableRaisingEvents = true;
                            p.Exited += (s, e) => ProcessRecievedMessage(app, processId, ClientExitMessage);
                        }
                        var argsGroup = match.Groups[CaptureGroup_Args];
                        var args = argsGroup.Success ? Json.Parse<string[]>(argsGroup.Value) : [];
                        //ExConsole.Write($"Client Started; pId={processId}, args={argsGroup.Value}\nCurrent Clients=[{string.Join(", ", _sub_processes)}]");
                        app.OnPipeClientStart(processId, args);
                    }
                }
                else
                {
                    app.OnPipeMessageRecieve(processId, message);
                }
            }
        }

        public static void SendStartMessage<TApp>(StartupEventArgs e) where TApp : IPipeApplication => SendMessage<TApp>(CreateStartMessage(e));
        public static void SendStartMessageAsync<TApp>(StartupEventArgs e) where TApp : IPipeApplication => _ = SendMessageAsync<TApp>(CreateStartMessage(e));

        private static string CreateStartMessage(StartupEventArgs e)
        {
            var args = e.Args;
            return args.Length is 0 ? ClientStartMessage : $"{ClientStartMessage}{args.GetJsonText()}";
        }

        public static void SendMessage<TApp>(string message)
            where TApp : IPipeApplication
        {
            using NamedPipeClientStream stream = new(".", TApp.PipeName, PipeDirection.Out);
            stream.Connect();
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(int));
            try
            {
                BitConverter.TryWriteBytes(buffer, Environment.ProcessId);
                stream.Write(buffer, 0, sizeof(int));
                using StreamWriter writer = new(stream);
                writer.Write(message);
                writer.Flush();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static async Task SendMessageAsync<TApp>(string message)
            where TApp : IPipeApplication
        {
            using NamedPipeClientStream stream = new(".", TApp.PipeName, PipeDirection.Out);
            await stream.ConnectAsync();
            var buffer = ArrayPool<byte>.Shared.Rent(sizeof(int));
            try
            {
                BitConverter.TryWriteBytes(buffer, Environment.ProcessId);
                await stream.WriteAsync(buffer.AsMemory(0, sizeof(int)));
                using StreamWriter writer = new(stream);
                await writer.WriteAsync(message);
                await writer.FlushAsync();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        /// <summary>
        /// Check if the window can be closed, and if not, set <see cref="CancelEventArgs.Cancel"/> to <see cref="bool">true</see>.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e">Pass through the argument of <see cref="Window.OnClosing"/></param>.
        /// <returns>
        /// <see cref="bool">true</see> if the window can be closed; otherwise <see cref="bool">false</see>.
        /// </returns>
        public static bool CheckServerClose(this Window window, CancelEventArgs e)
        {
            if (ApplicationMutex.HasHandle && _sub_processes.Count is > 0)
            {
                window.Hide();
                e.Cancel = true;
                return false;
            }
            return true;
        }
    }
}
