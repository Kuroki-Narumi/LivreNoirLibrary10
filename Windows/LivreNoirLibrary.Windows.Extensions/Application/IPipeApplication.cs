using System;

namespace LivreNoirLibrary.Windows
{
    public interface IPipeApplication
    {
        abstract static string PipeName { get; }
        virtual static bool IsSingleton { get => false; }
        virtual static bool ShowServerOnClientExit { get => false; }

        void OnPipeClientStart(int processId, string[] args) { }
        void OnPipeClientExit(int processId) { }
        void OnPipeMessageRecieve(int processId, string message) { }
    }
}
