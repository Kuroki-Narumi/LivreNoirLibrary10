using System;

namespace LivreNoirLibrary.Windows
{
    public interface IPipeApplication
    {
        public static abstract string PipeName { get; }
        public static virtual bool IsSingleton { get => false; }
        public static virtual bool ShowServerOnClientExit { get => false; }

        public void OnPipeClientStart(int processId, string[] args) { }
        public void OnPipeClientExit(int processId) { }
        public void OnPipeMessageRecieve(int processId, string message) { }
    }
}
