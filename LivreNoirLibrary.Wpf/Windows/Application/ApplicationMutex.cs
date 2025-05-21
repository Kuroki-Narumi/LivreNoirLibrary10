using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static class ApplicationMutex
    {
        private static Mutex? _mutex;

        public static bool HasHandle { get; private set; }

        [STAThread]
        public static bool CreateMutex(this Application app, string mutexName, ExitEventHandler? onExit = null)
        {
            if (_mutex is null)
            {
                _mutex = new(false, mutexName);
                if (onExit is not null)
                {
                    app.Exit += onExit;
                }
                app.Exit += ReleaseMutex;
            }
            if (!HasHandle)
            {
                try
                {
                    HasHandle = _mutex.WaitOne(0, false);
                }
                catch (AbandonedMutexException)
                {
                    HasHandle = true;
                }
            }
            return HasHandle;
        }

        private static void ReleaseMutex(object sender, ExitEventArgs e)
        {
            if (HasHandle)
            {
                _mutex?.ReleaseMutex();
            }
            _mutex?.Dispose();
        }
    }
}
